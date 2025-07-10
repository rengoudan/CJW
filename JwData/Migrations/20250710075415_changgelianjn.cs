using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace JwData.Migrations
{
    public partial class changgelianjn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JwLianjieDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Start = table.Column<Point>(type: "POINT", nullable: false),
                    End = table.Column<Point>(type: "POINT", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    ProjectSubName = table.Column<string>(type: "TEXT", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwLianjieDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwLianjieDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwLianjieDatas_JwProjectSubDataId",
                table: "JwLianjieDatas",
                column: "JwProjectSubDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwLianjieDatas");
        }
    }
}
