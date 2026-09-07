using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwData.Migrations
{
    public partial class lianjiedataaddvplpositionchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VPLPosition",
                table: "JwLianjieDatas",
                newName: "VPLStartPosition");

            migrationBuilder.AddColumn<int>(
                name: "VPLEndPosition",
                table: "JwLianjieDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VPLEndPosition",
                table: "JwLianjieDatas");

            migrationBuilder.RenameColumn(
                name: "VPLStartPosition",
                table: "JwLianjieDatas",
                newName: "VPLPosition");
        }
    }
}
