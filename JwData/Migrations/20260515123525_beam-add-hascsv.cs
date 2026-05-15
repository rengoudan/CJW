using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwData.Migrations
{
    public partial class beamaddhascsv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasCsv",
                table: "JwBeamDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCsv",
                table: "JwBeamDatas");
        }
    }
}
