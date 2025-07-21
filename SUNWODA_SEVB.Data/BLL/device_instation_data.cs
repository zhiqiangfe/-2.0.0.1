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
    /// BLL for device_instation_data
    /// </summary>
    public partial class device_instation_data
    {
        private readonly DAL.device_instation_data _dal = new DAL.device_instation_data();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public device_instation_data()
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
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.device_instation_data model) => _dal.Add(model);

        /// <summary>
        /// 得到一个对象实体.
        /// </summary>
        public Model.device_instation_data? GetModel(string barcode) => _dal.GetModel(barcode);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.device_instation_data? GetModelByCache(string barcode)
        {
            string cacheKey = $"device_instation_dataModel-{barcode}";
            if (!_memoryCache.TryGetValue(cacheKey, out Model.device_instation_data? model))
            {
                model = _dal.GetModel(barcode);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(_cacheDurationMinutes)
                );

                _memoryCache.Set(cacheKey, model, cacheEntryOptions);
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表.
        /// </summary>
        public List<Model.device_instation_data> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.device_instation_data> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.device_instation_data>();
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

        #endregion

        #region ExtensionMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public Task<bool> AddAsync(Model.device_instation_data model) => Task.Run(() => Add(model));

        /// <summary>
        /// 得到一个对象实体.
        /// </summary>
        public Task<Model.device_instation_data?> GetModelAsync(string barcode) =>
            Task.Run(() => GetModel(barcode));

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public async Task<Model.device_instation_data?> GetModelByCacheAsync(string barcode)
        {
            string cacheKey = $"device_instation_dataModel-{barcode}";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );

                    return await Task.Run(() => _dal.GetModel(barcode));
                }
            );
        }

        /// <summary>
        /// 获得数据列表.
        /// </summary>
        public Task<List<Model.device_instation_data>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
