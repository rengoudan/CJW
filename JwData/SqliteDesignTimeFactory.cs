using Microsoft.EntityFrameworkCore.Design;

namespace JwData
{
    /// <summary>
    /// SQLite 用の設計時ファクトリ（dotnet ef / PMC の Update-Database 用）。
    /// 接続先は環境変数 JW_SQLITE_PATH（jwdata.db の絶対パス）で指定できる。未指定時はプログラムフォルダの jwdata.db。
    /// 実行時（アプリ）には使われない。SQL Server 用ファクトリは JwData.Migrations.SqlServer にあり、この JwData プロジェクトは参照していないので衝突しない。
    /// PMC の例：Default project = JwData、Startup project = JwData
    /// </summary>
    public class SqliteDesignTimeFactory : IDesignTimeDbContextFactory<JwDataContext>
    {
        public JwDataContext CreateDbContext(string[] args)
        {
            var path = Environment.GetEnvironmentVariable("JW_SQLITE_PATH");
            if (string.IsNullOrWhiteSpace(path))
                path = Path.Combine(AppContext.BaseDirectory, "jwdata.db");

            return new JwDataContext(DbContextFactoryBuilder.BuildOptions(DbProviders.Sqlite, $"Data Source={path}"));
        }
    }
}
