﻿// <auto-generated />
using System;
using JwData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace JwData.Migrations
{
    [DbContext(typeof(JwDataContext))]
    [Migration("20241025071849_add-craetefrom")]
    partial class addcraetefrom
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.30");

            modelBuilder.Entity("JwCore.JwBeamData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BeamCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BeamXHId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BeamXHName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("DirectionType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EndTelosType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FloorName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GongQu")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasEndSide")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasQieGe")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasStartSide")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Height")
                        .HasColumnType("REAL");

                    b.Property<bool>("IsParentBeam")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsQiegeBeam")
                        .HasColumnType("INTEGER");

                    b.Property<string>("JwProjectSubDataId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Length")
                        .HasColumnType("REAL");

                    b.Property<Point>("Location")
                        .HasColumnType("POINT");

                    b.Property<int>("QieGeCount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Scale")
                        .HasColumnType("REAL");

                    b.Property<int>("StartTelosType")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Width")
                        .HasColumnType("REAL");

                    b.Property<double>("XXLength")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("JwProjectSubDataId");

                    b.ToTable("JwBeamDatas");
                });

            modelBuilder.Entity("JwCore.JwBudgetMainData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("JwProjectMainDataId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("JwProjectMainDataId");

                    b.ToTable("JwBudgetMainDatas");
                });

            modelBuilder.Entity("JwCore.JwBudgetSubData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<string>("BudgetItemName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("BudgetType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("JwBudgetMainDataId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("JwMaterialDataId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaterialType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModelParm")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Number")
                        .HasColumnType("TEXT");

                    b.Property<string>("UnitName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("JwBudgetMainDataId");

                    b.HasIndex("JwMaterialDataId");

                    b.ToTable("JwBudgetSubDatas");
                });

            modelBuilder.Entity("JwCore.JwCustDesignConstData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeamParseColorNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeamPillarParseColor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeamSplitParseColor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeamSymbolTextColorNumber")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<long?>("JwCustomerDataId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("JwJianxi")
                        .HasColumnType("REAL");

                    b.Property<double>("JwScale")
                        .HasColumnType("REAL");

                    b.Property<double>("NearSpliteMax")
                        .HasColumnType("REAL");

                    b.Property<int>("PickPrecision")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PillarPenStyle")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SplitPenStyle")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("JwCustomerDataId");

                    b.ToTable("JwCustDesignConstDatas");
                });

            modelBuilder.Entity("JwCore.JwCustomerData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("JwCustomerDatas");
                });

            modelBuilder.Entity("JwCore.JwHoleData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("ChangeFrom")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("DirectionType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FirstCreateFrom")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasBottom")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasCenter")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasTop")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Height")
                        .HasColumnType("REAL");

                    b.Property<int>("HoleType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnd")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsStart")
                        .HasColumnType("INTEGER");

                    b.Property<string>("JwBeamDataId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("KongNum")
                        .HasColumnType("INTEGER");

                    b.Property<Point>("Location")
                        .HasColumnType("POINT");

                    b.Property<double>("Scale")
                        .HasColumnType("REAL");

                    b.Property<double>("Width")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("JwBeamDataId");

                    b.ToTable("JwHoleDatas");
                });

            modelBuilder.Entity("JwCore.JwLinkPartData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BeamId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BujianName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CreateFrom")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Directed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DirectionType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GouJianType")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Height")
                        .HasColumnType("REAL");

                    b.Property<bool>("IsLianjie")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsNoBeam")
                        .HasColumnType("INTEGER");

                    b.Property<string>("JwProjectSubDataId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Point>("Location")
                        .HasColumnType("POINT");

                    b.Property<double>("Scale")
                        .HasColumnType("REAL");

                    b.Property<double>("Width")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("JwProjectSubDataId");

                    b.ToTable("JwLinkPartDatas");
                });

            modelBuilder.Entity("JwCore.JwMaterialData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("GeneralTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("JwMaterialTypeDataId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MaterialParameter")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaterialType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UnitName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("JwMaterialTypeDataId");

                    b.ToTable("JwMaterialDatas");
                });

            modelBuilder.Entity("JwCore.JwMaterialTypeData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("DefaultDataId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaterialCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaterialType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaterialTypeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("JwMaterialTypeDatas");
                });

            modelBuilder.Entity("JwCore.JwOperateLogData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("OperateLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OperateRelatedId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OperateResultId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OperateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("OperateType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("JwOperateLogDatas");
                });

            modelBuilder.Entity("JwCore.JwPillarData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("BaseType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BlocksCount")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("CenterHeight")
                        .HasColumnType("REAL");

                    b.Property<Point>("CenterLocation")
                        .HasColumnType("POINT");

                    b.Property<double?>("CenterWidth")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("DirectionType")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("FirstHeight")
                        .HasColumnType("REAL");

                    b.Property<Point>("FirstLocation")
                        .HasColumnType("POINT");

                    b.Property<double?>("FirstWidth")
                        .HasColumnType("REAL");

                    b.Property<double>("Height")
                        .HasColumnType("REAL");

                    b.Property<string>("JwProjectSubDataId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("LastHeight")
                        .HasColumnType("REAL");

                    b.Property<Point>("LastLocation")
                        .HasColumnType("POINT");

                    b.Property<double?>("LastWidth")
                        .HasColumnType("REAL");

                    b.Property<Point>("Location")
                        .HasColumnType("POINT");

                    b.Property<string>("PillarCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Scale")
                        .HasColumnType("REAL");

                    b.Property<string>("TaggTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Width")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("JwProjectSubDataId");

                    b.ToTable("JwPillarDatas");
                });

            modelBuilder.Entity("JwCore.JwProjectMainData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BGCount")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BeamsNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Biaochi")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("ExportCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FloorQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("JwCustomerDataId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KPillarCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParsedQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PillarCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ProjectStatus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SinglePillarCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("JwCustomerDataId");

                    b.ToTable("JwProjectMainDatas");
                });

            modelBuilder.Entity("JwCore.JwProjectSubData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("BCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BGCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeamCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Biaochi")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("DefaultBeamXHId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DefaultBeamXHName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DirectionType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExportCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FloorName")
                        .HasColumnType("TEXT");

                    b.Property<double>("Height")
                        .HasColumnType("REAL");

                    b.Property<int>("HorizontalBeamsCount")
                        .HasColumnType("INTEGER");

                    b.Property<long>("JwProjectMainDataId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KPillarCount")
                        .HasColumnType("INTEGER");

                    b.Property<Point>("Location")
                        .HasColumnType("POINT");

                    b.Property<int?>("MarkBeam")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PillarCount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Scale")
                        .HasColumnType("REAL");

                    b.Property<int>("SinglePillarCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VerticalBeamsCount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Width")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("JwProjectMainDataId");

                    b.ToTable("JwProjectSubDatas");
                });

            modelBuilder.Entity("JwData.JwLine", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EndPoint")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("JwBeamId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("LineLength")
                        .HasColumnType("TEXT");

                    b.Property<Point>("Location")
                        .IsRequired()
                        .HasColumnType("POINT");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("JwLines");
                });

            modelBuilder.Entity("JwCore.JwBeamData", b =>
                {
                    b.HasOne("JwCore.JwProjectSubData", "JwProjectSubData")
                        .WithMany("JwBeamDatas")
                        .HasForeignKey("JwProjectSubDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JwProjectSubData");
                });

            modelBuilder.Entity("JwCore.JwBudgetMainData", b =>
                {
                    b.HasOne("JwCore.JwProjectMainData", "JwProjectMainData")
                        .WithMany()
                        .HasForeignKey("JwProjectMainDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JwProjectMainData");
                });

            modelBuilder.Entity("JwCore.JwBudgetSubData", b =>
                {
                    b.HasOne("JwCore.JwBudgetMainData", "JwBudgetMainData")
                        .WithMany("JwBudgetSubDatas")
                        .HasForeignKey("JwBudgetMainDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JwCore.JwMaterialData", "JwMaterialData")
                        .WithMany()
                        .HasForeignKey("JwMaterialDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JwBudgetMainData");

                    b.Navigation("JwMaterialData");
                });

            modelBuilder.Entity("JwCore.JwCustDesignConstData", b =>
                {
                    b.HasOne("JwCore.JwCustomerData", "JwCustomerData")
                        .WithMany()
                        .HasForeignKey("JwCustomerDataId");

                    b.Navigation("JwCustomerData");
                });

            modelBuilder.Entity("JwCore.JwHoleData", b =>
                {
                    b.HasOne("JwCore.JwBeamData", "JwBeamData")
                        .WithMany("JwHoles")
                        .HasForeignKey("JwBeamDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JwBeamData");
                });

            modelBuilder.Entity("JwCore.JwLinkPartData", b =>
                {
                    b.HasOne("JwCore.JwProjectSubData", "JwProjectSubData")
                        .WithMany("JwLinkPartDatas")
                        .HasForeignKey("JwProjectSubDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JwProjectSubData");
                });

            modelBuilder.Entity("JwCore.JwMaterialData", b =>
                {
                    b.HasOne("JwCore.JwMaterialTypeData", "JwMaterialTypeData")
                        .WithMany("JwMaterialDatas")
                        .HasForeignKey("JwMaterialTypeDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JwMaterialTypeData");
                });

            modelBuilder.Entity("JwCore.JwPillarData", b =>
                {
                    b.HasOne("JwCore.JwProjectSubData", "JwProjectSubData")
                        .WithMany("JwPillarDatas")
                        .HasForeignKey("JwProjectSubDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JwProjectSubData");
                });

            modelBuilder.Entity("JwCore.JwProjectMainData", b =>
                {
                    b.HasOne("JwCore.JwCustomerData", "JwCustomerData")
                        .WithMany("JwProjectMainDatas")
                        .HasForeignKey("JwCustomerDataId");

                    b.Navigation("JwCustomerData");
                });

            modelBuilder.Entity("JwCore.JwProjectSubData", b =>
                {
                    b.HasOne("JwCore.JwProjectMainData", "JwProjectMainData")
                        .WithMany("JwProjectSubDatas")
                        .HasForeignKey("JwProjectMainDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JwProjectMainData");
                });

            modelBuilder.Entity("JwCore.JwBeamData", b =>
                {
                    b.Navigation("JwHoles");
                });

            modelBuilder.Entity("JwCore.JwBudgetMainData", b =>
                {
                    b.Navigation("JwBudgetSubDatas");
                });

            modelBuilder.Entity("JwCore.JwCustomerData", b =>
                {
                    b.Navigation("JwProjectMainDatas");
                });

            modelBuilder.Entity("JwCore.JwMaterialTypeData", b =>
                {
                    b.Navigation("JwMaterialDatas");
                });

            modelBuilder.Entity("JwCore.JwProjectMainData", b =>
                {
                    b.Navigation("JwProjectSubDatas");
                });

            modelBuilder.Entity("JwCore.JwProjectSubData", b =>
                {
                    b.Navigation("JwBeamDatas");

                    b.Navigation("JwLinkPartDatas");

                    b.Navigation("JwPillarDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
