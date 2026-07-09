using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace JwData.Migrations
{
    public partial class beamaddjiaodus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "CenterPoint",
                table: "JwBeamDatas",
                type: "POINT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Jiaodu",
                table: "JwBeamDatas",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CenterPoint",
                table: "JwBeamDatas");

            migrationBuilder.DropColumn(
                name: "Jiaodu",
                table: "JwBeamDatas");
        }
    }
}
