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
    /// BLL for user_define_variable
    /// </summary>
    public partial class user_define_variable
    {
        private readonly DAL.user_define_variable _dal = new DAL.user_define_variable();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public user_define_variable()
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

        public bool ExistsName(string name) => _dal.ExistsName(name);

        public bool Add(Model.user_define_variable model) => _dal.Add(model);

        public bool Update(Model.user_define_variable model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                InvalidateCache(model.id, model.variable_name);
            }
            return success;
        }

        public bool UpdateValueById(Model.user_define_variable model)
        {
            var fullModel = GetModel(model.id);
            bool success = _dal.UpdateValueById(model);
            if (success && fullModel != null)
            {
                InvalidateCache(fullModel.id, fullModel.variable_name);
            }
            return success;
        }

        /// <summary>
        ///
        /// </summary>
        public bool UpdateValue(string variableName, string value)
        {
            bool success = _dal.UpdateValue(variableName, value);
            if (success)
            {
                _memoryCache.Remove($"user_define_variableModelByName-{variableName}");
            }
            return success;
        }

        public bool Delete(int id)
        {
            var modelToDelete = GetModel(id);
            bool success = _dal.Delete(id);
            if (success && modelToDelete != null)
            {
                InvalidateCache(modelToDelete.id, modelToDelete.variable_name);
            }
            return success;
        }

        public bool DeleteByName(string variableName)
        {
            var modelToDelete = GetModelByName(variableName);
            bool success = _dal.DeleteByName(variableName);
            if (success && modelToDelete != null)
            {
                InvalidateCache(modelToDelete.id, modelToDelete.variable_name);
            }
            return success;
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist) => _dal.DeleteList(idlist);

        public Model.user_define_variable? GetModel(int id) => _dal.GetModel(id);

        private Model.user_define_variable? GetModelByName(string variableName)
        {
            var ds = GetList($"variable_name='{variableName}'");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return _dal.DataRowToModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        public Model.user_define_variable? GetModelByCache(int id) =>
            GetModelByCacheAsync(id).GetAwaiter().GetResult();

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.user_define_variable> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.user_define_variable> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.user_define_variable>();
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

        public Task<bool> UpdateAsync(Model.user_define_variable model) =>
            Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(int id) => Task.Run(() => Delete(id));

        public Task<bool> DeleteByNameAsync(string name) => Task.Run(() => DeleteByName(name));

        /// <summary>
        ///
        /// </summary>
        public Task<Model.user_define_variable?> GetModelByCacheAsync(int id)
        {
            string cacheKey = $"user_define_variableModel-{id}";
            return _memoryCache.GetOrCreateAsync(
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
        /// <remarks>
        ///
        /// </remarks>
        public Task<Model.user_define_variable?> GetModelByNameCacheAsync(string variableName)
        {
            string cacheKey = $"user_define_variableModelByName-{variableName}";
            return _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );
                    return Task.FromResult(GetModelByName(variableName));
                }
            );
        }

        #endregion

        #region Helper Methods

        private void InvalidateCache(int id, string? variableName)
        {
            if (id > 0)
            {
                _memoryCache.Remove($"user_define_variableModel-{id}");
            }
            if (!string.IsNullOrEmpty(variableName))
            {
                _memoryCache.Remove($"user_define_variableModelByName-{variableName}");
            }
        }

        #endregion
    }
}
