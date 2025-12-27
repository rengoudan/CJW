using JwCore;
using JwShapeCommon;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JwServices
{
    public class JwProjectMainService: BaseService
    {
        public JwProjectMainService(IDbContextFactory<JwData.JwDataContext> contextFactory) : base(contextFactory)
        {
        }

        #region 查询操作
        public async Task<List<JwProjectMainData>> GetAllAsync()
        {
            return await GetAllAsync<JwProjectMainData>(
                includes: new Expression<Func<JwProjectMainData, object>>[]
                { p => p.JwCustomerData});
        }

        /// <summary>
        /// 按照条件获取subdatas
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<JwProjectSubData>> GetSubDatasAsync(Expression<Func<JwProjectSubData, bool>> predicate)
        {
            return await GetAllAsync<JwProjectSubData>(predicate);
        }

        public async Task<List<JwBeamData>> GetBeamDatasAsync(Expression<Func<JwBeamData, bool>> predicate)
        {
            return await GetAllAsync<JwBeamData>(predicate);
        }

        public async Task<List<JwPillarData>> GetPillarDatasAsync(Expression<Func<JwPillarData, bool>> predicate)
        {
            return await GetAllAsync<JwPillarData>(predicate);
        }

        public async Task<List<JwLinkPartData>> GetLinkPartDatasAsync(Expression<Func<JwLinkPartData, bool>> predicate)
        {
            return await GetAllAsync<JwLinkPartData>(predicate);
        }

        public async Task<JwProjectMainData?> GetByIdAsync(long id)
        {
            return await GetByIdAsync<JwProjectMainData>(id,
                includes: new Expression<Func<JwProjectMainData, object>>[]
                { p => p.JwCustomerData});
        }

        public async Task<JwProjectSubData?> FindSubData(Expression<Func<JwProjectSubData, bool>> predicate)
        {
            return await FindAsync<JwProjectSubData>(predicate);
        }

        public async Task<JwBeamData> FindBeamData(Expression<Func<JwBeamData, bool>> predicate)
        {
            return await FindAsync<JwBeamData>(predicate);
        }
        #endregion


        #region 各类更改操作

        public async Task ChangeBeamCode(string id,string beamcode)
        {
            var bd = await GetByIdAsync<JwBeamData>(id);
            if(bd!=null)
            {
                bd.BeamCode = beamcode;
                await UpdateAsync<JwBeamData>(bd);
            }
        }

        public async Task<JwBeamData?> ChangeBeamGongqu(string id,string gongqu)
        {
             var bd= await GetByIdAsync<JwBeamData>(id);
            if (bd != null)
            {
                bd.GongQu = gongqu;
            }
            await UpdateAsync<JwBeamData>(bd);
            return bd;
        }
        #endregion

        #region 新增
        public async Task AddMainData(JwProjectMainData maindata)
        {
            await AddAsync<JwProjectMainData>(maindata);
        }

        public async Task AddNewSub(JwProjectSubData subdata)
        {
            await AddAsync<JwProjectSubData>(subdata);
        }

        public async Task AddLianjie(JwLianjieData lianjieData)
        {
            await AddAsync<JwLianjieData>(lianjieData);
        }

        #endregion

        /// <summary>
        /// 删除指定ID的形状 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shapeType"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSquare(string id,string subid, DrawShapeType shapeType)
        {
            var re = false;
            switch (shapeType)
            {
                case DrawShapeType.Beam:
                    {
                        re= await DeleteAsync<JwBeamData>(id);
                        re = true;
                        break;
                    }
                case DrawShapeType.Pillar:
                    {
                        re = await DeleteAsync<JwPillarData>(id);
                        break;
                    }
                case DrawShapeType.LinkPart:
                    {
                        re = await DeleteAsync<JwLinkPartData>(id);
                        break;
                    }
                default:
                    break;
            }
            //重新计算sub 和main的计数
            await ReCalculateSub(subid);
            return re;
        }

        public async Task DeleteSubData(string subId)
        {
            var subdata = await GetByIdAsync<JwProjectSubData>(subId, p => p.JwProjectMainData);
            if (subdata != null)
            {
                var maindata = await FindAsync<JwProjectMainData>(p => p.Id == subdata.JwProjectMainDataId);
                var beamdatas = await GetAllAsync<JwBeamData>(t => t.JwProjectSubDataId == subId);
                var beamidlst= beamdatas.Select(t => t.Id).ToList();
                await DeleteAsync<JwHoleData>(t => beamidlst.Contains(t.JwBeamDataId));
                await DeleteAsync<JwBeamData>(t=>t.JwProjectSubDataId == subId);
                await DeleteAsync<JwPillarData>(t => t.JwProjectSubDataId == subId);
                await DeleteAsync<JwLinkPartData>(t => t.JwProjectSubDataId == subId);
                await DeleteAsync<JwLianjieData>(t => t.JwProjectSubDataId == subId);
                await DeleteAsync<JwProjectSubData>(subId);
                await DeleteAsync<JwProjectSubData>(subId);
                if (maindata != null)
                {
                    //maindata.ParsedQuantity -= 1;
                    await ReCalculateMain(maindata.Id);
                }
            }
        }

        /// <summary>
        /// 重新计算sub 和 main 的各类计数
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        public async Task ReCalculateSub(string subId)
        {
            var subdata = await GetByIdAsync<JwProjectSubData>(subId,p=>p.JwProjectMainData);
            var maindata=await FindAsync<JwProjectMainData>(p=>p.Id==subdata.JwProjectMainDataId);
            await LoadSubDataAsync(maindata);
            var subidlst = maindata.JwProjectSubDatas.Select(t=>t.Id).ToList();
            
            if (subdata!=null)
            {
                var beamcount = await GetAllAsync<JwBeamData>(p => p.JwProjectSubDataId == subId);
                subdata.BeamCount = beamcount.Count;
                subdata.HorizontalBeamsCount = beamcount.Where(p => p.DirectionType == BeamDirectionType.Horizontal).Count();
                subdata.VerticalBeamsCount = beamcount.Where(p => p.DirectionType == BeamDirectionType.Vertical).Count();   
                
                var beammaincount = await GetAllAsync<JwBeamData>(p => subidlst.Contains( p.JwProjectSubDataId));
                maindata.BeamsNumber = beammaincount.Count;
                

                var pillarcount = await GetAllAsync<JwPillarData>(p => p.JwProjectSubDataId == subId);
                subdata.PillarCount = pillarcount.Count;
                subdata.KPillarCount = pillarcount.Where(p => p.BaseType==PillarBaseType.KPillar).Count();
                subdata.SinglePillarCount = pillarcount.Where(p => p.BaseType==PillarBaseType.SinglePillar).Count();

                var pillarmaincount = await GetAllAsync<JwPillarData>(p => subidlst.Contains(p.JwProjectSubDataId));
                maindata.PillarCount = pillarmaincount.Count;
                maindata.KPillarCount = pillarmaincount.Where(p => p.BaseType == PillarBaseType.KPillar).Count();
                maindata.SinglePillarCount = pillarmaincount.Where(p => p.BaseType == PillarBaseType.SinglePillar).Count();

                var bbg=await GetAllAsync<JwLinkPartData>(p => p.JwProjectSubDataId == subId);
                subdata.BCount = bbg.Where(p => p.GouJianType == GouJianType.B).Count();
                subdata.BGCount = bbg.Where(p => p.GouJianType == GouJianType.BG).Count();

                var bbgmain=await GetAllAsync<JwLinkPartData>(p => subidlst.Contains(p.JwProjectSubDataId));
                maindata.BCount = bbgmain.Where(p => p.GouJianType == GouJianType.B).Count();
                maindata.BGCount = bbgmain.Where(p => p.GouJianType == GouJianType.BG).Count();

                await UpdateAsync<JwProjectSubData>(subdata);
                await UpdateAsync<JwProjectMainData>(maindata);
            }
        }

        public async Task ReCalculateMain(long id)
        {
            var maindata=await GetByIdAsync<JwProjectMainData>(id);
            await LoadSubDataAsync(maindata);
            maindata.ParsedQuantity= maindata.JwProjectSubDatas.Count;
            var subidlst = maindata.JwProjectSubDatas.Select(t => t.Id).ToList();
            var beammaincount = await GetAllAsync<JwBeamData>(p => subidlst.Contains(p.JwProjectSubDataId));
            maindata.BeamsNumber = beammaincount.Count;
            var pillarmaincount = await GetAllAsync<JwPillarData>(p => subidlst.Contains(p.JwProjectSubDataId));
            maindata.PillarCount = pillarmaincount.Count;
            maindata.KPillarCount = pillarmaincount.Where(p => p.BaseType == PillarBaseType.KPillar).Count();
            maindata.SinglePillarCount = pillarmaincount.Where(p => p.BaseType == PillarBaseType.SinglePillar).Count();
            var bbgmain = await GetAllAsync<JwLinkPartData>(p => subidlst.Contains(p.JwProjectSubDataId));
            maindata.BCount = bbgmain.Where(p => p.GouJianType == GouJianType.B).Count();
            maindata.BGCount = bbgmain.Where(p => p.GouJianType == GouJianType.BG).Count();
            await UpdateAsync<JwProjectMainData>(maindata);
        }

        /// <summary>
        /// 保存解析结果
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <returns></returns>
        public async Task SaveNewParseResult(JwFileHandle fileHandle)
        {
            fileHandle._subData.DefaultBeamXHId = fileHandle.ProjectPathModel.MaterialData.Id;
            fileHandle._subData.DefaultBeamXHName = fileHandle.ProjectPathModel.MaterialData.GeneralTitle;
            await AddNewSub(fileHandle._subData);
            if (fileHandle._beamdatas.Count > 0)
            {
                foreach (var pl in fileHandle._beamdatas)
                {
                    pl.BeamXHId = fileHandle.ProjectPathModel.MaterialData.Id;
                    pl.BeamXHName = fileHandle.ProjectPathModel.MaterialData.GeneralTitle;
                    await AddAsync<JwBeamData>(pl);
                }
            }
            if (fileHandle._jwbvdatas.Count > 0)
            {
                foreach (var bv in fileHandle._jwbvdatas)
                {
                    await AddAsync<JwBeamVerticalData>(bv);
                }
            }
            if (fileHandle._holeDatas.Count > 0)
            {
                foreach (var hd in fileHandle._holeDatas)
                {
                    await AddAsync<JwHoleData>(hd);
                }
            }
            if (fileHandle._beampillarDatas.Count > 0)
            {
                foreach (var pd in fileHandle._beampillarDatas)
                {
                    await AddAsync<JwPillarData>(pd);
                }
            }
            if (fileHandle._linkPartDatas.Count > 0)
            {
                foreach (var lp in fileHandle._linkPartDatas)
                {
                    await AddAsync<JwLinkPartData>(lp);
                }
            }
            if (fileHandle._lianjieDatas.Count > 0)
            {
                foreach (var jlj in fileHandle._lianjieDatas)
                {
                    await AddAsync<JwLianjieData>(jlj);
                }
            }
            var md =await GetByIdAsync<JwProjectMainData>(fileHandle.ProjectPathModel.MainData.Id);
            md.BCount += fileHandle._subData.BCount;
            md.BGCount += fileHandle._subData.BGCount;
            if (md.BeamsNumber.HasValue)
            {
                md.BeamsNumber += fileHandle._subData.BeamCount;
            }
            else
            {
                md.BeamsNumber = fileHandle._subData.BeamCount;
            }

            md.PillarCount += fileHandle._subData.PillarCount;
            md.KPillarCount += fileHandle._subData.KPillarCount;
            md.SinglePillarCount += fileHandle._subData.SinglePillarCount;
            md.ParsedQuantity += 1;
            await UpdateAsync<JwProjectMainData>(md);
        }


        #region 加载导航集合等
        public async Task LoadSubDataAsync(JwProjectMainData maindata) 
        { 
            using var context = CreateContext(); 
            await LoadCollectionAsync(context, maindata, p => p.JwProjectSubDatas); 
        }

        public async Task LoadSubCollectionAsync(JwProjectSubData subdata) 
        { 
            using var context = CreateContext(); 
            //var newsubdata=await GetByIdAsync<JwProjectSubData>(subdata.Id);
            await LoadCollectionAsync(context, subdata, p => p.JwBeamDatas); 
            //await LoadCollectionAsync(context, subdata, p => p.JwBeamVerticalDatas); 
            await LoadCollectionAsync(context, subdata, p => p.JwPillarDatas); 
            await LoadCollectionAsync(context, subdata, p => p.JwLinkPartDatas); 
            await LoadCollectionAsync(context, subdata, p => p.JwLianjieDatas);
        }

        public async Task LoadBeamCollectionAsync(JwBeamData beamdata) 
        { 
            using var context = CreateContext(); 
            await LoadCollectionAsync(context, beamdata, p => p.JwBeamVerticalDatas);
            await LoadCollectionAsync(context, beamdata, p => p.JwHoles);
            //this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
            //this.dbContext.Entry(bd).Collection(e => e.JwBeamVerticalDatas).Load();
        }
        #endregion
    }
}
