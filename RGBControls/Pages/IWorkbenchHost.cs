namespace RGBControls.Pages
{
    /// <summary>ワークベンチから移動できる画面（左のメニューの順）。</summary>
    public enum WorkbenchNav { Home = 0, Projects = 1, Budget = 2, Customers = 3, BaseData = 4, Logs = 5 }

    /// <summary>
    /// 2026年11月24日 ワークベンチ（ホーム）から使う操作。実体は MainForm（工事一覧の既存の処理を呼ぶ）。
    /// </summary>
    public interface IWorkbenchHost
    {
        void ShowNav(WorkbenchNav nav);
        /// <summary>工事一覧へ移動し、ids の工事だけに絞り込む（null なら絞り込みなし）。</summary>
        Task ShowProjectsAsync(IEnumerable<long>? ids, string? label);
        Task AddProjectAsync();
        Task ImportAsync(long mainId);
        Task OpenNeedCheckAsync(string subId);
        Task ExportCsvAsync(string subId);
        Task BeamStatsAsync(long mainId);
        Task BatchExportAsync(long mainId);
        Task OpenWorkbenchAsync(long mainId);
    }
}
