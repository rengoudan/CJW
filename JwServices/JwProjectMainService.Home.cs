using JwCore;
using Microsoft.EntityFrameworkCore;

namespace JwServices
{
    /// <summary>ワークベンチ（ホーム）用の工事 1 件。</summary>
    public sealed record HomeProject(long Id, string Name, string Customer, int FloorQuantity, ProjectStatus Status, DateTime Created);

    /// <summary>ワークベンチ用の階 1 件。</summary>
    public sealed record HomeFloor(string Id, long MainId, string FloorName, DateTime Created, int ExportCount);

    /// <summary>ワークベンチの集計（読み取りのみ）。</summary>
    public sealed class HomeSummary
    {
        public List<HomeProject> Projects { get; init; } = new();
        public List<HomeFloor> Floors { get; init; } = new();
        public List<FloorCheck> Checks { get; init; } = new();
        public int CustomerCount { get; init; }
        public int CustomersWithSettings { get; init; }
        public int BeamMaterialCount { get; init; }
    }

    /// <summary>
    /// 2026年11月24日 ワークベンチ（ホーム）の集計。工事一覧の状態列と同じ判定（GetFloorChecksAsync）を使う。読み取りのみ。
    /// </summary>
    public partial class JwProjectMainService
    {
        public async Task<HomeSummary> GetHomeSummaryAsync()
        {
            var projects = await context.JwProjectMainDatas.AsNoTracking()
                .Select(m => new
                {
                    m.Id, m.ProjectName, m.FloorQuantity, m.ProjectStatus, m.CreationTime,
                    Customer = m.JwCustomerData != null ? m.JwCustomerData.CompanyName : "",
                })
                .ToListAsync();
            var floors = await context.JwProjectSubDatas.AsNoTracking()
                .Select(s => new { s.Id, s.JwProjectMainDataId, s.FloorName, s.CreationTime, s.ExportCount })
                .ToListAsync();
            var checks = await GetFloorChecksAsync();
            int customers = await context.JwCustomerDatas.AsNoTracking().CountAsync();
            int withSettings = await context.JwCustDesignConstDatas.AsNoTracking()
                .Where(c => c.JwCustomerDataId != null).Select(c => c.JwCustomerDataId).Distinct().CountAsync();
            int beamMaterials = await context.JwMaterialDatas.AsNoTracking().CountAsync(m => m.MaterialType == MaterialType.梁);

            return new HomeSummary
            {
                Projects = projects.Select(p => new HomeProject(p.Id, p.ProjectName ?? "", p.Customer ?? "", p.FloorQuantity, p.ProjectStatus, p.CreationTime)).ToList(),
                Floors = floors.Select(f => new HomeFloor(f.Id, f.JwProjectMainDataId, f.FloorName ?? "", f.CreationTime, f.ExportCount)).ToList(),
                Checks = checks,
                CustomerCount = customers,
                CustomersWithSettings = withSettings,
                BeamMaterialCount = beamMaterials,
            };
        }
    }
}
