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
    /// BLL for device_alert_config
    /// </summary>
    public partial class device_alert_config
    {
        private readonly DAL.device_alert_config _dal = new DAL.device_alert_config();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public device_alert_config()
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

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string equipmentId, string uploadParamId) =>
            _dal.Exists(equipmentId, uploadParamId);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.device_alert_config model) => _dal.Add(model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.device_alert_config model) => _dal.Update(model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string equipmentId, string uploadParamId) =>
            _dal.Delete(equipmentId, uploadParamId);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.device_alert_config? GetModel(string equipmentId, string uploadParamId) =>
            _dal.GetModel(equipmentId, uploadParamId);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.device_alert_config? GetModelByCache(string equipmentId, string uploadParamId)
        {
            string cacheKey = $"device_alert_configModel-{equipmentId}-{uploadParamId}";

            if (!_memoryCache.TryGetValue(cacheKey, out Model.device_alert_config? model))
            {
                model = _dal.GetModel(equipmentId, uploadParamId);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(_cacheDurationMinutes)
                );

                _memoryCache.Set(cacheKey, model, cacheEntryOptions);
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.device_alert_config> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.device_alert_config> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.device_alert_config>();
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

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList() => GetList(string.Empty);

        /// <summary>
        /// 分页获取数据列表
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

        public Task<bool> ExistsAsync(string equipmentId, string uploadParamId) =>
            Task.Run(() => Exists(equipmentId, uploadParamId));

        public Task<bool> AddAsync(Model.device_alert_config model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.device_alert_config model) =>
            Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(string equipmentId, string uploadParamId) =>
            Task.Run(() => Delete(equipmentId, uploadParamId));

        public Task<Model.device_alert_config?> GetModelAsync(
            string equipmentId,
            string uploadParamId
        ) => Task.Run(() => GetModel(equipmentId, uploadParamId));

        /// <summary>
        /// 得到一个对象实体，从缓存中 (Asynchronous Version)
        /// </summary>
        public async Task<Model.device_alert_config?> GetModelByCacheAsync(
            string equipmentId,
            string uploadParamId
        )
        {
            string cacheKey = $"device_alert_configModel-{equipmentId}-{uploadParamId}";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );

                    return Task.FromResult(_dal.GetModel(equipmentId, uploadParamId));
                }
            );
        }

        public Task<List<Model.device_alert_config>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
