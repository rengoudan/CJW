using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwData.Migrations
{
    public partial class beamaddinitialcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KPillarType",
                table: "JwProjectSubDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InitialBeamCode",
                table: "JwBeamDatas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KPillarType",
                table: "JwProjectSubDatas");

            migrationBuilder.DropColumn(
                name: "InitialBeamCode",
                table: "JwBeamDatas");
        }
    }
}
