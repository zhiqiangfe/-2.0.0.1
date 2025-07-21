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
    /// BLL for device_config
    /// </summary>
    public partial class device_config
    {
        private readonly DAL.device_config _dal = new DAL.device_config();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public device_config()
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
        public bool Exists(string id) => _dal.Exists(id);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.device_config model) => _dal.Add(model);

        /// <summary>
        /// 更新一条数据。
        /// </summary>
        public bool Update(Model.device_config model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"device_configModel-{model.id}");
            }
            return success;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(string oldId, Model.device_config model)
        {
            bool success = _dal.Update(oldId, model);
            if (success)
            {
                _memoryCache.Remove($"device_configModel-{oldId}");
                _memoryCache.Remove($"device_configModel-{model.id}");
            }
            return success;
        }

        /// <summary>
        /// 删除一条数据。
        /// </summary>
        public bool Delete(string id)
        {
            bool success = _dal.Delete(id);
            if (success)
            {
                _memoryCache.Remove($"device_configModel-{id}");
            }
            return success;
        }

        /// <summary>
        /// 批量删除数据.
        /// </summary>
        public bool DeleteList(string idlist) => _dal.DeleteList(idlist);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.device_config? GetModel(string id) => _dal.GetModel(id);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.device_config? GetModelByCache(string id)
        {
            string cacheKey = $"device_configModel-{id}";
            if (!_memoryCache.TryGetValue(cacheKey, out Model.device_config? model))
            {
                model = _dal.GetModel(id);
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
        public List<Model.device_config> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.device_config> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.device_config>();
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
        /// 获得所有数据列表
        /// </summary>
        public DataSet GetAllList() => GetList(string.Empty);

        public int GetRecordCount(string strWhere) => _dal.GetRecordCount(strWhere);

        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return _dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        #endregion

        #region ExtensionMethod

        public Task<bool> ExistsAsync(string id) => Task.Run(() => Exists(id));

        public Task<bool> AddAsync(Model.device_config model) => Task.Run(() => Add(model));

        public async Task<bool> UpdateAsync(Model.device_config model)
        {
            bool success = await Task.Run(() => _dal.Update(model));
            if (success)
            {
                _memoryCache.Remove($"device_configModel-{model.id}");
            }
            return success;
        }

        public async Task<bool> UpdateAsync(string oldId, Model.device_config model)
        {
            bool success = await Task.Run(() => _dal.Update(oldId, model));
            if (success)
            {
                _memoryCache.Remove($"device_configModel-{oldId}");
                _memoryCache.Remove($"device_configModel-{model.id}");
            }
            return success;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            bool success = await Task.Run(() => _dal.Delete(id));
            if (success)
            {
                _memoryCache.Remove($"device_configModel-{id}");
            }
            return success;
        }

        public Task<bool> DeleteListAsync(string idlist) => Task.Run(() => DeleteList(idlist));

        public Task<Model.device_config?> GetModelAsync(string id) => Task.Run(() => GetModel(id));

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public async Task<Model.device_config?> GetModelByCacheAsync(string id)
        {
            string cacheKey = $"device_configModel-{id}";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );
                    return Task.FromResult(_dal.GetModel(id));
                }
            );
        }

        public Task<List<Model.device_config>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
