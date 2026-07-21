using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace JwData.Migrations
{
    public partial class lianjiedatachange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "EndOriginal",
                table: "JwLianjieDatas",
                type: "POINT",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasEndChange",
                table: "JwLianjieDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasStartChange",
                table: "JwLianjieDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Point>(
                name: "StartOriginal",
                table: "JwLianjieDatas",
                type: "POINT",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndOriginal",
                table: "JwLianjieDatas");

            migrationBuilder.DropColumn(
                name: "HasEndChange",
                table: "JwLianjieDatas");

            migrationBuilder.DropColumn(
                name: "HasStartChange",
                table: "JwLianjieDatas");

            migrationBuilder.DropColumn(
                name: "StartOriginal",
                table: "JwLianjieDatas");
        }
    }
}
