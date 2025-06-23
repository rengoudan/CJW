using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwData.Migrations
{
    public partial class xiugaizengjiahole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBhLinkHole",
                table: "JwHoleDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPreLinkHole",
                table: "JwHoleDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBhLinkHole",
                table: "JwHoleDatas");

            migrationBuilder.DropColumn(
                name: "HasPreLinkHole",
                table: "JwHoleDatas");
        }
    }
}
