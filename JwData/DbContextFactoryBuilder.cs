using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwData
{
    /// <summary>2026年11月24日 データベースの種類</summary>
    public static class DbProviders
    {
        public const string Sqlite = "sqlite";
        public const string SqlServer = "sqlserver";

        /// <summary>SQL Server 用のマイグレーションは別アセンブリ（SQLite 用は JwData 内の Migrations）。</summary>
        public const string SqlServerMigrationsAssembly = "JwData.Migrations.SqlServer";
    }

    public static class DbContextFactoryBuilder
    {
        /// <summary>従来どおり：プログラムフォルダの jwdata.db（SQLite）。</summary>
        public static IDbContextFactory<JwDataContext>? CreateFactory(bool enableLogging = false)
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "jwdata.db");
            if (!File.Exists(dbPath))
            {
                //MessageBox.Show($"数据库文件未找到：\n{dbPath}\n\n请确保数据库文件存在后再启动程序。", "数据库未找到", MessageBoxButtons.OK, MessageBoxIcon.Error); return null;
            }

            var connectionString = $"Data Source={dbPath}";
            var optionsBuilder = new DbContextOptionsBuilder<JwDataContext>().
                UseSqlite(connectionString, x => x.UseNetTopologySuite());
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            if (enableLogging)
            {
                optionsBuilder.EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }

            return new PooledDbContextFactory<JwDataContext>(optionsBuilder.Options);
        }

        /// <summary>
        /// 2026年11月24日 種類を指定して作成。provider は必須（引数なし呼び出しが上の従来版と曖昧にならないように）。
        /// </summary>
        public static IDbContextFactory<JwDataContext> CreateFactory(
        string provider,
        string? connectionString,
        bool enableLogging = false)
        {
            return new PooledDbContextFactory<JwDataContext>(BuildOptions(provider, connectionString, enableLogging));
        }

        public static DbContextOptions<JwDataContext> BuildOptions(string provider, string? connectionString, bool enableLogging = false)
        {
            var optionsBuilder = new DbContextOptionsBuilder<JwDataContext>();

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            if (provider.Equals(DbProviders.Sqlite, StringComparison.OrdinalIgnoreCase))
            {
                var dbPath = Path.Combine(AppContext.BaseDirectory, "jwdata.db");
                connectionString ??= $"Data Source={dbPath}";

                optionsBuilder.UseSqlite(connectionString, x => x.UseNetTopologySuite());
            }
            else if (provider.Equals(DbProviders.SqlServer, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new ArgumentException("SQL Server 连接字符串不能为空");

                optionsBuilder.UseSqlServer(connectionString, x => x
                    .UseNetTopologySuite()
                    .MigrationsAssembly(DbProviders.SqlServerMigrationsAssembly));
            }
            else
            {
                throw new NotSupportedException($"不支持的数据库类型: {provider}");
            }

            if (enableLogging)
            {
                optionsBuilder.EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }

            return optionsBuilder.Options;
        }
    }
}
