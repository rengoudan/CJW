using System.Text;
using JwCore;
using JwServices;
using RGBControls.Forms;
using RGBJWMain.Forms;
using Sunny.UI;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 図面の取り込みの流れ（工事一覧の「アップロード」と作業台の両方から使う）。
    /// 1 ファイルの流れは従来どおり「解析設定 → 解析（FileParseForm）→ 保存」。追加した点：
    ///   ・顧客の解析設定が保存済みなら、まず簡易確認（階名・材料）を出す。「このまま解析」なら解析設定画面を見せずに確定
    ///     （解析設定画面の既存の「保存」処理がそのまま設定を反映する）。「設定を変更…」で従来の画面。
    ///   ・複数ファイルを順に取り込める。残りの階数を超える場合は始める前に知らせる。最後に結果をまとめて表示。
    /// </summary>
    internal static class FloorImportFlow
    {
        public enum Result { Saved, Cancelled, Failed }

        /// <summary>選んだファイルを順に取り込む。保存した件数を返す。</summary>
        public static async Task<int> ImportFilesAsync(IWin32Window owner, JwProjectMainData main, IReadOnlyList<string> paths)
        {
            if (paths.Count == 0) return 0;
            var service = ServiceFactory.GetInstance().CreateJwProjectMainService();
            var (parsed, planned) = await service.GetFloorCapacityAsync(main.Id);
            int remaining = Math.Max(0, planned - parsed);
            if (paths.Count > remaining)
            {
                UIMessageBox.ShowWarning(remaining == 0
                    ? JwProjectMainService.ImportBlockedReason(parsed, planned) ?? "これ以上取り込めません。"
                    : $"この工事は残り {remaining} 階分しか取り込めませんが、{paths.Count} ファイルが選択されています。\n取り込むファイルを {remaining} 個以内にしてください。");
                return 0;
            }

            var results = new List<(string File, Result Result)>();
            for (int i = 0; i < paths.Count; i++)
            {
                string label = paths.Count > 1 ? $"（{i + 1}/{paths.Count}）" : "";
                var r = await ImportOneAsync(owner, main, paths[i], label);
                results.Add((Path.GetFileName(paths[i]), r));
                int left = paths.Count - i - 1;
                if (r != Result.Saved && left > 0 &&
                    MessageBox.Show(owner, $"残り {left} ファイルの取り込みを続けますか？", "図面の取り込み",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    break;
            }

            if (paths.Count > 1)
            {
                var sb = new StringBuilder();
                foreach (var (file, r) in results)
                    sb.AppendLine((r == Result.Saved ? "○ 保存　" : r == Result.Cancelled ? "－ 中止　" : "✗ 失敗　") + file);
                int notStarted = paths.Count - results.Count;
                if (notStarted > 0) sb.AppendLine($"（未処理 {notStarted} ファイル）");
                MessageBox.Show(owner, sb.ToString(), "取り込みの結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return results.Count(x => x.Result == Result.Saved);
        }

        /// <summary>1 ファイルを取り込む（簡易確認 or 解析設定 → 解析 → 保存）。</summary>
        public static async Task<Result> ImportOneAsync(IWin32Window owner, JwProjectMainData main, string path, string progressLabel = "")
        {
            try
            {
                if (!string.Equals(Path.GetExtension(path), ".jww", StringComparison.OrdinalIgnoreCase))
                {
                    UIMessageBox.ShowError(string.Equals(Path.GetExtension(path), ".jws", StringComparison.OrdinalIgnoreCase)
                        ? "JWSを処理できません" : "jww ファイルを選んでください。");
                    return Result.Failed;
                }
                var model = new JwProjectPathModel { Path = path, MainData = main };

                // 顧客の解析設定が保存済みなら簡易確認
                bool showFull = true;
                string? floorName = null, materialId = null;
                var quick = await QuickConfirmAsync(owner, main, path, progressLabel);
                if (quick.Result == DialogResult.Cancel) return Result.Cancelled;
                if (quick.Result == DialogResult.OK) { showFull = false; floorName = quick.FloorName; materialId = quick.MaterialId; }
                else if (quick.Result == DialogResult.Retry) { floorName = quick.FloorName; materialId = quick.MaterialId; }

                using (var setting = new CustomerDesignSetting(model)
                {
                    AutoConfirm = !showFull,
                    FloorNameOverride = floorName,
                    MaterialIdOverride = materialId,
                })
                {
                    if (setting.ShowDialog(owner) != DialogResult.OK) return Result.Cancelled;
                }

                using var parseForm = new FileParseForm(model);
                if (!string.IsNullOrEmpty(progressLabel)) parseForm.Text += progressLabel;
                return parseForm.ShowDialog(owner) == DialogResult.OK ? Result.Saved : Result.Cancelled;
            }
            catch (Exception ex)
            {
                RGBControls.Errors.ErrorReporter.Report(ex, "ファイルの処理中にエラーが発生しました。");
                return Result.Failed;
            }
        }

        private sealed record QuickResult(DialogResult Result, string? FloorName, string? MaterialId);

        /// <summary>簡易確認を出す。顧客の解析設定が無い・材料が無いときは None（従来の解析設定画面へ）。</summary>
        private static async Task<QuickResult> QuickConfirmAsync(IWin32Window owner, JwProjectMainData main, string path, string progressLabel)
        {
            if (main.JwCustomerDataId == null) return new(DialogResult.None, null, null);
            var qita = ServiceFactory.GetInstance().CreateJwqitaService();
            long cid = main.JwCustomerDataId.Value;
            var c = await qita.FindCustDesignConstData(t => t.JwCustomerDataId == cid);
            if (c == null) return new(DialogResult.None, null, null);
            var materials = (await qita.GetMaterialDataAsync()).Where(t => t.MaterialType == MaterialType.梁).ToList();
            if (materials.Count == 0) return new(DialogResult.None, null, null); // 解析設定画面が「基礎データが未登録です」を出す

            static string Lc(int n) => n < 0 ? "未設定" : $"lc{n}";
            string summary = $"梁輪郭線 {Lc(c.BeamParseColorNumber)}・柱 {Lc(c.BeamPillarParseColor)}（{c.PillarDrawingMethod}）・継手記号 {Lc(c.BeamSplitParseColor)}\n"
                           + $"下階柱記号 {Lc(c.DownPillarColorNumber)}・ブレース {Lc(c.LianjieColorNumber)}・梁符号の文字 {Lc(c.BeamSymbolTextColorNumber)}";

            var service = ServiceFactory.GetInstance().CreateJwProjectMainService();
            long mainId = main.Id;
            async Task<string?> CheckName(string name)
            {
                var existing = await service.FindSubData(t => t.FloorName == name && t.JwProjectMainDataId == mainId);
                return existing != null ? $"階「{name}」は、この工事に既に取り込まれています。\n別の階名を入力してください。" : null;
            }

            using var f = new QuickImportForm(main.ProjectName, Path.GetFileName(path), progressLabel,
                Path.GetFileNameWithoutExtension(path), materials, summary, CheckName);
            var r = f.ShowDialog(owner);
            return new(r, f.FloorName, f.MaterialId);
        }
    }
}
