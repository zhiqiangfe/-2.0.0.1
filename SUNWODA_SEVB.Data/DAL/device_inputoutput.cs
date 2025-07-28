using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device_inputoutput
    /// </summary>
    public partial class device_inputoutput
    {
        public device_inputoutput() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "device_inputoutput");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_inputoutput");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_inputoutput model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_inputoutput(");
            strSql.Append(
                "equipment_id,send_param_id,upload_param_id,param_name,type,set_value,upper_limit_value,lower_limit_value,limit_control,change_monitor,actual_value,bycell_output,param_value_rate)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@equipment_id,@send_param_id,@upload_param_id,@param_name,@type,@set_value,@upper_limit_value,@lower_limit_value,@limit_control,@change_monitor,@actual_value,@bycell_output,@param_value_rate)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@send_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@type", MySqlDbType.VarChar, 20),
                new MySqlParameter("@set_value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upper_limit_value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@lower_limit_value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@limit_control", MySqlDbType.VarChar, 50),
                new MySqlParameter("@change_monitor", MySqlDbType.VarChar, 50),
                new MySqlParameter("@actual_value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@bycell_output", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_value_rate", MySqlDbType.Float),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.send_param_id;
            parameters[2].Value = model.upload_param_id;
            parameters[3].Value = model.param_name;
            parameters[4].Value = model.type;
            parameters[5].Value = model.set_value;
            parameters[6].Value = model.upper_limit_value;
            parameters[7].Value = model.lower_limit_value;
            parameters[8].Value = model.limit_control;
            parameters[9].Value = model.change_monitor;
            parameters[10].Value = model.actual_value;
            parameters[11].Value = model.bycell_output;
            parameters[12].Value = model.param_value_rate;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_inputoutput model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_inputoutput set ");
            strSql.Append("equipment_id=@equipment_id,");
            strSql.Append("send_param_id=@send_param_id,");
            strSql.Append("upload_param_id=@upload_param_id,");
            strSql.Append("param_name=@param_name,");
            strSql.Append("type=@type,");
            strSql.Append("set_value=@set_value,");
            strSql.Append("upper_limit_value=@upper_limit_value,");
            strSql.Append("lower_limit_value=@lower_limit_value,");
            strSql.Append("limit_control=@limit_control,");
            strSql.Append("change_monitor=@change_monitor,");
            strSql.Append("actual_value=@actual_value,");
            strSql.Append("bycell_output=@bycell_output,");
            strSql.Append("param_value_rate=@param_value_rate");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@send_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@type", MySqlDbType.VarChar, 20),
                new MySqlParameter("@set_value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upper_limit_value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@lower_limit_value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@limit_control", MySqlDbType.VarChar, 50),
                new MySqlParameter("@change_monitor", MySqlDbType.VarChar, 50),
                new MySqlParameter("@actual_value", MySqlDbType.VarChar, 50),
                new MySqlParameter("@bycell_output", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_value_rate", MySqlDbType.Float),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.send_param_id;
            parameters[2].Value = model.upload_param_id;
            parameters[3].Value = model.param_name;
            parameters[4].Value = model.type;
            parameters[5].Value = model.set_value;
            parameters[6].Value = model.upper_limit_value;
            parameters[7].Value = model.lower_limit_value;
            parameters[8].Value = model.limit_control;
            parameters[9].Value = model.change_monitor;
            parameters[10].Value = model.actual_value;
            parameters[11].Value = model.bycell_output;
            parameters[12].Value = model.param_value_rate;
            parameters[13].Value = model.id;

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
            strSql.Append("delete from device_inputoutput ");
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
            strSql.Append("delete from device_inputoutput ");
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
        public SUNWODA_SEVB.Data.Model.device_inputoutput? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,equipment_id,send_param_id,upload_param_id,param_name,type,set_value,upper_limit_value,lower_limit_value,limit_control,change_monitor,actual_value,bycell_output,param_value_rate from device_inputoutput "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.device_inputoutput model = new SUNWODA_SEVB.Data.Model.device_inputoutput();
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
        public SUNWODA_SEVB.Data.Model.device_inputoutput DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_inputoutput model = new SUNWODA_SEVB.Data.Model.device_inputoutput();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["equipment_id"] != null)
                {
                    model.equipment_id = row["equipment_id"].ToString()!;
                }
                if (row["send_param_id"] != null)
                {
                    model.send_param_id = row["send_param_id"].ToString()!;
                }
                if (row["upload_param_id"] != null)
                {
                    model.upload_param_id = row["upload_param_id"].ToString()!;
                }
                if (row["param_name"] != null)
                {
                    model.param_name = row["param_name"].ToString()!;
                }
                if (row["type"] != null)
                {
                    model.type = row["type"].ToString()!;
                }
                if (row["set_value"] != null)
                {
                    model.set_value = row["set_value"].ToString()!;
                }
                if (row["upper_limit_value"] != null)
                {
                    model.upper_limit_value = row["upper_limit_value"].ToString()!;
                }
                if (row["lower_limit_value"] != null)
                {
                    model.lower_limit_value = row["lower_limit_value"].ToString()!;
                }
                if (row["limit_control"] != null)
                {
                    model.limit_control = row["limit_control"].ToString()!;
                }
                if (row["change_monitor"] != null)
                {
                    model.change_monitor = row["change_monitor"].ToString()!;
                }
                if (row["actual_value"] != null)
                {
                    model.actual_value = row["actual_value"].ToString()!;
                }
                if (row["bycell_output"] != null)
                {
                    model.bycell_output = row["bycell_output"].ToString()!;
                }
                if (row["param_value_rate"] != null && row["param_value_rate"].ToString() != "")
                {
                    model.param_value_rate = decimal.Parse(row["param_value_rate"].ToString()!);
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
                "select id,equipment_id,send_param_id,upload_param_id,param_name,type,set_value,upper_limit_value,lower_limit_value,limit_control,change_monitor,actual_value,bycell_output,param_value_rate "
            );
            strSql.Append(" FROM device_inputoutput ");
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
            strSql.Append("select count(1) FROM device_inputoutput ");
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
            strSql.Append(")AS Row, T.*  from device_inputoutput T ");
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
            parameters[0].Value = "device_inputoutput";
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
