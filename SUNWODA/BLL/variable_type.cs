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
    /// BLL for variabletype
    /// </summary>
    public partial class variable_type
    {
        private readonly DAL.variable_type _dal = new DAL.variable_type();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public variable_type()
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

        public bool Add(Model.variable_type model)
        {
            bool success = _dal.Add(model);
            if (success)
            {
                _memoryCache.Remove("variabletype_AllItems");
            }
            return success;
        }

        public bool Update(Model.variable_type model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"variabletypeModel-{model.id}");
                _memoryCache.Remove("variabletype_AllItems");
            }
            return success;
        }

        public bool Delete(int id)
        {
            bool success = _dal.Delete(id);
            if (success)
            {
                _memoryCache.Remove($"variabletypeModel-{id}");
                _memoryCache.Remove("variabletype_AllItems");
            }
            return success;
        }

        /// <summary>
        ///
        /// </summary>
        public bool DeleteList(string idlist)
        {
            bool success = _dal.DeleteList(idlist);
            if (success)
            {
                _memoryCache.Remove("variabletype_AllItems");
            }
            return success;
        }

        public Model.variable_type? GetModel(int id) => _dal.GetModel(id);

        /// <summary>
        ///
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        ///
        /// </summary>
        public List<Model.variable_type> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.variable_type> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.variable_type>();
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

        public Task<int> GetMaxIdAsync() => Task.Run(() => GetMaxId());

        public Task<bool> ExistsAsync(int id) => Task.Run(() => Exists(id));

        public Task<bool> AddAsync(Model.variable_type model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.variable_type model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(int id) => Task.Run(() => Delete(id));

        public Task<Model.variable_type?> GetModelAsync(int id) => Task.Run(() => GetModel(id));

        /// <summary>
        ///
        /// </summary>
        public async Task<Model.variable_type?> GetModelByCacheAsync(int id)
        {
            string cacheKey = $"variabletypeModel-{id}";
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

        /// <summary>
        ///
        /// </summary>
        public async Task<List<Model.variable_type>> GetAllListAsync()
        {
            string cacheKey = "variabletype_AllItems";
            return await _memoryCache.GetOrCreateAsync(
                    cacheKey,
                    entry =>
                    {
                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                            _cacheDurationMinutes
                        );
                        return Task.Run(() => GetModelList(string.Empty));
                    }
                ) ?? new List<Model.variable_type>();
        }

        #endregion
    }
}
