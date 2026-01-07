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

        public async Task LoadSubDataAsync(JwMaterialTypeData maindata)
        {
            using var context = CreateContext();
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
            using var context = CreateContext();
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

        public async Task AddJwMaterialTypeDataAsync(JwMaterialTypeData data)
        {
            await AddAsync(data);
        }

        public async Task AddJwMaterialDataAsync(JwMaterialData data)
        {
            await AddAsync(data);
        }

        public async Task UpdateJwCustDesignConstDataAsync(JwCustDesignConstData data)
        {
            await UpdateAsync<JwCustDesignConstData>(data);
        }

    }
}
