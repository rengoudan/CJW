using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace JwData.Migrations
{
    public partial class addjwaddedholemark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JwAddedHoleMarkDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    LineAS = table.Column<Point>(type: "POINT", nullable: false),
                    LineAE = table.Column<Point>(type: "POINT", nullable: false),
                    LineBS = table.Column<Point>(type: "POINT", nullable: false),
                    LineBE = table.Column<Point>(type: "POINT", nullable: false),
                    HoleCenter = table.Column<Point>(type: "POINT", nullable: false),
                    HasBeam = table.Column<bool>(type: "INTEGER", nullable: false),
                    JwBeamDataId = table.Column<string>(type: "TEXT", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<Point>(type: "POINT", nullable: true),
                    Width = table.Column<double>(type: "REAL", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    DirectionType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwAddedHoleMarkDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwAddedHoleMarkDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwAddedHoleMarkDatas_JwProjectSubDataId",
                table: "JwAddedHoleMarkDatas",
                column: "JwProjectSubDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwAddedHoleMarkDatas");
        }
    }
}
