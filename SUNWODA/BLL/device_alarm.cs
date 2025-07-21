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
    /// BLL for device_alarm
    /// </summary>
    public partial class device_alarm
    {
        private readonly DAL.device_alarm _dal = new DAL.device_alarm();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public device_alarm()
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
        public bool Exists(int id) => _dal.Exists(id);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.device_alarm model) => _dal.Add(model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.device_alarm model) => _dal.Update(model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id) => _dal.Delete(id);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist) => _dal.DeleteList(idlist);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.device_alarm? GetModel(int id) => _dal.GetModel(id);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.device_alarm? GetModelByCache(int id)
        {
            string cacheKey = $"device_alarmModel-{id}";

            // The '?' on Model.device_alarm? indicates that the method can return null.
            if (!_memoryCache.TryGetValue(cacheKey, out Model.device_alarm? model))
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
        public List<Model.device_alarm> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.device_alarm> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.device_alarm>();
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

        public Task<bool> ExistsAsync(int id) => Task.Run(() => Exists(id));

        public Task<bool> AddAsync(Model.device_alarm model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.device_alarm model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(int id) => Task.Run(() => Delete(id));

        public Task<Model.device_alarm?> GetModelAsync(int id) => Task.Run(() => GetModel(id));

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public async Task<Model.device_alarm?> GetModelByCacheAsync(int id)
        {
            string cacheKey = $"device_alarmModel-{id}";

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

        public Task<List<Model.device_alarm>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
