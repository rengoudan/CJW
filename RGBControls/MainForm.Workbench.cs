using RGBControls.Pages;

namespace RGBControls
{
    /// <summary>
    /// 2026年11月24日 ワークベンチ（ホーム）から工事一覧の操作・画面移動を呼べるようにする。
    /// </summary>
    public partial class MainForm : IWorkbenchHost
    {
        private NewProjectMainPage? _projectsPage;

        public void ShowNav(WorkbenchNav nav) => uiNavBar1.SelectedIndex = (int)nav;

        public async Task ShowProjectsAsync(IEnumerable<long>? ids, string? label)
        {
            ShowNav(WorkbenchNav.Projects);
            if (_projectsPage != null) await _projectsPage.SetHomeFilterAsync(ids, label);
        }

        public Task AddProjectAsync() => _projectsPage?.AddProjectFromHomeAsync() ?? Task.CompletedTask;
        public Task ImportAsync(long mainId) => _projectsPage?.ImportFromHomeAsync(mainId) ?? Task.CompletedTask;
        public Task OpenNeedCheckAsync(string subId) => _projectsPage?.OpenNeedCheckFromHomeAsync(subId) ?? Task.CompletedTask;
        public Task ExportCsvAsync(string subId) => _projectsPage?.ExportCsvFromHomeAsync(subId) ?? Task.CompletedTask;
        public Task BeamStatsAsync(long mainId) => _projectsPage?.BeamStatsFromHomeAsync(mainId) ?? Task.CompletedTask;
        public Task BatchExportAsync(long mainId) => _projectsPage?.BatchExportFromHomeAsync(mainId) ?? Task.CompletedTask;
        public Task OpenWorkbenchAsync(long mainId) => _projectsPage?.OpenWorkbenchFromHomeAsync(mainId) ?? Task.CompletedTask;
    }
}
