using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SUNWODA_SEVB.Data.DAL;
using SUNWODA_SEVB.Data.Model;

namespace SUNWODA_SEVB.Data.BLL
{
    /// <summary>
    /// BLL for plc_brand
    /// </summary>
    public partial class plc_brand
    {
        private readonly DAL.plc_brand _dal = new DAL.plc_brand();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public plc_brand()
        {
            var cacheOptions = new MemoryCacheOptions();
            _memoryCache = new MemoryCache(cacheOptions);

            // 从 app.config 读取缓存持续时间，默认回退时间为 5 分钟
            if (
                int.TryParse(
                    ConfigurationManager.AppSettings["ModelCacheMinutes"],
                    out int cacheDurationMinutes
                )
            )
            {
                _cacheDurationMinutes = cacheDurationMinutes;
            }
            else
            {
                _cacheDurationMinutes = 5;
            }
        }

        #region BasicMethod

        public bool Exists(string brand) => _dal.Exists(brand);

        public bool Add(Model.plc_brand model)
        {
            bool success = _dal.Add(model);
            if (success)
            {
                _memoryCache.Remove($"plc_brandModel-{model.brand}");
            }
            return success;
        }

        /// <summary>
        ///
        /// </summary>
        public bool Update(Model.plc_brand model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"plc_brandModel-{model.brand}");
            }
            return success;
        }

        public bool Delete(string brand)
        {
            bool success = _dal.Delete(brand);
            if (success)
            {
                _memoryCache.Remove($"plc_brandModel-{brand}");
            }
            return success;
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string brandlist) => _dal.DeleteList(brandlist);

        public Model.plc_brand? GetModel(string brand) => _dal.GetModel(brand);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.plc_brand? GetModelByCache(string brand)
        {
            return GetModelByCacheAsync(brand).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.plc_brand> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.plc_brand> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.plc_brand>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var model = _dal.DataRowToModel(row);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        public DataSet GetAllList() => GetList(string.Empty);

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere) => _dal.GetRecordCount(strWhere);

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return _dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        #endregion

        #region ExtensionMethod

        public Task<bool> ExistsAsync(string brand) => Task.Run(() => Exists(brand));

        public Task<bool> AddAsync(Model.plc_brand model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.plc_brand model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(string brand) => Task.Run(() => Delete(brand));

        /// <summary>
        ///
        /// </summary>
        public Task<Model.plc_brand?> GetModelByCacheAsync(string brand)
        {
            string cacheKey = $"plc_brandModel-{brand}";

            return _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );

                    return Task.FromResult(_dal.GetModel(brand));
                }
            );
        }

        public Task<List<Model.plc_brand>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
