using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SUNWODA_SEVB.Data.DAL;
using SUNWODA_SEVB.Data.Model;

namespace SUNWODA_SEVB.Data.BLL
{
    /// <summary>
    /// BLL for role
    /// </summary>
    public partial class user_role
    {
        private readonly DAL.user_role _dal;
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public user_role(DAL.user_role dal, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _dal = dal;
            _memoryCache = memoryCache;
            // 从配置中读取缓存持续时间，默认回退时间为 5 分钟
            _cacheDurationMinutes = configuration.GetValue<int>("AppSettings:ModelCacheMinutes", 5);
        }

        #region BasicMethod

        public int GetMaxId() => _dal.GetMaxId();

        public bool Exists(int id) => _dal.Exists(id);

        public bool Add(Model.user_role model)
        {
            return _dal.Add(model);
        }

        public bool Update(Model.user_role model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"roleModel-{model.id}");
            }
            return success;
        }

        public bool Delete(int id)
        {
            bool success = _dal.Delete(id);
            if (success)
            {
                _memoryCache.Remove($"roleModel-{id}");
            }
            return success;
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist) => _dal.DeleteList(idlist);

        public Model.user_role? GetModel(int id) => _dal.GetModel(id);

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Model.user_role? GetModelByCache(int id)
        {
            return GetModelByCacheAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.user_role> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.user_role> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.user_role>();
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

        public Task<bool> ExistsAsync(int id) => Task.Run(() => Exists(id));

        public Task<bool> AddAsync(Model.user_role model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.user_role model) => Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(int id) => Task.Run(() => Delete(id));

        /// <summary>
        ///
        /// </summary>
        public Task<Model.user_role?> GetModelByCacheAsync(int id)
        {
            string cacheKey = $"roleModel-{id}";

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

        public Task<List<Model.user_role>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
