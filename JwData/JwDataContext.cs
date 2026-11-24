using JwCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwData
{
    public class JwDataContext: DbContext
    {
        public string DbPath { get; set; }

        public DbSet<JwAddedHoleMarkData> JwAddedHoleMarkDatas { get; set; }

        public DbSet<JwCutting> JwCuttings { get; set; }

        public DbSet<JwDownPillarData> JwDownPillarDatas { get; set; }

        public DbSet<JwLianjieData> JwLianjieDatas { get; set; }

        public DbSet<JwBeamVerticalData> JwBeamVerticalDatas { get; set; }

        public DbSet<JwMaterialTypeData> JwMaterialTypeDatas { get; set; }

        public DbSet<JwOperateLogData> JwOperateLogDatas { get; set; }

        public DbSet<JwBudgetMainData> JwBudgetMainDatas { get; set; }

        public DbSet<JwBudgetSubData> JwBudgetSubDatas { get; set; }

        public DbSet<JwMaterialData> JwMaterialDatas { get; set; }

        public DbSet<JwHoleData> JwHoleDatas { get; set; }
        public DbSet<JwLinkPartData> JwLinkPartDatas { get; set; }

        public DbSet<JwPillarData> JwPillarDatas { get; set; }

        public DbSet<JwBeamData> JwBeamDatas { get; set; }

        public DbSet<JwProjectSubData> JwProjectSubDatas { get; set; }

        public DbSet<JwCustDesignConstData> JwCustDesignConstDatas { get; set; }

        public DbSet<JwProjectMainData> JwProjectMainDatas { get; set; }

        public DbSet<JwCustomerData> JwCustomerDatas { get; set; }

        public DbSet<JwLine> JwLines { get;set; }

       

        public JwDataContext(DbContextOptions<JwDataContext> options) : base(options)
        { }

        /// <summary>
        /// 2026年11月24日 SQL Server のときだけ型を補正する。SQLite のモデルは一切変えない（SQLite 用マイグレーションに差分が出ない）。
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (!Database.IsSqlServer()) return;

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    var type = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;

                    // NTS の図形は既定だと geography（緯度経度・SRID 4326 必須）になり、図面座標は保存できない → 平面の geometry
                    if (typeof(NetTopologySuite.Geometries.Geometry).IsAssignableFrom(type))
                        property.SetColumnType("geometry");
                    // decimal は既定 decimal(18,2) で小数が切り捨てられる → 桁を確保
                    else if (type == typeof(decimal) && property.GetPrecision() == null)
                    {
                        property.SetPrecision(18);
                        property.SetScale(4);
                    }
                }
            }

            // 文字列（GUID）主キーの表：SQLite は並び順指定なしの検索を登録順（rowid 順）で返すが、
            // SQL Server は主キー（クラスター化）の GUID 順で返す → 梁・穴などの一覧の並びが変わる。
            // 隠し列 RowNo（自動採番）をクラスター化インデックスにし、主キーは非クラスター化にして登録順を保つ。
            // 外部キーのインデックスも（外部キー, RowNo）順になり、SQLite の（外部キー, rowid）と同じ並びになる。
            foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(e => e.BaseType == null).ToList())
            {
                var key = entity.FindPrimaryKey();
                if (key == null || key.Properties.Count != 1 || key.Properties[0].ClrType != typeof(string)) continue;

                var eb = modelBuilder.Entity(entity.ClrType);
                eb.Property<long>(RowNoColumn).UseIdentityColumn();
                eb.HasKey(key.Properties[0].Name).IsClustered(false);
                eb.HasIndex(RowNoColumn).IsUnique().IsClustered();
            }
        }

        /// <summary>SQL Server のみの隠し列（登録順）。</summary>
        public const string RowNoColumn = "RowNo";

        //public JwDataContext()
        //{
        //    DbPath = Environment.CurrentDirectory + @"\jwdata.db";
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlite($"Data Source={DbPath}", x => x.UseNetTopologySuite());
        //}

    }
}
