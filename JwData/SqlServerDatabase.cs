using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace JwData
{
    /// <summary>2026年11月24日 リモート（SQL Server）接続の確認・初期化。</summary>
    public static class SqlServerDatabase
    {
        public static string BuildConnectionString(string server, string database, string userName, string password,
            bool trustServerCertificate = true, int connectTimeout = 15)
        {
            // 文字列連結ではなく Builder を使う（パスワードに ; や ' が含まれても壊れない）
            var b = new SqlConnectionStringBuilder
            {
                DataSource = server.Trim(),
                InitialCatalog = database.Trim(),
                UserID = userName.Trim(),
                Password = password,
                TrustServerCertificate = trustServerCertificate,
                ConnectTimeout = connectTimeout > 0 ? connectTimeout : 15,
                MultipleActiveResultSets = true,
                ApplicationName = "RGBJW",
            };
            return b.ConnectionString;
        }

        public enum Status
        {
            /// <summary>サーバーに接続できない・ログイン失敗</summary>
            ConnectFailed,
            /// <summary>データベースがまだない（起動時に作成）</summary>
            DatabaseMissing,
            /// <summary>テーブル未作成または旧バージョン（起動時に更新）</summary>
            NeedsUpgrade,
            /// <summary>このプログラムより新しいバージョンで更新済み（プログラムの更新が必要）</summary>
            NewerThanApp,
            Ready,
        }

        public sealed record CheckResult(Status Status, int PendingCount, string? Error);

        /// <summary>接続テスト。データベースがなくてもサーバー・ログインが正しければ成功扱い（DatabaseMissing）。</summary>
        public static CheckResult Check(string connectionString)
        {
            var target = new SqlConnectionStringBuilder(connectionString);
            var dbName = target.InitialCatalog;
            try
            {
                var master = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "master" };
                using (var conn = new SqlConnection(master.ConnectionString))
                {
                    conn.Open();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT DB_ID(@n)";
                    cmd.Parameters.AddWithValue("@n", dbName);
                    if (cmd.ExecuteScalar() is null or DBNull)
                        return new CheckResult(Status.DatabaseMissing, 0, null);
                }

                using var ctx = new JwDataContext(DbContextFactoryBuilder.BuildOptions(DbProviders.SqlServer, connectionString));
                var known = ctx.Database.GetMigrations().ToHashSet();
                var applied = ctx.Database.GetAppliedMigrations().ToList();
                if (applied.Any(m => !known.Contains(m)))
                    return new CheckResult(Status.NewerThanApp, 0, null);
                var pending = known.Count - applied.Count;
                return new CheckResult(pending > 0 ? Status.NeedsUpgrade : Status.Ready, pending, null);
            }
            catch (Exception ex)
            {
                return new CheckResult(Status.ConnectFailed, 0, ex.Message);
            }
        }

        /// <summary>データベース作成・テーブル作成／更新（未適用のマイグレーションを適用）。</summary>
        public static void Migrate(string connectionString)
        {
            using var ctx = new JwDataContext(DbContextFactoryBuilder.BuildOptions(DbProviders.SqlServer, connectionString));
            ctx.Database.SetCommandTimeout(300);
            ctx.Database.Migrate();
        }
    }
}
