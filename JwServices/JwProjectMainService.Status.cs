using Microsoft.EntityFrameworkCore;

namespace JwServices
{
    /// <summary>
    /// 2026年11月22日 階の状態列（未解析／解析済／要確認／CSV出力済）用。
    /// 读取部分只做统计，不改任何数据；ExportCount 只在 CSV 导出成功后 +1（CSV 内容不变）。
    /// </summary>
    public sealed record FloorCheck(string SubId, long MainId, int PillarRows, int BeamRows, int BeamsWithoutHoles)
    {
        /// <summary>柱が1本も無い・梁が1本も無い・穴が1つも無い梁がある、のいずれか。</summary>
        public bool NeedsCheck => PillarRows == 0 || BeamRows == 0 || BeamsWithoutHoles > 0;

        /// <summary>要確認の理由（表示用）。</summary>
        public string Reason
        {
            get
            {
                var parts = new List<string>();
                if (BeamRows == 0) parts.Add("梁なし");
                if (PillarRows == 0) parts.Add("柱なし");
                if (BeamsWithoutHoles > 0) parts.Add($"穴なし梁 {BeamsWithoutHoles}本");
                return string.Join("・", parts);
            }
        }
    }

    public partial class JwProjectMainService
    {
        /// <summary>全階の確認用集計（読み取りのみ、追跡しない）。</summary>
        public async Task<List<FloorCheck>> GetFloorChecksAsync()
        {
            var q = context.JwProjectSubDatas.AsNoTracking().Select(s => new
            {
                s.Id,
                s.JwProjectMainDataId,
                Pillars = context.JwPillarDatas.Count(p => p.JwProjectSubDataId == s.Id),
                Beams = context.JwBeamDatas.Count(b => b.JwProjectSubDataId == s.Id),
                NoHole = context.JwBeamDatas.Count(b => b.JwProjectSubDataId == s.Id
                                                       && !context.JwHoleDatas.Any(h => h.JwBeamDataId == b.Id)),
            });
            var rows = await q.ToListAsync();
            return rows.Select(r => new FloorCheck(r.Id, r.JwProjectMainDataId, r.Pillars, r.Beams, r.NoHole)).ToList();
        }

        /// <summary>CSV 導出に成功したときに呼ぶ。更新後の回数を返す（階が見つからなければ -1）。</summary>
        public Task<int> IncrementExportCountAsync(string subId) => WriteAsync(async () =>
        {
            var sub = await context.JwProjectSubDatas.FindAsync(subId);
            if (sub == null) return -1;
            sub.ExportCount += 1;
            await context.SaveChangesAsync();
            return sub.ExportCount;
        });
    }
}
