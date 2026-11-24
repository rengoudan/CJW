using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace JwData.Migrations.SqlServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JwCustomerDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwCustomerDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JwLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineLength = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    JwBeamId = table.Column<long>(type: "bigint", nullable: true),
                    Location = table.Column<Point>(type: "geometry", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JwMaterialTypeDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaterialTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultDataId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialCount = table.Column<int>(type: "int", nullable: false),
                    MaterialType = table.Column<int>(type: "int", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwMaterialTypeDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "JwOperateLogDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OperateRelatedId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperateResultId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperateLevel = table.Column<int>(type: "int", nullable: false),
                    OperateType = table.Column<int>(type: "int", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwOperateLogDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "JwCustDesignConstDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickPrecision = table.Column<int>(type: "int", nullable: false),
                    JwJianxi = table.Column<double>(type: "float", nullable: false),
                    JwScale = table.Column<double>(type: "float", nullable: false),
                    BeamParseColorNumber = table.Column<int>(type: "int", nullable: false),
                    BeamSplitParseColor = table.Column<int>(type: "int", nullable: false),
                    BeamPillarParseColor = table.Column<int>(type: "int", nullable: false),
                    PillarPenStyle = table.Column<int>(type: "int", nullable: false),
                    NearSpliteMax = table.Column<double>(type: "float", nullable: false),
                    SplitPenStyle = table.Column<int>(type: "int", nullable: false),
                    BeamSymbolTextColorNumber = table.Column<int>(type: "int", nullable: false),
                    DownPillarColorNumber = table.Column<int>(type: "int", nullable: false),
                    LianjieColorNumber = table.Column<int>(type: "int", nullable: false),
                    LianjieParsingMethod = table.Column<int>(type: "int", nullable: false),
                    PillarDrawingMethod = table.Column<int>(type: "int", nullable: false),
                    SplitDrawingMethod = table.Column<int>(type: "int", nullable: false),
                    LianjieCompentType = table.Column<int>(type: "int", nullable: false),
                    JwCustomerDataId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeamsNumber = table.Column<int>(type: "int", nullable: true),
                    Biaochi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PillarCount = table.Column<int>(type: "int", nullable: false),
                    KPillarCount = table.Column<int>(type: "int", nullable: false),
                    SinglePillarCount = table.Column<int>(type: "int", nullable: false),
                    BCount = table.Column<int>(type: "int", nullable: false),
                    BGCount = table.Column<int>(type: "int", nullable: false),
                    FloorQuantity = table.Column<int>(type: "int", nullable: false),
                    ParsedQuantity = table.Column<int>(type: "int", nullable: false),
                    ExportCount = table.Column<int>(type: "int", nullable: false),
                    ProjectStatus = table.Column<int>(type: "int", nullable: false),
                    SiteAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwCustomerDataId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneralTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialParameter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumParameter = table.Column<double>(type: "float", nullable: false),
                    MaterialType = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwMaterialTypeDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwMaterialDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JwProjectMainDataId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwBudgetMainDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FloorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeamCount = table.Column<int>(type: "int", nullable: false),
                    HorizontalBeamsCount = table.Column<int>(type: "int", nullable: false),
                    VerticalBeamsCount = table.Column<int>(type: "int", nullable: false),
                    Biaochi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarkBeam = table.Column<int>(type: "int", nullable: true),
                    PillarCount = table.Column<int>(type: "int", nullable: false),
                    KPillarCount = table.Column<int>(type: "int", nullable: false),
                    SinglePillarCount = table.Column<int>(type: "int", nullable: false),
                    BCount = table.Column<int>(type: "int", nullable: false),
                    BGCount = table.Column<int>(type: "int", nullable: false),
                    DefaultBeamXHId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultBeamXHName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExportCount = table.Column<int>(type: "int", nullable: false),
                    KPillarType = table.Column<int>(type: "int", nullable: false),
                    JwProjectMainDataId = table.Column<long>(type: "bigint", nullable: false),
                    MinimalStrategy = table.Column<bool>(type: "bit", nullable: false),
                    LianjieCompentType = table.Column<int>(type: "int", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<Point>(type: "geometry", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Scale = table.Column<double>(type: "float", nullable: false),
                    DirectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwProjectSubDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BudgetItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ModelParm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetType = table.Column<int>(type: "int", nullable: false),
                    MaterialType = table.Column<int>(type: "int", nullable: false),
                    JwMaterialDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JwBudgetMainDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwBudgetSubDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
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
                name: "JwAddedHoleMarkDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LineAS = table.Column<Point>(type: "geometry", nullable: false),
                    LineAE = table.Column<Point>(type: "geometry", nullable: false),
                    LineBS = table.Column<Point>(type: "geometry", nullable: false),
                    LineBE = table.Column<Point>(type: "geometry", nullable: false),
                    HoleCenter = table.Column<Point>(type: "geometry", nullable: false),
                    HasBeam = table.Column<bool>(type: "bit", nullable: false),
                    JwBeamDataId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<Point>(type: "geometry", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Scale = table.Column<double>(type: "float", nullable: false),
                    DirectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwAddedHoleMarkDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_JwAddedHoleMarkDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwBeamDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BeamCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FloorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GongQu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasQieGe = table.Column<bool>(type: "bit", nullable: false),
                    QieGeCount = table.Column<int>(type: "int", nullable: false),
                    IsParentBeam = table.Column<bool>(type: "bit", nullable: false),
                    IsQiegeBeam = table.Column<bool>(type: "bit", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    XXLength = table.Column<double>(type: "float", nullable: false),
                    HasStartSide = table.Column<bool>(type: "bit", nullable: false),
                    HasEndSide = table.Column<bool>(type: "bit", nullable: false),
                    StartTelosType = table.Column<int>(type: "int", nullable: false),
                    EndTelosType = table.Column<int>(type: "int", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BeamXHId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeamXHName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartCenter = table.Column<double>(type: "float", nullable: false),
                    EndCenter = table.Column<double>(type: "float", nullable: false),
                    HasBFG = table.Column<bool>(type: "bit", nullable: false),
                    InitialBeamCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaiFangGTBDistance = table.Column<int>(type: "int", nullable: false),
                    BeamSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasCsv = table.Column<bool>(type: "bit", nullable: false),
                    Jiaodu = table.Column<double>(type: "float", nullable: false),
                    CenterPoint = table.Column<Point>(type: "geometry", nullable: true),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<Point>(type: "geometry", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Scale = table.Column<double>(type: "float", nullable: false),
                    DirectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwBeamDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_JwBeamDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwCuttings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstPoint = table.Column<Point>(type: "geometry", nullable: false),
                    SecondPoint = table.Column<Point>(type: "geometry", nullable: false),
                    ThirdPoint = table.Column<Point>(type: "geometry", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwCuttings", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_JwCuttings_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwDownPillarDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LineAS = table.Column<Point>(type: "geometry", nullable: false),
                    LineAE = table.Column<Point>(type: "geometry", nullable: false),
                    LineBS = table.Column<Point>(type: "geometry", nullable: false),
                    LineBE = table.Column<Point>(type: "geometry", nullable: false),
                    HasBeam = table.Column<bool>(type: "bit", nullable: false),
                    HasPillar = table.Column<bool>(type: "bit", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<Point>(type: "geometry", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Scale = table.Column<double>(type: "float", nullable: false),
                    DirectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwDownPillarDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_JwDownPillarDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwLianjieDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Start = table.Column<Point>(type: "geometry", nullable: false),
                    End = table.Column<Point>(type: "geometry", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    ProjectSubName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateFrom = table.Column<int>(type: "int", nullable: false),
                    HasStartChange = table.Column<bool>(type: "bit", nullable: false),
                    HasEndChange = table.Column<bool>(type: "bit", nullable: false),
                    StartOriginal = table.Column<Point>(type: "geometry", nullable: false),
                    EndOriginal = table.Column<Point>(type: "geometry", nullable: false),
                    VPLStartPosition = table.Column<int>(type: "int", nullable: false),
                    VPLEndPosition = table.Column<int>(type: "int", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwLianjieDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_JwLianjieDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwLinkPartDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BujianName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeamId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Directed = table.Column<int>(type: "int", nullable: false),
                    GouJianType = table.Column<int>(type: "int", nullable: false),
                    CreateFrom = table.Column<int>(type: "int", nullable: false),
                    IsLianjie = table.Column<bool>(type: "bit", nullable: false),
                    IsNoBeam = table.Column<bool>(type: "bit", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<Point>(type: "geometry", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Scale = table.Column<double>(type: "float", nullable: false),
                    DirectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwLinkPartDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PillarCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlocksCount = table.Column<int>(type: "int", nullable: false),
                    BaseType = table.Column<int>(type: "int", nullable: false),
                    FirstLocation = table.Column<Point>(type: "geometry", nullable: true),
                    FirstWidth = table.Column<double>(type: "float", nullable: true),
                    FirstHeight = table.Column<double>(type: "float", nullable: true),
                    CenterLocation = table.Column<Point>(type: "geometry", nullable: true),
                    CenterWidth = table.Column<double>(type: "float", nullable: true),
                    CenterHeight = table.Column<double>(type: "float", nullable: true),
                    LastLocation = table.Column<Point>(type: "geometry", nullable: true),
                    LastWidth = table.Column<double>(type: "float", nullable: true),
                    LastHeight = table.Column<double>(type: "float", nullable: true),
                    TaggTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwProjectSubDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<Point>(type: "geometry", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Scale = table.Column<double>(type: "float", nullable: false),
                    DirectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwPillarDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_JwPillarDatas_JwProjectSubDatas_JwProjectSubDataId",
                        column: x => x.JwProjectSubDataId,
                        principalTable: "JwProjectSubDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwBeamVerticalDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JwBeamDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Center = table.Column<double>(type: "float", nullable: false),
                    HasPre = table.Column<bool>(type: "bit", nullable: false),
                    HasLast = table.Column<bool>(type: "bit", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwBeamVerticalDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_JwBeamVerticalDatas_JwBeamDatas_JwBeamDataId",
                        column: x => x.JwBeamDataId,
                        principalTable: "JwBeamDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwHoleDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasTop = table.Column<bool>(type: "bit", nullable: false),
                    HasCenter = table.Column<bool>(type: "bit", nullable: false),
                    HasBottom = table.Column<bool>(type: "bit", nullable: false),
                    KongNum = table.Column<int>(type: "int", nullable: false),
                    FirstCreateFrom = table.Column<int>(type: "int", nullable: false),
                    ChangeFrom = table.Column<int>(type: "int", nullable: false),
                    IsStart = table.Column<bool>(type: "bit", nullable: false),
                    IsEnd = table.Column<bool>(type: "bit", nullable: false),
                    HoleType = table.Column<int>(type: "int", nullable: false),
                    JwBeamDataId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasPreLinkHole = table.Column<bool>(type: "bit", nullable: false),
                    HasBhLinkHole = table.Column<bool>(type: "bit", nullable: false),
                    IsMachining = table.Column<bool>(type: "bit", nullable: false),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<Point>(type: "geometry", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Scale = table.Column<double>(type: "float", nullable: false),
                    DirectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwHoleDatas", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_JwHoleDatas_JwBeamDatas_JwBeamDataId",
                        column: x => x.JwBeamDataId,
                        principalTable: "JwBeamDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwAddedHoleMarkDatas_JwProjectSubDataId",
                table: "JwAddedHoleMarkDatas",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwAddedHoleMarkDatas_RowNo",
                table: "JwAddedHoleMarkDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwBeamDatas_JwProjectSubDataId",
                table: "JwBeamDatas",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwBeamDatas_RowNo",
                table: "JwBeamDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwBeamVerticalDatas_JwBeamDataId",
                table: "JwBeamVerticalDatas",
                column: "JwBeamDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwBeamVerticalDatas_RowNo",
                table: "JwBeamVerticalDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwBudgetMainDatas_JwProjectMainDataId",
                table: "JwBudgetMainDatas",
                column: "JwProjectMainDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwBudgetMainDatas_RowNo",
                table: "JwBudgetMainDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwBudgetSubDatas_JwBudgetMainDataId",
                table: "JwBudgetSubDatas",
                column: "JwBudgetMainDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwBudgetSubDatas_JwMaterialDataId",
                table: "JwBudgetSubDatas",
                column: "JwMaterialDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwBudgetSubDatas_RowNo",
                table: "JwBudgetSubDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwCustDesignConstDatas_JwCustomerDataId",
                table: "JwCustDesignConstDatas",
                column: "JwCustomerDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwCuttings_JwProjectSubDataId",
                table: "JwCuttings",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwCuttings_RowNo",
                table: "JwCuttings",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwDownPillarDatas_JwProjectSubDataId",
                table: "JwDownPillarDatas",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwDownPillarDatas_RowNo",
                table: "JwDownPillarDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwHoleDatas_JwBeamDataId",
                table: "JwHoleDatas",
                column: "JwBeamDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwHoleDatas_RowNo",
                table: "JwHoleDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwLianjieDatas_JwProjectSubDataId",
                table: "JwLianjieDatas",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwLianjieDatas_RowNo",
                table: "JwLianjieDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwLinkPartDatas_JwProjectSubDataId",
                table: "JwLinkPartDatas",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwLinkPartDatas_RowNo",
                table: "JwLinkPartDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwMaterialDatas_JwMaterialTypeDataId",
                table: "JwMaterialDatas",
                column: "JwMaterialTypeDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwMaterialDatas_RowNo",
                table: "JwMaterialDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwMaterialTypeDatas_RowNo",
                table: "JwMaterialTypeDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwOperateLogDatas_RowNo",
                table: "JwOperateLogDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwPillarDatas_JwProjectSubDataId",
                table: "JwPillarDatas",
                column: "JwProjectSubDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwPillarDatas_RowNo",
                table: "JwPillarDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_JwProjectMainDatas_JwCustomerDataId",
                table: "JwProjectMainDatas",
                column: "JwCustomerDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwProjectSubDatas_JwProjectMainDataId",
                table: "JwProjectSubDatas",
                column: "JwProjectMainDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JwProjectSubDatas_RowNo",
                table: "JwProjectSubDatas",
                column: "RowNo",
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwAddedHoleMarkDatas");

            migrationBuilder.DropTable(
                name: "JwBeamVerticalDatas");

            migrationBuilder.DropTable(
                name: "JwBudgetSubDatas");

            migrationBuilder.DropTable(
                name: "JwCustDesignConstDatas");

            migrationBuilder.DropTable(
                name: "JwCuttings");

            migrationBuilder.DropTable(
                name: "JwDownPillarDatas");

            migrationBuilder.DropTable(
                name: "JwHoleDatas");

            migrationBuilder.DropTable(
                name: "JwLianjieDatas");

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
