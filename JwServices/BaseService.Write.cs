using System.Collections;
using System.Runtime.CompilerServices;
using JwLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JwServices
{
    /// <summary>
    /// 2026年11月21日 写操作的事务与失败恢复。
    ///
    /// 背景：ServiceFactory 让所有服务共享同一个 DbContext。原来写操作没有事务，一次失败会：
    ///   1) 在数据库里留下"半截"数据（如楼层已插入、梁只插入了一部分）；
    ///   2) 在共享上下文里残留 Added/Modified 实体，之后任何一次 SaveChanges 都会带着它们再次失败。
    ///
    /// WriteAsync：最外层调用开启事务；成功提交；失败回滚，并把变更跟踪器恢复到操作开始前的状态：
    ///   - 本次操作中新跟踪的实体 → 分离（Detach），并从已有实体的集合导航中移除，避免界面出现"幽灵"数据；
    ///   - 操作前已存在、但在操作中状态发生过变化（或仍未保存）的实体 → 从数据库重新加载（Reload）。
    /// 嵌套调用（已有事务时）直接执行，由最外层统一提交/回滚。
    /// </summary>
    public abstract partial class BaseService
    {
        protected Task WriteAsync(Func<Task> work, [CallerMemberName] string operation = "")
            => WriteAsync<object?>(async () => { await work(); return null; }, operation);

        protected async Task<T> WriteAsync<T>(Func<Task<T>> work, [CallerMemberName] string operation = "")
        {
            if (context.Database.CurrentTransaction != null)
            {
                return await work(); // 嵌套：交给最外层处理
            }

            var tracker = context.ChangeTracker;
            var before = new HashSet<object>(tracker.Entries().Select(e => e.Entity), ReferenceEqualityComparer.Instance);
            var touched = new HashSet<object>(ReferenceEqualityComparer.Instance);
            void OnStateChanged(object? s, EntityStateChangedEventArgs e)
            {
                if (before.Contains(e.Entry.Entity)) touched.Add(e.Entry.Entity);
            }

            tracker.StateChanged += OnStateChanged;
            try
            {
                await using var tx = await context.Database.BeginTransactionAsync();
                try
                {
                    var result = await work();
                    await tx.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    AppLog.Error(ex, "DB書き込み失敗 {Operation}、ロールバックします", operation);
                    try { await tx.RollbackAsync(); }
                    catch (Exception rbEx) { AppLog.Error(rbEx, "ロールバック失敗 {Operation}", operation); }

                    tracker.StateChanged -= OnStateChanged; // 恢复过程中的状态变化不再记录
                    RestoreTracker(before, touched, operation);
                    throw;
                }
            }
            finally
            {
                tracker.StateChanged -= OnStateChanged;
            }
        }

        private void RestoreTracker(HashSet<object> before, HashSet<object> touched, string operation)
        {
            var tracker = context.ChangeTracker;
            int detached = 0, reloaded = 0;

            // 1) 新跟踪的实体：先记下它们挂在哪些"已有实体"的集合里，分离后再从集合中移除
            var added = tracker.Entries().Where(e => !before.Contains(e.Entity)).ToList();
            var removals = new List<(IList list, object item)>();
            foreach (var entry in added)
            {
                foreach (var r in entry.References)
                {
                    var principal = r.CurrentValue;
                    if (principal == null || !before.Contains(principal)) continue;
                    if (r.Metadata is INavigation nav && nav.Inverse is { IsCollection: true } inverse)
                    {
                        try
                        {
                            if (context.Entry(principal).Collection(inverse.Name).CurrentValue is IList list && list.Contains(entry.Entity))
                                removals.Add((list, entry.Entity));
                        }
                        catch (Exception ex) { AppLog.Warn(ex, "ナビゲーションの復元に失敗 {Nav}", inverse.Name); }
                    }
                }
            }
            foreach (var entry in added)
            {
                entry.State = EntityState.Detached;
                detached++;
            }
            foreach (var (list, item) in removals)
            {
                list.Remove(item);
            }

            // 2) 原有实体：操作中被改动过（含已在事务内保存、随后被回滚的），或仍处于未保存状态 → 以数据库为准重新加载
            foreach (var entity in before)
            {
                var entry = context.Entry(entity);
                if (!touched.Contains(entity) && entry.State == EntityState.Unchanged) continue;
                try
                {
                    if (entry.State == EntityState.Detached)
                    {
                        // 在事务内被删除并保存后变为 Detached；回滚后数据库里仍存在，重新附加
                        entry.State = EntityState.Unchanged;
                    }
                    entry.Reload();
                    reloaded++;
                }
                catch (Exception ex)
                {
                    AppLog.Warn(ex, "エンティティの再読み込みに失敗 {Type}", entity.GetType().Name);
                    try { entry.State = EntityState.Detached; } catch { /* 已无法恢复，至少不再参与后续保存 */ }
                }
            }

            AppLog.Info("変更追跡を復元 {Operation}: detached={Detached} reloaded={Reloaded}", operation, detached, reloaded);
        }
    }
}
