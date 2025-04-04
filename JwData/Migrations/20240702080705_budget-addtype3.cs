using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace JwData.Migrations
{
    public partial class budgetaddtype3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Sqlite:InitSpatialMetaData", true);

            migrationBuilder.CreateTable(
                name: "JwCustomerDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Contact = table.Column<string>(type: "TEXT", nullable: false),
                    Telephone = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwCustomerDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JwLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Origin = table.Column<string>(type: "TEXT", nullable: false),
                    EndPoint = table.Column<string>(type: "TEXT", nullable: false),
                    LineLength = table.Column<decimal>(type: "TEXT", nullable: false),
                    JwBeamId = table.Column<long>(type: "INTEGER", nullable: true),
                    Location = table.Column<Point>(type: "POINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JwMaterialTypeDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialTypeName = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultDataId = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialType = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwMaterialTypeDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JwOperateLogDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    OperateRelatedId = table.Column<string>(type: "TEXT", nullable: false),
                    OperateResultId = table.Column<string>(type: "TEXT", nullable: false),
                    OperateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OperateLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    OperateType = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwOperateLogDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JwCustDesignConstDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PickPrecision = table.Column<int>(type: "INTEGER", nullable: false),
                    JwJianxi = table.Column<double>(type: "REAL", nullable: false),
                    JwScale = table.Column<double>(type: "REAL", nullable: false),
                    BeamParseColorNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    BeamSplitParseColor = table.Column<int>(type: "INTEGER", nullable: false),
                    BeamPillarParseColor = table.Column<int>(type: "INTEGER", nullable: false),
                    PillarPenStyle = table.Column<int>(type: "INTEGER", nullable: false),
                    NearSpliteMax = table.Column<double>(type: "REAL", nullable: false),
                    SplitPenStyle = table.Column<int>(type: "INTEGER", nullable: false),
                    BeamSymbolTextColorNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    JwCustomerDataId = table.Column<long>(type: "INTEGER", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwCustDesignConstDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwCustDesignConstDatas_JwCustomerDatas_JwCustomerDataId",
                        column: x => x.JwCustomerDataId,
                        principalTable: "JwCustomerDatas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JwProjectMainDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectName = table.Column<string>(type: "TEXT", nullable: false),
                    BeamsNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    Biaochi = table.Column<string>(type: "TEXT", nullable: true),
                    PillarCount = table.Column<int>(type: "INTEGER", nullable: false),
                    KPillarCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SinglePillarCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BGCount = table.Column<int>(type: "INTEGER", nullable: false),
                    FloorQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ParsedQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ExportCount = table.Column<int>(type: "INTEGER", nullable: false),
                    JwCustomerDataId = table.Column<long>(type: "INTEGER", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwProjectMainDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwProjectMainDatas_JwCustomerDatas_JwCustomerDataId",
                        column: x => x.JwCustomerDataId,
                        principalTable: "JwCustomerDatas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JwMaterialDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialName = table.Column<string>(type: "TEXT", nullable: false),
                    GeneralTitle = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialParameter = table.Column<string>(type: "TEXT", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    UnitName = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialType = table.Column<int>(type: "INTEGER", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: false),
                    JwMaterialTypeDataId = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwMaterialDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwMaterialDatas_JwMaterialTypeDatas_JwMaterialTypeDataId",
                        column: x => x.JwMaterialTypeDataId,
                        principalTable: "JwMaterialTypeDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwBudgetMainDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    JwProjectMainDataId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProjectName = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwBudgetMainDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwBudgetMainDatas_JwProjectMainDatas_JwProjectMainDataId",
                        column: x => x.JwProjectMainDataId,
                        principalTable: "JwProjectMainDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwProjectSubDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FloorName = table.Column<string>(type: "TEXT", nullable: true),
                    BeamCount = table.Column<int>(type: "INTEGER", nullable: false),
                    HorizontalBeamsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    VerticalBeamsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Biaochi = table.Column<string>(type: "TEXT", nullable: true),
                    PillarCount = table.Column<int>(type: "INTEGER", nullable: false),
                    KPillarCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SinglePillarCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BGCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultBeamXHId = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultBeamXHName = table.Column<string>(type: "TEXT", nullable: false),
                    ExportCount = table.Column<int>(type: "INTEGER", nullable: false),
                    JwProjectMainDataId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<Point>(type: "POINT", nullable: true),
                    Width = table.Column<double>(type: "REAL", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    DirectionType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwProjectSubDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwProjectSubDatas_JwProjectMainDatas_JwProjectMainDataId",
                        column: x => x.JwProjectMainDataId,
                        principalTable: "JwProjectMainDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwBudgetSubDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    BudgetItemName = table.Column<string>(type: "TEXT", nullable: false),
                    UnitName = table.Column<string>(type: "TEXT", nullable: false),
                    Number = table.Column<decimal>(type: "TEXT", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    ModelParm = table.Column<string>(type: "TEXT", nullable: false),
                    BudgetType = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialType = table.Column<int>(type: "INTEGER", nullable: false),
                    JwMaterialDataId = table.Column<string>(type: "TEXT", nullable: false),
                    JwBudgetMainDataId = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwBudgetSubDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwBudgetSubDatas_JwBudgetMainDatas_JwBudgetMainDataId",
                        column: x => x.JwBudgetMainDataId,
                        principalTable: "JwBudgetMainDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JwBudgetSubDatas_JwMaterialDatas_JwMaterialDataId",
                        column: x => x.JwMaterialDataId,
                        principalTable: "JwMaterialDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwBeamDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    BeamCode = table.Column<string>(type: "TEXT", nullable: false),
                    FloorName = table.Column<string>(type: "TEXT", nullable: false),
                    GongQu = table.Column<string>(type: "TEXT", nullable: false),
                    HasQieGe = table.Column<bool>(type: "INTEGER", nullable: false),
                    QieGeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsParentBeam = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsQiegeBeam = table.Column<bool>(type: "INTEGER", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    XXLength = table.Column<double>(type: "REAL", nullable: false),
                    HasStartSide = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasEndSide = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartTelosType = table.Column<int>(type: "INTEGER", nullable: false),
                    EndTelosType = table.Column<int>(type: "INTEGER", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "TEXT", nullable: false),
                    BeamXHId = table.Column<string>(type: "TEXT", nullable: false),
                    BeamXHName = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<Point>(type: "POINT", nullable: true),
                    Width = table.Column<double>(type: "REAL", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    DirectionType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwBeamDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwBeamDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwLinkPartDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    BujianName = table.Column<string>(type: "TEXT", nullable: false),
                    BeamId = table.Column<string>(type: "TEXT", nullable: false),
                    Directed = table.Column<int>(type: "INTEGER", nullable: false),
                    GouJianType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsLianjie = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsNoBeam = table.Column<bool>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_JwLinkPartDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwLinkPartDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwPillarDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    PillarCode = table.Column<string>(type: "TEXT", nullable: false),
                    BlocksCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseType = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstLocation = table.Column<Point>(type: "POINT", nullable: true),
                    FirstWidth = table.Column<double>(type: "REAL", nullable: true),
                    FirstHeight = table.Column<double>(type: "REAL", nullable: true),
                    CenterLocation = table.Column<Point>(type: "POINT", nullable: true),
                    CenterWidth = table.Column<double>(type: "REAL", nullable: true),
                    CenterHeight = table.Column<double>(type: "REAL", nullable: true),
                    LastLocation = table.Column<Point>(type: "POINT", nullable: true),
                    LastWidth = table.Column<double>(type: "REAL", nullable: true),
                    LastHeight = table.Column<double>(type: "REAL", nullable: true),
                    TaggTitle = table.Column<string>(type: "TEXT", nullable: false),
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
                    table.PrimaryKey("PK_JwPillarDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwPillarDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwHoleDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    HasTop = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasCenter = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasBottom = table.Column<bool>(type: "INTEGER", nullable: false),
                    KongNum = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstCreateFrom = table.Column<int>(type: "INTEGER", nullable: false),
                    ChangeFrom = table.Column<int>(type: "INTEGER", nullable: false),
                    IsStart = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEnd = table.Column<bool>(type: "INTEGER", nullable: false),
                    HoleType = table.Column<int>(type: "INTEGER", nullable: false),
                    JwBeamDataId = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<Point>(type: "POINT", nullable: true),
                    Width = table.Column<double>(type: "REAL", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    DirectionType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwHoleDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwHoleDatas_JwBeamDatas_JwBeamDataId",
                        column: x => x.JwBeamDataId,
                        principalTable: "JwBeamDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwBeamDatas_JwProjectSubDataId",
                table: "JwBeamDatas",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwBudgetMainDatas_JwProjectMainDataId",
                table: "JwBudgetMainDatas",
                column: "JwProjectMainDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwBudgetSubDatas_JwBudgetMainDataId",
                table: "JwBudgetSubDatas",
                column: "JwBudgetMainDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwBudgetSubDatas_JwMaterialDataId",
                table: "JwBudgetSubDatas",
                column: "JwMaterialDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwCustDesignConstDatas_JwCustomerDataId",
                table: "JwCustDesignConstDatas",
                column: "JwCustomerDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwHoleDatas_JwBeamDataId",
                table: "JwHoleDatas",
                column: "JwBeamDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwLinkPartDatas_JwProjectSubDataId",
                table: "JwLinkPartDatas",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwMaterialDatas_JwMaterialTypeDataId",
                table: "JwMaterialDatas",
                column: "JwMaterialTypeDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwPillarDatas_JwProjectSubDataId",
                table: "JwPillarDatas",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwProjectMainDatas_JwCustomerDataId",
                table: "JwProjectMainDatas",
                column: "JwCustomerDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwProjectSubDatas_JwProjectMainDataId",
                table: "JwProjectSubDatas",
                column: "JwProjectMainDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwBudgetSubDatas");

            migrationBuilder.DropTable(
                name: "JwCustDesignConstDatas");

            migrationBuilder.DropTable(
                name: "JwHoleDatas");

            migrationBuilder.DropTable(
                name: "JwLines");

            migrationBuilder.DropTable(
                name: "JwLinkPartDatas");

            migrationBuilder.DropTable(
                name: "JwOperateLogDatas");

            migrationBuilder.DropTable(
                name: "JwPillarDatas");

            migrationBuilder.DropTable(
                name: "JwBudgetMainDatas");

            migrationBuilder.DropTable(
                name: "JwMaterialDatas");

            migrationBuilder.DropTable(
                name: "JwBeamDatas");

            migrationBuilder.DropTable(
                name: "JwMaterialTypeDatas");

            migrationBuilder.DropTable(
                name: "JwProjectSubDatas");

            migrationBuilder.DropTable(
                name: "JwProjectMainDatas");

            migrationBuilder.DropTable(
                name: "JwCustomerDatas");
        }
    }
}
