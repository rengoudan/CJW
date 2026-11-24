using Microsoft.EntityFrameworkCore.Design;

namespace JwData.Migrations.SqlServer
{
    /// <summary>
    /// 2026年11月24日 dotnet ef 用（設計時のみ。接続はしない）。
    /// 実体を変更したら SQLite 用と SQL Server 用の両方にマイグレーションを追加する：
    ///   dotnet ef migrations add 名前 --project JwData.Migrations.SqlServer --startup-project JwData.Migrations.SqlServer
    /// </summary>
    public class DesignTimeFactory : IDesignTimeDbContextFactory<JwDataContext>
    {
        public JwDataContext CreateDbContext(string[] args) =>
            new JwDataContext(DbContextFactoryBuilder.BuildOptions(DbProviders.SqlServer,
                "Server=(localdb)\\MSSQLLocalDB;Database=JwDataDesign;Trusted_Connection=True"));
    }
}
