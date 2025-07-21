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
    /// BLL for device_inputoutput_data
    /// </summary>
    public partial class device_inputoutput_data
    {
        private readonly DAL.device_inputoutput_data _dal = new DAL.device_inputoutput_data();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public device_inputoutput_data()
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

        public int GetMaxId() => _dal.GetMaxId();

        public bool Exists(int id) => _dal.Exists(id);

        public bool Add(Model.device_inputoutput_data model) => _dal.Add(model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.device_inputoutput_data model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"device_inputoutput_dataModel-{model.id}");
            }
            return success;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            bool success = _dal.Delete(id);
            if (success)
            {
                _memoryCache.Remove($"device_inputoutput_dataModel-{id}");
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
        public Model.device_inputoutput_data? GetModel(int id) => _dal.GetModel(id);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.device_inputoutput_data? GetModelByCache(int id)
        {
            string cacheKey = $"device_inputoutput_dataModel-{id}";
            if (!_memoryCache.TryGetValue(cacheKey, out Model.device_inputoutput_data? model))
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
        /// 获得数据列表.
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表.
        /// </summary>
        public List<Model.device_inputoutput_data> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.device_inputoutput_data> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.device_inputoutput_data>();
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

        /// <summary>
        /// 获取记录总数.
        /// </summary>
        public int GetRecordCount(string strWhere) => _dal.GetRecordCount(strWhere);

        /// <summary>
        /// 分页获取数据列表.
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return _dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        #endregion

        #region ExtensionMethod

        public Task<int> GetMaxIdAsync() => Task.Run(() => GetMaxId());

        public Task<bool> ExistsAsync(int id) => Task.Run(() => Exists(id));

        public Task<bool> AddAsync(Model.device_inputoutput_data model) =>
            Task.Run(() => Add(model));

        public async Task<bool> UpdateAsync(Model.device_inputoutput_data model)
        {
            bool success = await Task.Run(() => _dal.Update(model));
            if (success)
            {
                _memoryCache.Remove($"device_inputoutput_dataModel-{model.id}");
            }
            return success;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool success = await Task.Run(() => _dal.Delete(id));
            if (success)
            {
                _memoryCache.Remove($"device_inputoutput_dataModel-{id}");
            }
            return success;
        }

        public Task<bool> DeleteListAsync(string idlist) => Task.Run(() => DeleteList(idlist));

        public Task<Model.device_inputoutput_data?> GetModelAsync(int id) =>
            Task.Run(() => GetModel(id));

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public async Task<Model.device_inputoutput_data?> GetModelByCacheAsync(int id)
        {
            string cacheKey = $"device_inputoutput_dataModel-{id}";

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

        public Task<List<Model.device_inputoutput_data>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
