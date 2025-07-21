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
    /// BLL for alarm_temp
    /// </summary>
    public partial class alarm_temp
    {
        private readonly DAL.alarm_temp _dal = new DAL.alarm_temp();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public alarm_temp()
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
        /// 得到最大ID
        /// </summary>
        public int GetMaxId() => _dal.GetMaxId();

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id) => _dal.Exists(id);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.alarm_temp model) => _dal.Add(model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.alarm_temp model) => _dal.Update(model);

        /// <summary>
        /// 更新报警数据 (Custom Method)
        /// </summary>
        public bool UpdateAlarmTime(Model.alarm_temp model) => _dal.UpdateAlarmTime(model);

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
        public Model.alarm_temp? GetModel(int id) => _dal.GetModel(id);

        /// <summary>
        /// 通过UploadParamID得到一个对象实体
        /// </summary>
        public Model.alarm_temp? GetModelByUploadparamID(string uploadParamID) =>
            _dal.GetModelByUploadparamID(uploadParamID);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.alarm_temp? GetModelByCache(int id)
        {
            string cacheKey = $"alarm_tempModel-{id}";

            if (!_memoryCache.TryGetValue(cacheKey, out Model.alarm_temp? model))
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
        public List<Model.alarm_temp> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.alarm_temp> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.alarm_temp>();
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

        public Task<bool> AddAsync(Model.alarm_temp model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.alarm_temp model) => Task.Run(() => Update(model));

        public Task<bool> UpdateAlarmTimeAsync(Model.alarm_temp model) =>
            Task.Run(() => UpdateAlarmTime(model));

        public Task<bool> DeleteAsync(int id) => Task.Run(() => Delete(id));

        public Task<Model.alarm_temp?> GetModelAsync(int id) => Task.Run(() => GetModel(id));

        public Task<Model.alarm_temp?> GetModelByUploadparamIDAsync(string uploadParamID) =>
            Task.Run(() => GetModelByUploadparamID(uploadParamID));

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public async Task<Model.alarm_temp?> GetModelByCacheAsync(int id)
        {
            string cacheKey = $"alarm_tempModel-{id}";

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

        public Task<List<Model.alarm_temp>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
