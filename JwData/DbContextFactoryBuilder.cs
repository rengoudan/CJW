using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwData
{
    public static class DbContextFactoryBuilder
    {
        public static IDbContextFactory<JwDataContext>? CreateFactory(bool enableLogging = false) 
        { 
            var dbPath = Path.Combine(AppContext.BaseDirectory, "jwdata.db"); 
            if (!File.Exists(dbPath)) 
            { 
                //MessageBox.Show($"数据库文件未找到：\n{dbPath}\n\n请确保数据库文件存在后再启动程序。", "数据库未找到", MessageBoxButtons.OK, MessageBoxIcon.Error); return null; 
            } 

            var connectionString = $"Data Source={dbPath}";
            var optionsBuilder = new DbContextOptionsBuilder<JwDataContext>().
                UseSqlite(connectionString, x => x.UseNetTopologySuite()); 
            if (enableLogging) 
            { 
                optionsBuilder.EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information); 
            } 

            return new PooledDbContextFactory<JwDataContext>(optionsBuilder.Options); }
    }
}
