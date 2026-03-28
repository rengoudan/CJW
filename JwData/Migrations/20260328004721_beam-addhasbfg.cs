using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwData.Migrations
{
    public partial class beamaddhasbfg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaiFangGTBDistance",
                table: "JwBeamDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasBFG",
                table: "JwBeamDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaiFangGTBDistance",
                table: "JwBeamDatas");

            migrationBuilder.DropColumn(
                name: "HasBFG",
                table: "JwBeamDatas");
        }
    }
}
