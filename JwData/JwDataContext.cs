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

        public JwDataContext()
        {
            DbPath = Environment.CurrentDirectory + @"\jwdata.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}", x => x.UseNetTopologySuite());
        }

    }
}
