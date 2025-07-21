using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SUNWODA_SEVB.Data.DAL;
using SUNWODA_SEVB.Data.Model;

namespace SUNWODA_SEVB.Data.BLL
{
    /// <summary>
    /// BLL for user_define_variable_data
    /// </summary>
    public partial class user_define_variable_data
    {
        private readonly DAL.user_define_variable_data _dal = new DAL.user_define_variable_data();

        public user_define_variable_data() { }

        #region BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.user_define_variable_data model) => _dal.Add(model);

        /// <summary>
        /// 增加多条数据
        /// </summary>
        public bool Adds(List<Model.user_define_variable_data> models) => _dal.Adds(models);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.user_define_variable_data model) => _dal.Update(model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete() => _dal.Delete();

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.user_define_variable_data? GetModel() => _dal.GetModel();

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) => _dal.GetList(strWhere);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.user_define_variable_data> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.user_define_variable_data> DataTableToList(DataTable dt)
        {
            var modelList = new List<Model.user_define_variable_data>();
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

        public Task<bool> AddAsync(Model.user_define_variable_data model) =>
            Task.Run(() => Add(model));

        public Task<bool> AddsAsync(List<Model.user_define_variable_data> models) =>
            Task.Run(() => Adds(models));

        #endregion
    }
}
