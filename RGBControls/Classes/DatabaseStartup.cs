using JwData;
using JwServices;
using JwSharedConfig.Config;
using JwSharedConfig.Models;
using RGBControls.Forms;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月24日 起動時にデータの保存先を決めて ServiceFactory を初期化する（MainForm より前に呼ぶ）。
    /// ローカル：何もしない（ServiceFactory.GetInstance() が従来どおり jwdata.db を開く）。
    /// リモート：接続確認 → テーブル作成／更新 → ServiceFactory.Initialize。失敗したら設定画面を出す。
    /// </summary>
    public static class DatabaseStartup
    {
        /// <summary>現在の接続先の表示用（例：「リモート：192.168.1.10 / JwData」）。</summary>
        public static string Description { get; private set; } = "ローカル";

        public static bool IsRemote { get; private set; }

        /// <returns>false = 利用者が中止した（プログラムを終了する）。</returns>
        public static bool Initialize()
        {
            while (true)
            {
                var s = AppConfigManager.Current.Database;
                if (!s.IsRemote)
                {
                    IsRemote = false;
                    Description = "ローカル";
                    return true;
                }

                var error = TryConnect(s);
                if (error == null)
                {
                    IsRemote = true;
                    Description = $"リモート：{s.Server} / {s.Database}";
                    return true;
                }

                // 接続できない → 設定画面（ここで「ローカル」に切り替えて起動することもできる）
                using var form = new DatabaseSettingsForm(error + "\n設定を確認して保存してください（キャンセルで終了）。");
                if (form.ShowDialog() != DialogResult.OK)
                    return false;
            }
        }

        /// <returns>エラー内容（null = 成功）。</returns>
        private static string? TryConnect(DatabaseSettings s)
        {
            var conn = SqlServerDatabase.BuildConnectionString(s.Server, s.Database, s.UserName, s.GetPassword(),
                s.TrustServerCertificate, s.ConnectTimeout);

            using var wait = new Form
            {
                Text = "データベース",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ControlBox = false,
                StartPosition = FormStartPosition.CenterScreen,
                ClientSize = new System.Drawing.Size(380, 90),
                Font = new System.Drawing.Font("Meiryo UI", 10F),
            };
            wait.Controls.Add(new Label
            {
                Text = $"データベースに接続しています…\n{s.Server} / {s.Database}",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
            });

            string? error = null;
            wait.Shown += async (_, _) =>
            {
                error = await Task.Run(() => Connect(conn));
                wait.Close();
            };
            wait.ShowDialog();
            return error;
        }

        private static string? Connect(string conn)
        {
            try
            {
                var r = SqlServerDatabase.Check(conn);
                switch (r.Status)
                {
                    case SqlServerDatabase.Status.ConnectFailed:
                        JwLogging.AppLog.Warn("Remote database connect failed: {Error}", r.Error);
                        return "データベースに接続できません：" + r.Error;
                    case SqlServerDatabase.Status.NewerThanApp:
                        return "このデータベースは新しいバージョンのプログラムで更新されています。プログラムを更新してください。";
                    case SqlServerDatabase.Status.DatabaseMissing:
                    case SqlServerDatabase.Status.NeedsUpgrade:
                        JwLogging.AppLog.Info("Remote database migrate: {Status} pending={Pending}", r.Status, r.PendingCount);
                        SqlServerDatabase.Migrate(conn);
                        break;
                }

                ServiceFactory.Initialize(DbProviders.SqlServer, conn, enableLogging: false);
                JwLogging.AppLog.Info("Remote database ready");
                return null;
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "Remote database init failed");
                return "データベースの準備に失敗しました：" + ex.Message;
            }
        }
    }
}
