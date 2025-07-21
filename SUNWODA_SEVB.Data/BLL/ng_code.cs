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
    /// BLL for ngcode
    /// </summary>
    public partial class ng_code
    {
        private readonly DAL.ng_code _dal = new DAL.ng_code();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public ng_code()
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

        public bool Exists(string code) => _dal.Exists(code);

        public bool Add(Model.ng_code model)
        {
            bool success = _dal.Add(model);
            if (success)
            {
                _memoryCache.Remove($"ngcodeModel-{model.code}");
            }
            return success;
        }

        public bool Update(Model.ng_code model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"ngcodeModel-{model.code}");
            }
            return success;
        }

        public bool Delete(string code)
        {
            bool success = _dal.Delete(code);
            if (success)
            {
                _memoryCache.Remove($"ngcodeModel-{code}");
            }
            return success;
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string codelist) => _dal.DeleteList(codelist);

        public Model.ng_code? GetModel(string code) => _dal.GetModel(code);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.ng_code? GetModelByCache(string code)
        {
            return GetModelByCacheAsync(code).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.ng_code> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.ng_code> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.ng_code>();
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

        public Task<bool> ExistsAsync(string code) => Task.Run(() => Exists(code));

        public Task<bool> AddAsync(Model.ng_code model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.ng_code model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(string code) => Task.Run(() => Delete(code));

        /// <summary>
        ///
        /// </summary>
        public Task<Model.ng_code?> GetModelByCacheAsync(string code)
        {
            string cacheKey = $"ngcodeModel-{code}";

            return _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );

                    return Task.FromResult(_dal.GetModel(code));
                }
            );
        }

        public Task<List<Model.ng_code>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
