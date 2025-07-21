using System;
using System.Data;
using System.Text;
using Maticsoft.DBUtility; //Please add references
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:log4net
    /// </summary>
    public partial class log_operation
    {
        public log_operation() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "log_operation");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from log_operation");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.log_operation model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into log_operation(");
            strSql.Append("logdate,operationMsg,userName,softwareName)");
            strSql.Append(" values (");
            strSql.Append("@logdate,@operationMsg,@userName,@softwareName)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@logdate", MySqlDbType.DateTime),
                new MySqlParameter("@operationMsg", MySqlDbType.MediumText, 1000),
                new MySqlParameter("@userName", MySqlDbType.VarChar, 50),
                new MySqlParameter("@softwareName", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.logdate;
            parameters[1].Value = model.operationMsg;
            parameters[2].Value = model.userName;
            parameters[3].Value = model.softwareName;

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
        public bool Update(SUNWODA_SEVB.Data.Model.log_operation model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update log_operation set ");
            strSql.Append("logdate=@logdate,");
            strSql.Append("operationMsg=@operationMsg,");
            strSql.Append("userName=@userName,");
            strSql.Append("softwareName=@softwareName");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@logdate", MySqlDbType.DateTime),
                new MySqlParameter("@operationMsg", MySqlDbType.VarChar, 50),
                new MySqlParameter("@userName", MySqlDbType.VarChar, 50),
                new MySqlParameter("@softwareName", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.logdate;
            parameters[1].Value = model.operationMsg;
            parameters[2].Value = model.userName;
            parameters[3].Value = model.softwareName;
            parameters[4].Value = model.id;

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
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from log_operation ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

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
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from log_operation ");
            strSql.Append(" where id in (" + idlist + ")  ");
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
        public SUNWODA_SEVB.Data.Model.log_operation? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,logdate,operationMsg,userName,softwareName from log_operation "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.log_operation model = new SUNWODA_SEVB.Data.Model.log_operation();
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
        public SUNWODA_SEVB.Data.Model.log_operation DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.log_operation model = new SUNWODA_SEVB.Data.Model.log_operation();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["logdate"] != null && row["logdate"].ToString() != "")
                {
                    model.logdate = DateTime.Parse(row["logdate"].ToString()!);
                }
                if (row["operationMsg"] != null)
                {
                    model.operationMsg = row["operationMsg"].ToString()!;
                }
                if (row["userName"] != null)
                {
                    model.userName = row["userName"].ToString()!;
                }
                if (row["softwareName"] != null)
                {
                    model.softwareName = row["softwareName"].ToString()!;
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
            strSql.Append("select id,logdate,operationMsg,userName,softwareName ");
            strSql.Append(" FROM log_operation ");
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
            strSql.Append("select count(1) FROM log_operation ");
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
                strSql.Append("order by T.id desc");
            }
            strSql.Append(")AS Row, T.*  from log_operation T ");
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
            parameters[0].Value = "log4net";
            parameters[1].Value = "id";
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
