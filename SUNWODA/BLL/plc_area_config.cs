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
    /// BLL for plc_area_config
    /// </summary>
    public partial class plc_area_config
    {
        private readonly DAL.plc_area_config _dal = new DAL.plc_area_config();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public plc_area_config()
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

        public bool Exists(string area_name) => _dal.Exists(area_name);

        public bool Add(Model.plc_area_config model)
        {
            bool success = _dal.Add(model);
            if (success)
            {
                _memoryCache.Remove($"plc_area_configModel-{model.area_name}");
            }
            return success;
        }

        public bool Update(Model.plc_area_config model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"plc_area_configModel-{model.area_name}");
            }
            return success;
        }

        public bool Delete(string area_name)
        {
            bool success = _dal.Delete(area_name);
            if (success)
            {
                _memoryCache.Remove($"plc_area_configModel-{area_name}");
            }
            return success;
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string area_namelist) => _dal.DeleteList(area_namelist);

        public Model.plc_area_config? GetModel(string area_name) => _dal.GetModel(area_name);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.plc_area_config? GetModelByCache(string area_name)
        {
            return GetModelByCacheAsync(area_name).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.plc_area_config> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.plc_area_config> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.plc_area_config>();
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

        public Task<bool> ExistsAsync(string area_name) => Task.Run(() => Exists(area_name));

        public Task<bool> AddAsync(Model.plc_area_config model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.plc_area_config model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(string area_name) => Task.Run(() => Delete(area_name));

        /// <summary>
        ///
        /// </summary>
        public Task<Model.plc_area_config?> GetModelByCacheAsync(string area_name)
        {
            string cacheKey = $"plc_area_configModel-{area_name}";

            return _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );

                    return Task.FromResult(_dal.GetModel(area_name));
                }
            );
        }

        public Task<List<Model.plc_area_config>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
