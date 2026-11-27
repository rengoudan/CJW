using JwCore;
using JwData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JwServices
{
    public class JwqitaService : BaseService
    {
        public JwqitaService(IDbContextFactory<JwDataContext> contextFactory) : base(contextFactory)
        {
        }

        public JwqitaService(JwDataContext context) : base(context)
        {
        }

        public async Task LoadSubDataAsync(JwMaterialTypeData maindata)
        {
            //using var context = CreateContext();
            await LoadCollectionAsync(context, maindata, p => p.JwMaterialDatas);
        }

        /// <summary>
        /// 获取预算
        /// </summary>
        /// <returns></returns>
        public async Task<List<JwBudgetMainData>> GetAllJwBudgetMainDataAsync(Expression<Func<JwBudgetMainData, bool>>? predicate)
        {
            return await GetAllAsync<JwBudgetMainData>(predicate);
        }

        public async Task<List<JwCustomerData>> GetAllJwCustomerDatasAsync(Expression<Func<JwCustomerData, bool>>? predicate)
        {
            return await GetAllAsync<JwCustomerData>(predicate);
        }

        public async Task<List<JwMaterialData>> GetMaterialDataAsync()
        {
            return await GetAllAsync<JwMaterialData>(includes: new Expression<Func<JwMaterialData, object>>[]
            { p => p.JwMaterialTypeData});
        }

        public List<JwMaterialData> GetMaterialData(Expression<Func<JwMaterialData, bool>>? predicate = null)
        {
            return  GetAll<JwMaterialData>(predicate, includes: new Expression<Func<JwMaterialData, object>>[]
            { p => p.JwMaterialTypeData});
        }
        

        public async Task<List<JwMaterialTypeData>> GetJwMaterialTypeDatasAsync(Expression<Func<JwMaterialTypeData, bool>>? predicate=null)
        {
            return await GetAllAsync(predicate);
        }

        public async Task<List<JwCustDesignConstData>> GetConstDatasAsync()
        {
                       return await GetAllAsync<JwCustDesignConstData>();
        }

        public async Task<JwCustDesignConstData?> FindCustDesignConstData(Expression<Func<JwCustDesignConstData, bool>> predicate)
        {
            return await FindAsync(predicate);
        }

        public async Task AddJwBudgetMainDataAsync(JwBudgetMainData data)
        {
            await AddAsync<JwBudgetMainData>(data);
        }

        public async Task AddJwBudgetSubDataAsync(JwBudgetSubData data)
        {

            await AddAsync<JwBudgetSubData>(data);
        }

        public async Task UpdateJwBudgetMainDataAsync(JwBudgetMainData data)
        {
            await UpdateAsync<JwBudgetMainData>(data);
        }

        public async Task LoadBudgetSubCollectionAsync(JwBudgetMainData subdata)
        {
            //using var context = CreateContext();
            //var newsubdata=await GetByIdAsync<JwProjectSubData>(subdata.Id);
            await LoadCollectionAsync(context, subdata, p => p.JwBudgetSubDatas);
        }

        public async Task AddJwCustDesignConstDataAsync(JwCustDesignConstData data)
        {
            await AddAsync<JwCustDesignConstData>(data);
        }   

        public async Task AddJwCustomerDataAsync(JwCustomerData data)
        {
            await AddAsync(data);
        }

        /// <summary>顧客の基本情報を保存する（顧客管理画面の編集）。</summary>
        public async Task UpdateJwCustomerDataAsync(JwCustomerData data)
        {
            await UpdateAsync(data);
        }

        /// <summary>
        /// 顧客を削除する（解析設定も一緒に削除）。
        /// 工事に使われている顧客は削除せず false を返す。
        /// </summary>
        public Task<bool> DeleteJwCustomerDataAsync(long customerId) => WriteAsync(async () =>
        {
            if (await context.JwProjectMainDatas.AnyAsync(m => m.JwCustomerDataId == customerId))
                return false;

            var settings = await context.JwCustDesignConstDatas
                .Where(c => c.JwCustomerDataId == customerId).ToListAsync();
            context.JwCustDesignConstDatas.RemoveRange(settings);

            var customer = await context.JwCustomerDatas.FindAsync(customerId);
            if (customer != null) context.JwCustomerDatas.Remove(customer);

            await context.SaveChangesAsync();
            return true;
        }, nameof(DeleteJwCustomerDataAsync));

        /// <summary>顧客管理画面用：工事と顧客の対応（工事を持つ顧客だけ）。</summary>
        public async Task<List<CustomerProjectItem>> GetCustomerProjectItemsAsync()
        {
            var rows = await context.JwProjectMainDatas.AsNoTracking()
                .Where(m => m.JwCustomerDataId != null)
                .Select(m => new { CustomerId = m.JwCustomerDataId!.Value, m.Id, m.ProjectName, m.FloorQuantity })
                .ToListAsync();
            return rows.Select(r => new CustomerProjectItem(r.CustomerId, r.Id, r.ProjectName ?? "", r.FloorQuantity)).ToList();
        }

        /// <summary>解析設定（JwCustDesignConstData）を持つ顧客の Id。</summary>
        public async Task<HashSet<long>> GetCustomerIdsWithSettingsAsync()
        {
            var ids = await context.JwCustDesignConstDatas.AsNoTracking()
                .Where(c => c.JwCustomerDataId != null)
                .Select(c => c.JwCustomerDataId!.Value)
                .ToListAsync();
            return ids.ToHashSet();
        }

        public async Task AddJwMaterialTypeDataAsync(JwMaterialTypeData data)
        {
            await AddAsync(data);
        }

        public async Task AddJwMaterialDataAsync(JwMaterialData data)
        {
            await AddAsync(data);
        }

        /// <summary>
        /// 種類（タイプ名・カテゴリー）を保存する。カテゴリーを変えたら、その種類のデータも同じカテゴリーにそろえる。
        /// </summary>
        public Task<bool> UpdateJwMaterialTypeDataAsync(string id, string name, MaterialType type) => WriteAsync(async () =>
        {
            var t = await context.JwMaterialTypeDatas.FindAsync(id);
            if (t == null) return false;

            t.MaterialTypeName = name;
            if (t.MaterialType != type)
            {
                t.MaterialType = type;
                var materials = await context.JwMaterialDatas.Where(m => m.JwMaterialTypeDataId == id).ToListAsync();
                foreach (var m in materials) m.MaterialType = type;
            }

            await context.SaveChangesAsync();
            return true;
        }, nameof(UpdateJwMaterialTypeDataAsync));

        /// <summary>データ（材料）の内容を保存する（基礎データ画面の編集）。</summary>
        public async Task UpdateJwMaterialDataAsync(JwMaterialData data)
        {
            await UpdateAsync(data);
        }

        public async Task UpdateJwCustDesignConstDataAsync(JwCustDesignConstData data)
        {
            await UpdateAsync<JwCustDesignConstData>(data);
        }

    }
}
