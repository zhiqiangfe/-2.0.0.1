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
    /// BLL for valuetypes
    /// </summary>
    public partial class value_types
    {
        private readonly DAL.value_types _dal = new DAL.value_types();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public value_types()
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

        public bool Exists(string value_type) => _dal.Exists(value_type);

        public bool Add(Model.value_types model)
        {
            bool success = _dal.Add(model);
            if (success)
            {
                _memoryCache.Remove("valuetypes_AllItems");
            }
            return success;
        }

        public bool Update(Model.value_types model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"valuetypesModel-{model.value_type}");
                _memoryCache.Remove("valuetypes_AllItems");
            }
            return success;
        }

        public bool Delete(string value_type)
        {
            bool success = _dal.Delete(value_type);
            if (success)
            {
                _memoryCache.Remove($"valuetypesModel-{value_type}");
                _memoryCache.Remove("valuetypes_AllItems");
            }
            return success;
        }

        /// <summary>
        ///
        /// </summary>
        public bool DeleteList(string value_typelist)
        {
            bool success = _dal.DeleteList(value_typelist);
            if (success)
            {
                _memoryCache.Remove("valuetypes_AllItems");
            }
            return success;
        }

        public Model.value_types? GetModel(string value_type) => _dal.GetModel(value_type);

        /// <summary>
        ///
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        ///
        /// </summary>
        public List<Model.value_types> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.value_types> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.value_types>();
            foreach (DataRow row in dt.Rows)
            {
                var model = _dal.DataRowToModel(row);
                if (model != null)
                {
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        ///
        /// </summary>
        public int GetRecordCount(string strWhere) => _dal.GetRecordCount(strWhere);

        /// <summary>
        ///
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return _dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        #endregion

        #region ExtensionMethod

        public Task<bool> ExistsAsync(string value_type) => Task.Run(() => Exists(value_type));

        public Task<bool> AddAsync(Model.value_types model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.value_types model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(string value_type) => Task.Run(() => Delete(value_type));

        public Task<Model.value_types?> GetModelAsync(string value_type) =>
            Task.Run(() => GetModel(value_type));

        /// <summary>
        ///
        /// </summary>
        public async Task<Model.value_types?> GetModelByCacheAsync(string value_type)
        {
            string cacheKey = $"valuetypesModel-{value_type}";
            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );
                    return Task.FromResult(_dal.GetModel(value_type));
                }
            );
        }

        /// <summary>
        ///
        /// </summary>
        public async Task<List<Model.value_types>> GetAllListAsync()
        {
            string cacheKey = "valuetypes_AllItems";
            return await _memoryCache.GetOrCreateAsync(
                    cacheKey,
                    entry =>
                    {
                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                            _cacheDurationMinutes
                        );
                        return Task.Run(() => GetModelList(string.Empty));
                    }
                ) ?? new List<Model.value_types>();
        }

        #endregion
    }
}
