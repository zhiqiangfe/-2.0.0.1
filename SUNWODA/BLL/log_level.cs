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
    /// BLL for loglevel
    /// </summary>
    public partial class log_level
    {
        private readonly DAL.log_level _dal = new DAL.log_level();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public log_level()
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

        public bool Exists(string level) => _dal.Exists(level);

        public bool Add(Model.log_level model)
        {
            bool success = _dal.Add(model);
            if (success)
            {
                _memoryCache.Remove($"loglevelModel-{model.level}");
            }
            return success;
        }

        /// <summary>
        ///
        /// </summary>
        public bool Update(Model.log_level model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"loglevelModel-{model.level}");
            }
            return success;
        }

        public bool Delete(string level)
        {
            bool success = _dal.Delete(level);
            if (success)
            {
                _memoryCache.Remove($"loglevelModel-{level}");
            }
            return success;
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string levellist) => _dal.DeleteList(levellist);

        public Model.log_level? GetModel(string level) => _dal.GetModel(level);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.log_level? GetModelByCache(string level)
        {
            return GetModelByCacheAsync(level).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.log_level> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.log_level> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.log_level>();
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

        #region ExtensionMethod (Asynchronous - Recommended for .NET 8)

        public Task<bool> ExistsAsync(string level) => Task.Run(() => Exists(level));

        public Task<bool> AddAsync(Model.log_level model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.log_level model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(string level) => Task.Run(() => Delete(level));

        public Task<Model.log_level?> GetModelByCacheAsync(string level)
        {
            string cacheKey = $"loglevelModel-{level}";

            return _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );

                    return Task.FromResult(_dal.GetModel(level));
                }
            );
        }

        public Task<List<Model.log_level>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
