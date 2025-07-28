using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SUNWODA_SEVB.Data.DAL;
using SUNWODA_SEVB.Data.Model;

namespace SUNWODA_SEVB.Data.BLL
{
    /// <summary>
    /// BLL for users
    /// </summary>
    public partial class users_info
    {
        private readonly DAL.users_info _dal ;
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationMinutes;

        public users_info(DAL.users_info dal, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _dal = dal;
            _memoryCache = memoryCache;
            // 从配置中读取缓存持续时间，默认回退时间为 5 分钟
            _cacheDurationMinutes = configuration.GetValue<int>("AppSettings:ModelCacheMinutes", 5);
        }

        #region BasicMethod

        public int GetMaxId() => _dal.GetMaxId();

        public bool Exists(int id) => _dal.Exists(id);

        public bool Add(Model.users_info model) => _dal.Add(model);

        public bool Update(Model.users_info model)
        {
            bool success = _dal.Update(model);
            if (success)
            {
                _memoryCache.Remove($"usersModel-{model.id}");
            }
            return success;
        }

        public bool Delete(int id)
        {
            bool success = _dal.Delete(id);
            if (success)
            {
                _memoryCache.Remove($"usersModel-{id}");
            }
            return success;
        }

        /// <summary>
        ///
        /// </summary>
        public bool DeleteList(string idlist) => _dal.DeleteList(idlist);

        public Model.users_info? GetModel(int id) => _dal.GetModel(id);

        /// <summary>
        ///
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        ///
        /// </summary>
        public List<Model.users_info> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.users_info> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.users_info>();
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

        public DataSet GetAllList() => GetList(string.Empty);

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

        public Task<bool> ExistsAsync(int id) => Task.Run(() => Exists(id));

        public Task<bool> AddAsync(Model.users_info model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.users_info model)
        {
            return Task.Run(() => Update(model));
        }

        public Task<bool> DeleteAsync(int id)
        {
            return Task.Run(() => Delete(id));
        }

        public Task<Model.users_info?> GetModelAsync(int id) => Task.Run(() => GetModel(id));

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Model.users_info?> GetModelByCacheAsync(int id)
        {
            string cacheKey = $"usersModel-{id}";

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

        #endregion
    }
}
