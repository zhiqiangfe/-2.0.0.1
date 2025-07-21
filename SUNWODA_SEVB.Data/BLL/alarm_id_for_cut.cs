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
    /// BLL for alarm_id_for_cut
    /// </summary>
    public partial class alarm_id_for_cut
    {
        private readonly DAL.alarm_id_for_cut _dal = new DAL.alarm_id_for_cut();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public alarm_id_for_cut()
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

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId() => _dal.GetMaxId();

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int did) => _dal.Exists(did);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.alarm_id_for_cut model) => _dal.Add(model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.alarm_id_for_cut model) => _dal.Update(model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int did) => _dal.Delete(did);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string didlist) => _dal.DeleteList(didlist);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.alarm_id_for_cut? GetModel(int did) => _dal.GetModel(did);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.alarm_id_for_cut? GetModelByCache(int did)
        {
            string cacheKey = $"alarm_id_for_cutModel-{did}";

            // TryGetValue 是从缓存中获取的标准方法
            if (!_memoryCache.TryGetValue(cacheKey, out Model.alarm_id_for_cut? model))
            {
                // 键不在缓存中，因此从源获取数据
                model = _dal.GetModel(did);
                if (model != null)
                {
                    // 设置缓存选项
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(
                        TimeSpan.FromMinutes(_cacheDurationMinutes)
                    );

                    // 将数据保存在缓存中
                    _memoryCache.Set(cacheKey, model, cacheEntryOptions);
                }
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
        public List<Model.alarm_id_for_cut> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.alarm_id_for_cut> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.alarm_id_for_cut>();
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

        #region  ExtensionMethod

        // 即使 DAL 是同步的，这些方法也提供了异步接口
        // Task.Run 将阻塞的 DB 调用卸载到线程池线程。

        public Task<bool> ExistsAsync(int did) => Task.Run(() => Exists(did));

        public Task<bool> AddAsync(Model.alarm_id_for_cut model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.alarm_id_for_cut model) =>
            Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(int did) => Task.Run(() => Delete(did));

        public Task<Model.alarm_id_for_cut?> GetModelAsync(int did) =>
            Task.Run(() => GetModel(did));

        /// <summary>
        /// 得到一个对象实体，从缓存中 (异步版本)
        /// </summary>
        public async Task<Model.alarm_id_for_cut?> GetModelByCacheAsync(int did)
        {
            string cacheKey = $"alarm_id_for_cutModel-{did}";

            // GetOrCreateAsync 是一种线程安全且方便的处理缓存的方法
            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );

                    // 仅当该项目不在缓存中时才会调用此工厂函数
                    // 将同步 DAL 调用包装在异步工厂的任务中
                    return Task.FromResult(_dal.GetModel(did));
                }
            );
        }

        public Task<List<Model.alarm_id_for_cut>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
