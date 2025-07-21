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
    /// BLL for input_log_from
    /// </summary>
    public partial class input_log_from
    {
        private readonly DAL.input_log_from _dal = new DAL.input_log_from();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public input_log_from()
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

        public bool Exists(string source) => _dal.Exists(source);

        public bool Add(Model.input_log_from model) => _dal.Add(model);

        /// <summary>
        ///
        /// </summary>
        public bool Update(Model.input_log_from model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"input_log_fromModel-{model.source}");
            }
            return success;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string source)
        {
            bool success = _dal.Delete(source);
            if (success)
            {
                _memoryCache.Remove($"input_log_fromModel-{source}");
            }
            return success;
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string sourcelist) => _dal.DeleteList(sourcelist);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.input_log_from? GetModel(string source) => _dal.GetModel(source);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.input_log_from? GetModelByCache(string source)
        {
            string cacheKey = $"input_log_fromModel-{source}";
            if (!_memoryCache.TryGetValue(cacheKey, out Model.input_log_from? model))
            {
                model = _dal.GetModel(source);
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
        public List<Model.input_log_from> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.input_log_from> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.input_log_from>();
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

        public Task<bool> ExistsAsync(string source) => Task.Run(() => Exists(source));

        public Task<bool> AddAsync(Model.input_log_from model) => Task.Run(() => Add(model));

        public async Task<bool> UpdateAsync(Model.input_log_from model)
        {
            bool success = await Task.Run(() => _dal.Update(model));
            if (success)
            {
                _memoryCache.Remove($"input_log_fromModel-{model.source}");
            }
            return success;
        }

        public async Task<bool> DeleteAsync(string source)
        {
            bool success = await Task.Run(() => _dal.Delete(source));
            if (success)
            {
                _memoryCache.Remove($"input_log_fromModel-{source}");
            }
            return success;
        }

        public Task<bool> DeleteListAsync(string sourcelist) =>
            Task.Run(() => DeleteList(sourcelist));

        public Task<Model.input_log_from?> GetModelAsync(string source) =>
            Task.Run(() => GetModel(source));

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public async Task<Model.input_log_from?> GetModelByCacheAsync(string source)
        {
            string cacheKey = $"input_log_fromModel-{source}";
            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );
                    return Task.FromResult(_dal.GetModel(source));
                }
            );
        }

        public Task<List<Model.input_log_from>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
