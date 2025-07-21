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
    /// BLL for alarm_rule
    /// </summary>
    public partial class alarm_rule
    {
        private readonly DAL.alarm_rule _dal = new DAL.alarm_rule();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public alarm_rule()
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
        public bool Exists(string equipment_id, string upload_param_id) =>
            _dal.Exists(equipment_id, upload_param_id);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.alarm_rule model) => _dal.Add(model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.alarm_rule model) => _dal.Update(model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string equipment_id, string upload_param_id) =>
            _dal.Delete(equipment_id, upload_param_id);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.alarm_rule? GetModel(string equipment_id, string upload_param_id) =>
            _dal.GetModel(equipment_id, upload_param_id);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.alarm_rule? GetModelByCache(string equipment_id, string upload_param_id)
        {
            string cacheKey = $"alarm_ruleModel-{equipment_id}:{upload_param_id}";

            if (!_memoryCache.TryGetValue(cacheKey, out Model.alarm_rule? model))
            {
                model = _dal.GetModel(equipment_id, upload_param_id);
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
        public List<Model.alarm_rule> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.alarm_rule> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.alarm_rule>();
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

        public Task<bool> ExistsAsync(string equipment_id, string upload_param_id) =>
            Task.Run(() => Exists(equipment_id, upload_param_id));

        public Task<bool> AddAsync(Model.alarm_rule model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.alarm_rule model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(string equipment_id, string upload_param_id) =>
            Task.Run(() => Delete(equipment_id, upload_param_id));

        public Task<Model.alarm_rule?> GetModelAsync(string equipment_id, string upload_param_id) =>
            Task.Run(() => GetModel(equipment_id, upload_param_id));

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public async Task<Model.alarm_rule?> GetModelByCacheAsync(
            string equipment_id,
            string upload_param_id
        )
        {
            string cacheKey = $"alarm_ruleModel-{equipment_id}:{upload_param_id}";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );

                    return Task.FromResult(_dal.GetModel(equipment_id, upload_param_id));
                }
            );
        }

        public Task<List<Model.alarm_rule>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
