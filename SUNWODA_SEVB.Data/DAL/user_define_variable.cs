using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:user_define_variable
    /// </summary>
    public partial class user_define_variable
    {
        private readonly DbHelperMySQL _dbHelper;
        public user_define_variable(DbHelperMySQL dbHelper) { _dbHelper = dbHelper; }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return _dbHelper.GetMaxID("id", "user_define_variable");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from user_define_variable");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return _dbHelper.Exists(strSql.ToString(), parameters);
        }

        public bool ExistsName(string variable_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from user_define_variable");
            strSql.Append(" where variable_name=@variable_name");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@variable_name", MySqlDbType.String),
            };
            parameters[0].Value = variable_name;
            return _dbHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.user_define_variable model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_define_variable(");
            strSql.Append(
                "variable_name,variable_type_id,value,unit,description,value_type,variable_length,plc_rw_config_id,plc_address,is_monitor,remark,datatime)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@variable_name,@variable_type_id,@value,@unit,@description,@value_type,@variable_length,@plc_rw_config_id,@plc_address,@is_monitor,@remark,@datatime)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@variable_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@variable_type_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@unit", MySqlDbType.VarChar, 50),
                new MySqlParameter("@description", MySqlDbType.VarChar, 50),
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@variable_length", MySqlDbType.Int32, 10),
                new MySqlParameter("@plc_rw_config_id", MySqlDbType.Int32, 10),
                new MySqlParameter("@plc_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@is_monitor", MySqlDbType.Bit),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
                new MySqlParameter("@datatime", MySqlDbType.DateTime),
            };
            parameters[0].Value = model.variable_name;
            parameters[1].Value = model.variable_type_id;
            parameters[2].Value = model.value;
            parameters[3].Value = model.unit;
            parameters[4].Value = model.description;
            parameters[5].Value = model.value_type;
            parameters[6].Value = model.variable_length;
            parameters[7].Value = model.plc_rw_config_id;
            parameters[8].Value = model.plc_address;
            parameters[9].Value = model.is_monitor;
            parameters[10].Value = model.remark;
            parameters[11].Value = model.datatime;

            int rows = _dbHelper.ExecuteSql(strSql.ToString(), parameters);
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
        /// 更新数据的value
        /// </summary>
        /// <param name="variable_name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateValue(string variable_name, string value)
        {
            string sql =
                $"update user_define_variable set value='{value}' where variable_name ='{variable_name}'";
            int rows = _dbHelper.ExecuteSql(sql);
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
        public bool Update(SUNWODA_SEVB.Data.Model.user_define_variable model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_define_variable set ");
            strSql.Append("variable_name=@variable_name,");
            strSql.Append("variable_type_id=@variable_type_id,");
            strSql.Append("value=@value,");
            strSql.Append("unit=@unit,");
            strSql.Append("description=@description,");
            strSql.Append("value_type=@value_type,");
            strSql.Append("variable_length=@variable_length,");
            strSql.Append("plc_rw_config_id=@plc_rw_config_id,");
            strSql.Append("plc_address=@plc_address,");
            strSql.Append("is_monitor=@is_monitor,");
            strSql.Append("remark=@remark,");
            strSql.Append("datatime=@datatime");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@variable_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@variable_type_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@unit", MySqlDbType.VarChar, 50),
                new MySqlParameter("@description", MySqlDbType.VarChar, 50),
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@variable_length", MySqlDbType.Int32, 10),
                new MySqlParameter("@plc_rw_config_id", MySqlDbType.Int32, 10),
                new MySqlParameter("@plc_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@is_monitor", MySqlDbType.Bit),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
                new MySqlParameter("@datatime", MySqlDbType.DateTime),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.variable_name;
            parameters[1].Value = model.variable_type_id;
            parameters[2].Value = model.value;
            parameters[3].Value = model.unit;
            parameters[4].Value = model.description;
            parameters[5].Value = model.value_type;
            parameters[6].Value = model.variable_length;
            parameters[7].Value = model.plc_rw_config_id;
            parameters[8].Value = model.plc_address;
            parameters[9].Value = model.is_monitor;
            parameters[10].Value = model.remark;
            parameters[11].Value = model.datatime;
            parameters[12].Value = model.id;

            int rows = _dbHelper.ExecuteSql(strSql.ToString(), parameters);
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
        /// 更新一条数据通过Id
        /// </summary>
        public bool UpdateValueById(SUNWODA_SEVB.Data.Model.user_define_variable model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_define_variable set ");
            strSql.Append("value=@value");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.value;
            parameters[1].Value = model.id;

            int rows = _dbHelper.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append("delete from user_define_variable ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            int rows = _dbHelper.ExecuteSql(strSql.ToString(), parameters);
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
        public bool DeleteByName(string variable_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from user_define_variable ");
            strSql.Append(" where variable_name=@variable_name");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@variable_name", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = variable_name;

            int rows = _dbHelper.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append("delete from user_define_variable ");
            strSql.Append(" where id in (" + idlist + ")  ");
            int rows = _dbHelper.ExecuteSql(strSql.ToString());
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
        public SUNWODA_SEVB.Data.Model.user_define_variable? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,variable_name,variable_type_id,value,unit,description,value_type,variable_length,plc_rw_config_id,plc_address,is_monitor,remark,datatime from user_define_variable "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.user_define_variable model = new SUNWODA_SEVB.Data.Model.user_define_variable();
            DataSet ds = _dbHelper.Query(strSql.ToString(), parameters);
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
        public SUNWODA_SEVB.Data.Model.user_define_variable DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.user_define_variable model = new SUNWODA_SEVB.Data.Model.user_define_variable();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["variable_name"] != null)
                {
                    model.variable_name = row["variable_name"].ToString()!;
                }
                if (row["variable_type_id"] != null && row["variable_type_id"].ToString() != "")
                {
                    model.variable_type_id = int.Parse(row["variable_type_id"].ToString()!);
                }
                if (row["value"] != null)
                {
                    model.value = row["value"].ToString()!;
                }
                if (row["unit"] != null)
                {
                    model.unit = row["unit"].ToString()!;
                }
                if (row["description"] != null)
                {
                    model.description = row["description"].ToString()!;
                }
                if (row["value_type"] != null)
                {
                    model.value_type = row["value_type"].ToString()!;
                }
                if (row["variable_length"] != null && row["variable_length"].ToString() != "")
                {
                    model.variable_length = int.Parse(row["variable_length"].ToString()!);
                }
                if (row["plc_rw_config_id"] != null && row["plc_rw_config_id"].ToString() != "")
                {
                    model.plc_rw_config_id = int.Parse(row["plc_rw_config_id"].ToString()!);
                }
                if (row["plc_address"] != null)
                {
                    model.plc_address = row["plc_address"].ToString()!;
                }
                if (row["is_monitor"] != null && row["is_monitor"].ToString() != "")
                {
                    if (
                        (row["is_monitor"].ToString() == "1")
                        || (row["is_monitor"].ToString()!.ToLower() == "true")
                    )
                    {
                        model.is_monitor = true;
                    }
                    else
                    {
                        model.is_monitor = false;
                    }
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString()!;
                }
                if (row["datatime"] != null && row["datatime"].ToString() != "")
                {
                    model.datatime = DateTime.Parse(row["datatime"].ToString()!);
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
                "select id,variable_name,variable_type_id,value,unit,description,value_type,variable_length,plc_rw_config_id,plc_address,is_monitor,remark,datatime "
            );
            strSql.Append(" FROM user_define_variable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return _dbHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM user_define_variable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object? obj = _dbHelper.GetSingle(strSql.ToString());
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
            strSql.Append(")AS Row, T.*  from user_define_variable T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return _dbHelper.Query(strSql.ToString());
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
            parameters[0].Value = "user_define_variable";
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
