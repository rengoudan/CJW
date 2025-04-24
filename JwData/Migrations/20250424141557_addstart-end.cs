using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwData.Migrations
{
    public partial class addstartend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EndCenter",
                table: "JwBeamDatas",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StartCenter",
                table: "JwBeamDatas",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndCenter",
                table: "JwBeamDatas");

            migrationBuilder.DropColumn(
                name: "StartCenter",
                table: "JwBeamDatas");
        }
    }
}
