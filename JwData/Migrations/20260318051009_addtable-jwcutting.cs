using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace JwData.Migrations
{
    public partial class addtablejwcutting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JwCuttings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FirstPoint = table.Column<Point>(type: "POINT", nullable: false),
                    SecondPoint = table.Column<Point>(type: "POINT", nullable: false),
                    ThirdPoint = table.Column<Point>(type: "POINT", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwCuttings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwCuttings_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwCuttings_JwProjectSubDataId",
                table: "JwCuttings",
                column: "JwProjectSubDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwCuttings");
        }
    }
}
