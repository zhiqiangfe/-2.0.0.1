using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Maticsoft.DBUtility; //Please add references
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:user_define_variable_data
    /// </summary>
    public partial class user_define_variable_data
    {
        public user_define_variable_data() { }

        #region  BasicMethod



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.user_define_variable_data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_define_variable_data(");
            strSql.Append("variable_id,value_name,value_type,value,remark,data_time)");
            strSql.Append(" values (");
            strSql.Append("@variable_id,@value_name,@value_type,@value,@remark,@data_time)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@variable_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@value_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
                new MySqlParameter("@data_time", MySqlDbType.DateTime),
            };
            parameters[0].Value = model.variable_id;
            parameters[1].Value = model.value_name;
            parameters[2].Value = model.value_type;
            parameters[3].Value = model.value;
            parameters[4].Value = model.remark;
            parameters[5].Value = model.data_time;

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
        /// 增加多条条数据
        /// </summary>
        public bool Adds(List<SUNWODA_SEVB.Data.Model.user_define_variable_data> models)
        {
            Hashtable hashtable = new Hashtable();
            foreach (SUNWODA_SEVB.Data.Model.user_define_variable_data model in models)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into user_define_variable_data(");
                strSql.Append("variable_id,value_name,value_type,value,remark,data_time)");
                strSql.Append(" values (");
                strSql.Append("@variable_id,@value_name,@value_type,@value,@remark,@data_time)");
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@variable_id", MySqlDbType.Int32, 11),
                    new MySqlParameter("@value_name", MySqlDbType.VarChar, 50),
                    new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
                    new MySqlParameter("@value", MySqlDbType.VarChar, 50),
                    new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
                    new MySqlParameter("@data_time", MySqlDbType.DateTime),
                };
                parameters[0].Value = model.variable_id;
                parameters[1].Value = model.value_name;
                parameters[2].Value = model.value_type;
                parameters[3].Value = model.value;
                parameters[4].Value = model.remark;
                parameters[5].Value = model.data_time;
                hashtable.Add(strSql, parameters);
            }
            DbHelperMySQL.ExecuteSqlTranWithIndentity(hashtable);
            return true;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SUNWODA_SEVB.Data.Model.user_define_variable_data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_define_variable_data set ");
            strSql.Append("variable_id=@variable_id,");
            strSql.Append("value_name=@value_name,");
            strSql.Append("value_type=@value_type,");
            strSql.Append("value=@value,");
            strSql.Append("remark=@remark,");
            strSql.Append("data_time=@data_time");
            strSql.Append(" where ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@variable_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@value_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
                new MySqlParameter("@data_time", MySqlDbType.DateTime),
            };
            parameters[0].Value = model.variable_id;
            parameters[1].Value = model.value_name;
            parameters[2].Value = model.value_type;
            parameters[3].Value = model.value;
            parameters[4].Value = model.remark;
            parameters[5].Value = model.data_time;

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
        public bool Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from user_define_variable_data ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = { };

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
        /// 得到一个对象实体
        /// </summary>
        public SUNWODA_SEVB.Data.Model.user_define_variable_data? GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select variable_id,value_name,value_type,value,remark,data_time from user_define_variable_data "
            );
            strSql.Append(" where ");
            MySqlParameter[] parameters = { };

            SUNWODA_SEVB.Data.Model.user_define_variable_data model =
                new SUNWODA_SEVB.Data.Model.user_define_variable_data();
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
        public SUNWODA_SEVB.Data.Model.user_define_variable_data DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.user_define_variable_data model =
                new SUNWODA_SEVB.Data.Model.user_define_variable_data();
            if (row != null)
            {
                if (row["variable_id"] != null && row["variable_id"].ToString() != "")
                {
                    model.variable_id = int.Parse(row["variable_id"].ToString()!);
                }
                if (row["value_name"] != null)
                {
                    model.value_name = row["value_name"].ToString()!;
                }
                if (row["value_type"] != null)
                {
                    model.value_type = row["value_type"].ToString()!;
                }
                if (row["value"] != null)
                {
                    model.value = row["value"].ToString()!;
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString()!;
                }
                if (row["data_time"] != null && row["data_time"].ToString() != "")
                {
                    model.data_time = DateTime.Parse(row["data_time"].ToString()!);
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
            strSql.Append("select variable_id,value_name,value_type,value,remark,data_time ");
            strSql.Append(" FROM user_define_variable_data ");
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
            strSql.Append("select count(1) FROM user_define_variable_data ");
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
            strSql.Append(")AS Row, T.*  from user_define_variable_data T ");
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
            parameters[0].Value = "user_define_variable_data";
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
