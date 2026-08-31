using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwData.Migrations
{
    public partial class projectsubchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LianjieCompentType",
                table: "JwProjectSubDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "MinimalStrategy",
                table: "JwProjectSubDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LianjieCompentType",
                table: "JwProjectSubDatas");

            migrationBuilder.DropColumn(
                name: "MinimalStrategy",
                table: "JwProjectSubDatas");
        }
    }
}
