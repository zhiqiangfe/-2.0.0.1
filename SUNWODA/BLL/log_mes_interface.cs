using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SUNWODA_SEVB.Data.DAL;
using SUNWODA_SEVB.Data.Model;

namespace SUNWODA_SEVB.Data.BLL
{
    /// <summary>
    /// BLL for log_mes_interface
    /// </summary>
    public partial class log_mes_interface
    {
        private readonly DAL.log_mes_interface _dal = new DAL.log_mes_interface();

        public log_mes_interface() { }

        #region BasicMethod

        public int GetMaxId() => _dal.GetMaxId();

        public bool Exists(int id) => _dal.Exists(id);

        public bool Add(Model.log_mes_interface model) => _dal.Add(model);

        /// <summary>
        ///
        /// </summary>
        public bool Update(Model.log_mes_interface model) => _dal.Update(model);

        public bool Delete(int id) => _dal.Delete(id);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist) => _dal.DeleteList(idlist);

        public Model.log_mes_interface? GetModel(int id) => _dal.GetModel(id);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.log_mes_interface> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.log_mes_interface> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.log_mes_interface>();
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

        public Task<int> GetMaxIdAsync() => Task.Run(() => GetMaxId());

        public Task<bool> ExistsAsync(int id) => Task.Run(() => Exists(id));

        public Task<bool> AddAsync(Model.log_mes_interface model) => Task.Run(() => Add(model));

        public Task<bool> UpdateAsync(Model.log_mes_interface model) =>
            Task.Run(() => Update(model));

        public Task<bool> DeleteAsync(int id) => Task.Run(() => Delete(id));

        public Task<bool> DeleteListAsync(string idlist) => Task.Run(() => DeleteList(idlist));

        public Task<Model.log_mes_interface?> GetModelAsync(int id) => Task.Run(() => GetModel(id));

        public Task<List<Model.log_mes_interface>> GetModelListAsync(string strWhere)
        {
            return Task.Run(() => GetModelList(strWhere));
        }

        #endregion
    }
}
