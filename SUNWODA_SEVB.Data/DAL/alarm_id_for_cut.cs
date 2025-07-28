using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:alarm_id_for_cut
    /// </summary>
    public partial class alarm_id_for_cut
    {
        public alarm_id_for_cut() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("did", "alarm_id_for_cut");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int did)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from alarm_id_for_cut");
            strSql.Append(" where did=@did ");
            MySqlParameter[] parameters = { new MySqlParameter("@did", MySqlDbType.Int32, 11) };
            parameters[0].Value = did;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.alarm_id_for_cut model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into alarm_id_for_cut(");
            strSql.Append("did)");
            strSql.Append(" values (");
            strSql.Append("@did)");
            MySqlParameter[] parameters = { new MySqlParameter("@did", MySqlDbType.Int32, 11) };
            parameters[0].Value = model.did;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SUNWODA_SEVB.Data.Model.alarm_id_for_cut model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update alarm_id_for_cut set ");
            strSql.Append("did=@did");
            strSql.Append(" where did=@did ");
            MySqlParameter[] parameters = { new MySqlParameter("@did", MySqlDbType.Int32, 11) };
            parameters[0].Value = model.did;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int did)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from alarm_id_for_cut ");
            strSql.Append(" where did=@did ");
            MySqlParameter[] parameters = { new MySqlParameter("@did", MySqlDbType.Int32, 11) };
            parameters[0].Value = did;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string didlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from alarm_id_for_cut ");
            strSql.Append(" where did in (" + didlist + ")  ");
            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SUNWODA_SEVB.Data.Model.alarm_id_for_cut? GetModel(int did)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select did from alarm_id_for_cut ");
            strSql.Append(" where did=@did ");
            MySqlParameter[] parameters = { new MySqlParameter("@did", MySqlDbType.Int32, 11) };
            parameters[0].Value = did;

            SUNWODA_SEVB.Data.Model.alarm_id_for_cut model = new SUNWODA_SEVB.Data.Model.alarm_id_for_cut();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SUNWODA_SEVB.Data.Model.alarm_id_for_cut DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.alarm_id_for_cut model = new SUNWODA_SEVB.Data.Model.alarm_id_for_cut();
            if (row != null)
            {
                if (row["did"] != null && row["did"].ToString() != "")
                {
                    model.did = int.Parse(row["did"].ToString()!);
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select did ");
            strSql.Append(" FROM alarm_id_for_cut ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM alarm_id_for_cut ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object? obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.did desc");
            }
            strSql.Append(")AS Row, T.*  from alarm_id_for_cut T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            MySqlParameter[] parameters = {
                    new MySqlParameter("@tblName", MySqlDbType.VarChar, 255),
                    new MySqlParameter("@fldName", MySqlDbType.VarChar, 255),
                    new MySqlParameter("@PageSize", MySqlDbType.Int32),
                    new MySqlParameter("@PageIndex", MySqlDbType.Int32),
                    new MySqlParameter("@IsReCount", MySqlDbType.Bit),
                    new MySqlParameter("@OrderType", MySqlDbType.Bit),
                    new MySqlParameter("@strWhere", MySqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "alarm_id_for_cut";
            parameters[1].Value = "did";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperMySQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}
