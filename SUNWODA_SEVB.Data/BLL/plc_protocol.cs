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
    /// BLL for plc_protocol
    /// </summary>
    public partial class plc_protocol
    {
        private readonly DAL.plc_protocol _dal = new DAL.plc_protocol();
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public plc_protocol()
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

        public bool Exists(string protocol_name) => _dal.Exists(protocol_name);

        public bool Add(Model.plc_protocol model)
        {
            bool success = _dal.Add(model);
            if (success)
            {
                _memoryCache.Remove($"plc_protocolModel-{model.protocol_name}");
            }
            return success;
        }

        public bool Update(Model.plc_protocol model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"plc_protocolModel-{model.protocol_name}");
            }
            return success;
        }

        public bool Delete(string protocol_name)
        {
            bool success = _dal.Delete(protocol_name);
            if (success)
            {
                _memoryCache.Remove($"plc_protocolModel-{protocol_name}");
            }
            return success;
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string protocol_namelist) => _dal.DeleteList(protocol_namelist);

        public Model.plc_protocol? GetModel(string protocol_name) => _dal.GetModel(protocol_name);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.plc_protocol? GetModelByCache(string protocol_name)
        {
            return GetModelByCacheAsync(protocol_name).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.plc_protocol> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.plc_protocol> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.plc_protocol>();
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

        public Task<bool> ExistsAsync(string protocol_name) =>
            Task.Run(() => Exists(protocol_name));

        public Task<bool> AddAsync(Model.plc_protocol model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.plc_protocol model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(string protocol_name) =>
            Task.Run(() => Delete(protocol_name));

        /// <summary>
        ///
        /// </summary>
        public Task<Model.plc_protocol?> GetModelByCacheAsync(string protocol_name)
        {
            string cacheKey = $"plc_protocolModel-{protocol_name}";

            return _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(
                        _cacheDurationMinutes
                    );

                    return Task.FromResult(_dal.GetModel(protocol_name));
                }
            );
        }

        public Task<List<Model.plc_protocol>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
