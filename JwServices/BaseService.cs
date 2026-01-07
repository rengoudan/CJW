using JwData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace JwServices
{
    public abstract class BaseService
    {
        protected readonly IDbContextFactory<JwData.JwDataContext> ContextFactory;

        public BaseService(IDbContextFactory<JwData.JwDataContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }


        protected JwDataContext CreateContext() => ContextFactory.CreateDbContext();

        // ✅ 获取所有实体（可选条件、排序、包含）
        protected async Task<List<T>> GetAllAsync<T>( Expression<Func<T, bool>>? predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
            params Expression<Func<T, object>>[] includes ) where T : class 
        { 
            using var context = CreateContext(); 
            IQueryable<T> query = context.Set<T>(); 
            foreach (var include in includes) query = query.Include(include); 
            if (predicate != null) 
                query = query.Where(predicate); 
            if (orderBy != null) 
                query = orderBy(query); 
            return await query.ToListAsync(); 
        }

        protected  List<T> GetAll<T>(Expression<Func<T, bool>>? predicate = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          params Expression<Func<T, object>>[] includes) where T : class
        {
            using var context = CreateContext();
            IQueryable<T> query = context.Set<T>();
            foreach (var include in includes) query = query.Include(include);
            if (predicate != null)
                query = query.Where(predicate);
            if (orderBy != null)
                query = orderBy(query);
            return query.ToList();
        }

        // ✅ 获取单个实体（可选包含）
        protected async Task<T?> GetByIdAsync<T>( object id, params Expression<Func<T, object>>[] includes ) where T : class 
        { 
            using var context = CreateContext(); 
            var entity = await context.Set<T>().FindAsync(id); 
            if (entity == null) 
                return null; 
            foreach (var include in includes) 
            { 
                context.Entry(entity).Reference(include).Load(); 
            } 
            return entity; 
        }

        protected async Task<T?> FindAsync<T>(
    Expression<Func<T, bool>> predicate
) where T : class
        {
            using var context = CreateContext();
            IQueryable<T> query = context.Set<T>();


            return await query.FirstOrDefaultAsync(predicate);
        }


        // ✅ 新增实体
        protected async Task AddAsync<T>(T entity) where T : class 
        { 
            using var context = CreateContext(); 
            context.Set<T>().Add(entity); 
            await context.SaveChangesAsync(); 
        }

        protected async Task<bool> DeleteAsync<T>(object id) where T : class 
        { 
            using var context = CreateContext(); 
            var entity = await context.Set<T>().FindAsync(id); 
            if (entity == null) 
                return false; 
            context.Set<T>().Remove(entity); 
            await context.SaveChangesAsync(); 
            return true; 
        }

        protected async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using var context = CreateContext();
            var entities = context.Set<T>().Where(predicate);
            context.Set<T>().RemoveRange(entities);
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// 原有方法 如果在context = CreateContext(); 生成新的上下文
        /// 如果单纯使用update 无法更新
        /// 方法1 ：Attach + Modified
        /// 方法2：查询和更新使用同一个上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task UpdateAsync<T>(T entity) where T : class 
        { 
            using var context = CreateContext();
            context.Set<T>().Attach(entity); 
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(); 
        }

        protected async Task<bool> UpdateFieldsAsync<T>(
     object id,
     Dictionary<Expression<Func<T, object>>, object> updates
 ) where T : class
        {
            using var context = CreateContext();
            var entity = await context.Set<T>().FindAsync(id);
            if (entity == null) return false;

            foreach (var kvp in updates)
            {
                var expr = kvp.Key.Body;

                // 处理可能的 Convert 表达式（值类型被装箱）
                if (expr is UnaryExpression unary && unary.Operand is MemberExpression member1)
                    expr = member1;
                else if (expr is MemberExpression member2)
                    expr = member2;
                else
                    throw new ArgumentException("属性表达式无效，必须是直接属性访问", nameof(updates));

                var memberExpr = (MemberExpression)expr;
                var propertyInfo = typeof(T).GetProperty(memberExpr.Member.Name);
                if (propertyInfo == null || !propertyInfo.CanWrite)
                    throw new InvalidOperationException($"属性 {memberExpr.Member.Name} 不可写");

                propertyInfo.SetValue(entity, kvp.Value);
            }

            await context.SaveChangesAsync();
            return true;
        }

        protected async Task SaveAsync(JwDataContext context) 
        { 
            await context.SaveChangesAsync(); 
        }


        protected async Task LoadCollectionAsync<TEntity, TProperty>(JwDataContext context, TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> navigation) where TEntity : class where TProperty : class 
        { 
            context.Attach(entity); 
            await context.Entry(entity).Collection(navigation).LoadAsync(); 
        }
        protected async Task LoadCollectionAsync<TEntity, TProperty>(
    object id,
    Expression<Func<TEntity, IEnumerable<TProperty>>> navigation
) where TEntity : class where TProperty : class
{
    using var context = CreateContext();
    var entity = await context.Set<TEntity>().FindAsync(id);
    if (entity == null) return;

    await context.Entry(entity).Collection(navigation).LoadAsync();
}


        protected async Task LoadReferenceAsync<TEntity, TProperty>(JwDataContext context, TEntity entity, Expression<Func<TEntity, TProperty?>> navigation) 
            where TEntity : class 
            where TProperty : class 
        { 
            context.Attach(entity); 
            await context.Entry(entity).Reference(navigation).LoadAsync(); 
        }

    }
}
