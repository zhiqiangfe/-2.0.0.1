using System;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    public partial class log_mes_interface
    {
        public log_mes_interface() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "log_mes_interface");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from log_mes_interface");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.log_mes_interface model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into log_mes_interface(");
            strSql.Append(
                "logdate,method,input_json,output_json,start_time,end_time,consuming_time,success_flag)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@logdate,@method,@input_json,@output_json,@start_time,@end_time,@consuming_time,@success_flag)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@logdate", MySqlDbType.DateTime),
                new MySqlParameter("@method", MySqlDbType.VarChar, 50),
                new MySqlParameter("@input_json", MySqlDbType.MediumText),
                new MySqlParameter("@output_json", MySqlDbType.MediumText),
                new MySqlParameter("@start_time", MySqlDbType.DateTime),
                new MySqlParameter("@end_time", MySqlDbType.DateTime),
                new MySqlParameter("@consuming_time", MySqlDbType.VarChar, 50),
                new MySqlParameter("@success_flag", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.logdate;
            parameters[1].Value = model.method;
            parameters[2].Value = model.input_json;
            parameters[3].Value = model.output_json;
            parameters[4].Value = model.start_time;
            parameters[5].Value = model.end_time;
            parameters[6].Value = model.consuming_time;
            parameters[7].Value = model.success_flag;
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
        public bool Update(SUNWODA_SEVB.Data.Model.log_mes_interface model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update log_mes_interface set ");
            strSql.Append("logdate=@logdate,");
            strSql.Append("method=@method,");
            strSql.Append("input_json=@input_json,");
            strSql.Append("output_json=@output_json,");
            strSql.Append("start_time=@start_time,");
            strSql.Append("end_time=@end_time, ");
            strSql.Append("consuming_time=@consuming_time, ");
            strSql.Append("success_flag=@success_flag ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@logdate", MySqlDbType.DateTime),
                new MySqlParameter("@method", MySqlDbType.VarChar, 50),
                new MySqlParameter("@input_json", MySqlDbType.VarChar, 50),
                new MySqlParameter("@output_json", MySqlDbType.VarChar, 50),
                new MySqlParameter("@start_time", MySqlDbType.DateTime),
                new MySqlParameter("@end_time", MySqlDbType.DateTime),
                new MySqlParameter("@consuming_time", MySqlDbType.VarChar, 50),
                new MySqlParameter("@success_flag", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.logdate;
            parameters[1].Value = model.method;
            parameters[2].Value = model.input_json;
            parameters[3].Value = model.output_json;
            parameters[4].Value = model.start_time;
            parameters[5].Value = model.end_time;
            parameters[6].Value = model.consuming_time;
            parameters[7].Value = model.success_flag;
            parameters[8].Value = model.id;

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
            strSql.Append("delete from log_mes_interface ");
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
            strSql.Append("delete from log_mes_interface ");
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
        public SUNWODA_SEVB.Data.Model.log_mes_interface? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,logdate,method,input_json,output_json,start_time,end_time,consuming_time,success_flag from log_mes_interface "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.log_mes_interface model = new SUNWODA_SEVB.Data.Model.log_mes_interface();
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
        public SUNWODA_SEVB.Data.Model.log_mes_interface DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.log_mes_interface model = new SUNWODA_SEVB.Data.Model.log_mes_interface();
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
                if (row["method"] != null)
                {
                    model.method = row["method"].ToString()!;
                }
                if (row["input_json"] != null)
                {
                    model.input_json = row["input_json"].ToString()!;
                }
                if (row["output_json"] != null)
                {
                    model.output_json = row["output_json"].ToString()!;
                }
                if (row["start_time"] != null)
                {
                    model.start_time = DateTime.Parse(row["start_time"].ToString()!);
                }
                if (row["end_time"] != null)
                {
                    model.end_time = DateTime.Parse(row["end_time"].ToString()!);
                }
                if (row["consuming_time"] != null)
                {
                    model.consuming_time = row["consuming_time"].ToString()!;
                }
                if (row["success_flag"] != null)
                {
                    model.success_flag = row["success_flag"].ToString()!;
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
            strSql.Append(
                "select id,logdate,method,input_json,output_json,start_time,end_time,consuming_time,success_flag "
            );
            strSql.Append(" FROM log_mes_interface ");
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
            strSql.Append("select count(1) FROM log_mes_interface ");
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
            strSql.Append(")AS Row, T.*  from log_mes_interface T ");
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
