using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwData.Migrations
{
    public partial class addv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JwBeamVerticalDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ParentBeamId = table.Column<string>(type: "TEXT", nullable: false),
                    JwBeamDataId = table.Column<string>(type: "TEXT", nullable: false),
                    BaiBeamId = table.Column<string>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    Center = table.Column<double>(type: "REAL", nullable: false),
                    HasPre = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasLast = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwBeamVerticalDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwBeamVerticalDatas_JwBeamDatas_JwBeamDataId",
                        column: x => x.JwBeamDataId,
                        principalTable: "JwBeamDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwBeamVerticalDatas_JwBeamDataId",
                table: "JwBeamVerticalDatas",
                column: "JwBeamDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwBeamVerticalDatas");
        }
    }
}
