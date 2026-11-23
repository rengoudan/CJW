using JwCore;
using JwServices;
using RGBControls.Classes;
using RGBControls.Forms;
using RGBJWMain.Forms;
using Sunny.UI;

namespace RGBControls.Pages
{
    /// <summary>
    /// 2026年11月24日 ワークベンチ（ホーム）から呼ぶ操作。中身は工事一覧の既存の処理（取り込み・確認・出力など）をそのまま使う。
    /// あわせて「絞り込み」：ワークベンチの数字カードから、関係する工事だけを一覧に出す（上に「解除」）。
    /// </summary>
    public partial class NewProjectMainPage
    {
        private HashSet<long>? _homeFilter;
        private Panel? _filterBanner;
        private System.Windows.Forms.Label? _filterLabel;

        /// <summary>工事一覧の絞り込み（ids が null なら解除）。</summary>
        public async Task SetHomeFilterAsync(IEnumerable<long>? ids, string? label)
        {
            _homeFilter = ids == null ? null : new HashSet<long>(ids);
            EnsureFilterBanner();
            _filterBanner!.Visible = _homeFilter != null;
            _filterLabel!.Text = $"絞り込み中：{label}（{_homeFilter?.Count ?? 0} 件）";
            await ReloadData();
        }

        private void EnsureFilterBanner()
        {
            if (_filterBanner != null) return;
            _filterLabel = new System.Windows.Forms.Label { AutoSize = true, Margin = new Padding(8, 8, 12, 0), ForeColor = Color.FromArgb(48, 48, 48) };
            var clear = new LinkLabel { Text = "解除", AutoSize = true, Margin = new Padding(0, 8, 0, 0) };
            clear.LinkClicked += async (s, e) => await SetHomeFilterAsync(null, null);
            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            flow.Controls.Add(_filterLabel);
            flow.Controls.Add(clear);
            _filterBanner = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = Color.FromArgb(235, 243, 255), Visible = false };
            _filterBanner.Controls.Add(flow);
            panel2.Controls.Add(_filterBanner);
            _filterBanner.SendToBack(); // 一番上に配置
        }

        /// <summary>ReloadData から呼ぶ：絞り込み中なら該当する工事だけにする。</summary>
        private List<JwProjectMainData> ApplyHomeFilter(List<JwProjectMainData> all) =>
            _homeFilter == null ? all : all.Where(m => _homeFilter.Contains(m.Id)).ToList();

        private async Task<JwProjectMainData?> FindMainAsync(long mainId) =>
            (await JwProjectMainService.GetAllAsync()).FirstOrDefault(m => m.Id == mainId); // 顧客も読み込まれる

        // ───────── ワークベンチからの操作 ─────────

        /// <summary>工事を追加（工事一覧の「工事を追加」と同じ）。</summary>
        public Task AddProjectFromHomeAsync() => AddProjectAsync();

        /// <summary>図面の取り込み（工事一覧の「アップロード」と同じ確認・流れ。複数ファイル可）。</summary>
        public async Task ImportFromHomeAsync(long mainId)
        {
            var main = await FindMainAsync(mainId);
            if (main == null) return;
            if (main.ProjectStatus == ProjectStatus.Budget)
            {
                UIMessageBox.ShowError($"{main.ProjectName}には予算が生成されているためアップロードできません");
                return;
            }
            var blocked = await JwProjectMainService.CheckImportAsync(main.Id);
            if (blocked != null) { UIMessageBox.ShowWarning(blocked); return; }
            using var f = new OpenFileDialog { Filter = "Jww Files|*.jww|Jws Files|*.jws|All Files|*.*", Multiselect = true };
            if (f.ShowDialog() != DialogResult.OK) return;
            int saved = await FloorImportFlow.ImportFilesAsync(this, main, f.FileNames);
            if (saved > 0) await ReloadData();
        }

        /// <summary>要確認の梁を表示（階画面を要確認だけで開く）。</summary>
        public async Task OpenNeedCheckFromHomeAsync(string subId)
        {
            var sub = await JwProjectMainService.FindSubData(s => s.Id == subId);
            if (sub != null) await OpenFloorViewAsync(sub, needCheck: true);
        }

        /// <summary>梁製造 CSV の出力（確認画面 → 保存。工事一覧の「梁製造CSV」と同じ）。</summary>
        public async Task ExportCsvFromHomeAsync(string subId)
        {
            var sub = await JwProjectMainService.FindSubData(s => s.Id == subId);
            if (sub == null) return;
            await JwProjectMainService.LoadSubCollectionAsync(sub);
            foreach (var b in sub.JwBeamDatas) await JwProjectMainService.LoadBeamCollectionAsync(b);
            if (sub.JwBeamDatas.Count == 0) { UIMessageBox.ShowWarning("この階には梁がありません。"); return; }
            bool exported = false;
            await Progress(() => { exported = ExportCsv(sub); });
            if (exported) await CsvExportRecorder.RecordAsync(sub);
        }

        /// <summary>梁統計。</summary>
        public async Task BeamStatsFromHomeAsync(long mainId)
        {
            var main = await FindMainAsync(mainId);
            if (main == null) return;
            using var f = new ShowAllBeams(main);
            f.ShowDialog();
        }

        /// <summary>一括出力。</summary>
        public async Task BatchExportFromHomeAsync(long mainId)
        {
            var main = await FindMainAsync(mainId);
            if (main == null) return;
            _selectedMainData = main;
            await BatchExportAsync();
        }

        /// <summary>階の作業台（工事をダブルクリックしたときと同じ）。</summary>
        public async Task OpenWorkbenchFromHomeAsync(long mainId)
        {
            var main = await FindMainAsync(mainId);
            if (main == null) return;
            await Progress(async () =>
            {
                await JwProjectMainService.LoadSubDataAsync(main);
                foreach (var sub in main.JwProjectSubDatas)
                {
                    await JwProjectMainService.LoadSubCollectionAsync(sub);
                    foreach (var bd in sub.JwBeamDatas)
                        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                }
            });
            using (var form = new SubsForm(main)) form.ShowDialog(this);
            await ReloadData();
        }
    }
}
