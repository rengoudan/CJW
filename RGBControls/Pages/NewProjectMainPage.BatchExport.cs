using System.Text;
using JwCore;
using JwShapeCommon;
using RGBControls.Classes;
using RGBControls.Forms;
using Sunny.UI;
using Kind = RGBControls.Forms.BatchExportForm.Kind;

namespace RGBControls.Pages
{
    /// <summary>
    /// 2026年11月22日 工事の「一括出力」：選んだ種類・階を、1 回選んだフォルダへまとめて出力する。
    /// 各ファイルは単階の出力と同じ処理（SaveSubBeamsTo などに分けた原来的处理）で書くので、内容は同じ。
    /// 違いは：出力先を 1 回だけ選ぶ・フォルダを消さない（同名ファイルのみ上書き）・最後に結果をまとめて表示。
    /// </summary>
    public partial class NewProjectMainPage
    {
        private async Task BatchExportAsync()
        {
            if (_selectedMainData == null) return;
            var main = _selectedMainData;
            await JwProjectMainService.LoadSubDataAsync(main);
            if (main.JwProjectSubDatas.Count == 0)
            {
                UIMessageBox.ShowWarning("この工事には取り込み済みの階がありません。");
                return;
            }

            List<Kind> kinds;
            List<JwProjectSubData> floors;
            string root;
            using (var dlg = new BatchExportForm(main, main.JwProjectSubDatas))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                kinds = dlg.SelectedKinds;
                floors = dlg.SelectedFloors;
                root = dlg.Folder;
            }
            string projectFolder = Path.Combine(root, main.ProjectName);

            var log = new StringBuilder();
            int ok = 0, skipped = 0, failed = 0;
            var csvDone = new List<JwProjectSubData>();

            await Progress(async () =>
            {
                Directory.CreateDirectory(projectFolder);
                foreach (var sub in floors)
                {
                    await JwProjectMainService.LoadSubCollectionAsync(sub);
                    foreach (var bd in sub.JwBeamDatas)
                        await JwProjectMainService.LoadBeamCollectionAsync(bd);

                    foreach (var kind in kinds)
                    {
                        string name = BatchExportForm.Kinds.First(x => x.Kind == kind).Name;
                        try
                        {
                            string? skip = await ExportOneAsync(kind, sub, root, projectFolder, csvDone);
                            if (skip == null) { ok++; log.AppendLine($"○ {sub.FloorName}　{name}"); }
                            else { skipped++; log.AppendLine($"－ {sub.FloorName}　{name}：{skip}"); }
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            log.AppendLine($"✗ {sub.FloorName}　{name}：{ex.Message}");
                            JwLogging.AppLog.Error(ex, "一括出力に失敗 {Project}/{Floor} {Kind}", main.ProjectName, sub.FloorName, kind);
                        }
                    }
                }
            });

            foreach (var sub in csvDone) await CsvExportRecorder.RecordAsync(sub); // 状態列「CSV出力済」

            JwLogging.AppLog.Info("一括出力 {Project}: 成功 {Ok}・スキップ {Skip}・失敗 {Fail} → {Folder}", main.ProjectName, ok, skipped, failed, projectFolder);
            string summary = $"出力先：{projectFolder}\n成功 {ok} 件・スキップ {skipped} 件・失敗 {failed} 件\n\n{log}";
            var icon = failed > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information;
            if (MessageBox.Show(this, summary + "\n出力先のフォルダを開きますか？", "一括出力の結果", MessageBoxButtons.YesNo, icon) == DialogResult.Yes)
            {
                try { System.Diagnostics.Process.Start("explorer.exe", $"\"{projectFolder}\""); }
                catch (Exception ex) { JwLogging.AppLog.Warn(ex, "フォルダを開けませんでした {Folder}", projectFolder); }
            }
        }

        /// <summary>1 階・1 種類を出力する。スキップしたときはその理由、出力したら null。</summary>
        private async Task<string?> ExportOneAsync(Kind kind, JwProjectSubData sub, string root, string projectFolder, List<JwProjectSubData> csvDone)
        {
            switch (kind)
            {
                case Kind.BeamJw:
                    // SaveSubBeamsTo は「選んだフォルダ\工事名\階\」へ出力する。一括では工事名フォルダを消さない
                    await SaveSubBeamsTo(sub, root, clearProjectFolder: false);
                    return null;
                case Kind.BanzukeTop:
                    await SaveSubTopCanvasTo(sub, projectFolder);
                    return null;
                case Kind.BanzukeBottom:
                    await SaveSubBottomCanvasTo(sub, projectFolder);
                    return null;
                case Kind.Brace:
                    if (sub.JwLianjieDatas.Count == 0) return "ブレースデータがありません"; // 単階の出力と同じ判定
                    await SaveSubCanvasLinesTo(sub, projectFolder);
                    return null;
                case Kind.Csv:
                    if (sub.JwBeamDatas.Count == 0) return "梁がありません";
                    var canvas = sub.DataToCanvas();
                    if (canvas == null || canvas.Beams.Count == 0) return "梁がありません";
                    var csv = canvas.ToProcessCsv();
                    if (string.IsNullOrEmpty(csv)) return "CSV の内容がありません";
                    WriteCsvFile(Path.Combine(projectFolder, string.Format("{0}-3015-2.csv", sub.FloorName)), csv);
                    csvDone.Add(sub);
                    return null;
                default:
                    return "未対応";
            }
        }
    }
}
