using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwData.Migrations
{
    public partial class addcleam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaiBeamId",
                table: "JwBeamVerticalDatas");

            migrationBuilder.DropColumn(
                name: "ParentBeamId",
                table: "JwBeamVerticalDatas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaiBeamId",
                table: "JwBeamVerticalDatas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParentBeamId",
                table: "JwBeamVerticalDatas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
