using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device_inputoutput_config
    /// </summary>
    public partial class device_inputoutput_config
    {
        public device_inputoutput_config() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "device_inputoutput_config");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_inputoutput_config");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_inputoutput_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_inputoutput_config(");
            strSql.Append(
                "equipment_id,plc_config_id,send_param_id,upload_param_id,param_name,type,set_value_address,upper_limit_value_address,lower_limit_value_address,limit_control,change_monitor_address,actual_value_address,bycell_output_address,param_value_rate,param_unit)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@equipment_id,@plc_config_id,@send_param_id,@upload_param_id,@param_name,@type,@set_value_address,@upper_limit_value_address,@lower_limit_value_address,@limit_control,@change_monitor_address,@actual_value_address,@bycell_output_address,@param_value_rate,@param_unit)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@send_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@type", MySqlDbType.VarChar, 20),
                new MySqlParameter("@set_value_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upper_limit_value_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@lower_limit_value_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@limit_control", MySqlDbType.VarChar, 50),
                new MySqlParameter("@change_monitor_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@actual_value_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@bycell_output_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_value_rate", MySqlDbType.Float),
                new MySqlParameter("@param_unit", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.plc_config_id;
            parameters[2].Value = model.send_param_id;
            parameters[3].Value = model.upload_param_id;
            parameters[4].Value = model.param_name;
            parameters[5].Value = model.type;
            parameters[6].Value = model.set_value_address;
            parameters[7].Value = model.upper_limit_value_address;
            parameters[8].Value = model.lower_limit_value_address;
            parameters[9].Value = model.limit_control;
            parameters[10].Value = model.change_monitor_address;
            parameters[11].Value = model.actual_value_address;
            parameters[12].Value = model.bycell_output_address;
            parameters[13].Value = model.param_value_rate;
            parameters[14].Value = model.param_unit;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_inputoutput_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_inputoutput_config set ");
            strSql.Append("equipment_id=@equipment_id,");
            strSql.Append("plc_config_id=@plc_config_id,");
            strSql.Append("send_param_id=@send_param_id,");
            strSql.Append("upload_param_id=@upload_param_id,");
            strSql.Append("param_name=@param_name,");
            strSql.Append("type=@type,");
            strSql.Append("set_value_address=@set_value_address,");
            strSql.Append("upper_limit_value_address=@upper_limit_value_address,");
            strSql.Append("lower_limit_value_address=@lower_limit_value_address,");
            strSql.Append("limit_control=@limit_control,");
            strSql.Append("change_monitor_address=@change_monitor_address,");
            strSql.Append("actual_value_address=@actual_value_address,");
            strSql.Append("bycell_output_address=@bycell_output_address,");
            strSql.Append("param_value_rate=@param_value_rate,");
            strSql.Append("param_unit=@param_unit");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@send_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@type", MySqlDbType.VarChar, 20),
                new MySqlParameter("@set_value_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upper_limit_value_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@lower_limit_value_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@limit_control", MySqlDbType.VarChar, 50),
                new MySqlParameter("@change_monitor_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@actual_value_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@bycell_output_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_value_rate", MySqlDbType.Float),
                new MySqlParameter("@param_unit", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.plc_config_id;
            parameters[2].Value = model.send_param_id;
            parameters[3].Value = model.upload_param_id;
            parameters[4].Value = model.param_name;
            parameters[5].Value = model.type;
            parameters[6].Value = model.set_value_address;
            parameters[7].Value = model.upper_limit_value_address;
            parameters[8].Value = model.lower_limit_value_address;
            parameters[9].Value = model.limit_control;
            parameters[10].Value = model.change_monitor_address;
            parameters[11].Value = model.actual_value_address;
            parameters[12].Value = model.bycell_output_address;
            parameters[13].Value = model.param_value_rate;
            parameters[14].Value = model.param_unit;
            parameters[15].Value = model.id;

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
            strSql.Append("delete from device_inputoutput_config ");
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
            strSql.Append("delete from device_inputoutput_config ");
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
        public SUNWODA_SEVB.Data.Model.device_inputoutput_config? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,equipment_id,plc_config_id,send_param_id,upload_param_id,param_name,type,set_value_address,upper_limit_value_address,lower_limit_value_address,limit_control,change_monitor_address,actual_value_address,bycell_output_address,param_value_rate,param_unit from device_inputoutput_config "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.device_inputoutput_config model =
                new SUNWODA_SEVB.Data.Model.device_inputoutput_config();
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
        public SUNWODA_SEVB.Data.Model.device_inputoutput_config DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_inputoutput_config model =
                new SUNWODA_SEVB.Data.Model.device_inputoutput_config();
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
                if (row["plc_config_id"] != null && row["plc_config_id"].ToString() != "")
                {
                    model.plc_config_id = int.Parse(row["plc_config_id"].ToString()!);
                }
                if (row["plc_rw_config_id"] != null && row["plc_rw_config_id"].ToString() != "")
                {
                    model.plc_rw_config_id = int.Parse(row["plc_rw_config_id"].ToString()!);
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
                if (row["set_value_address"] != null)
                {
                    model.set_value_address = row["set_value_address"].ToString()!;
                }
                if (row["upper_limit_value_address"] != null)
                {
                    model.upper_limit_value_address = row["upper_limit_value_address"].ToString()!;
                }
                if (row["lower_limit_value_address"] != null)
                {
                    model.lower_limit_value_address = row["lower_limit_value_address"].ToString()!;
                }
                if (row["limit_control"] != null)
                {
                    model.limit_control = row["limit_control"].ToString()!;
                }
                if (row["change_monitor_address"] != null)
                {
                    model.change_monitor_address = row["change_monitor_address"].ToString()!;
                }
                if (row["actual_value_address"] != null)
                {
                    model.actual_value_address = row["actual_value_address"].ToString()!;
                }
                if (row["bycell_output_address"] != null)
                {
                    model.bycell_output_address = row["bycell_output_address"].ToString()!;
                }
                if (row["param_value_rate"] != null && row["param_value_rate"].ToString() != "")
                {
                    model.param_value_rate = decimal.Parse(row["param_value_rate"].ToString()!);
                }
                if (row["param_unit"] != null)
                {
                    model.param_unit = row["param_unit"].ToString()!;
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
                "select id,equipment_id,plc_config_id,send_param_id,upload_param_id,param_name,type,set_value_address,upper_limit_value_address,lower_limit_value_address,limit_control,change_monitor_address,actual_value_address,bycell_output_address,param_value_rate,param_unit,plc_rw_config_id "
            );
            strSql.Append(" FROM device_inputoutput_config ");
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
            strSql.Append("select count(1) FROM device_inputoutput_config ");
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
            strSql.Append(")AS Row, T.*  from device_inputoutput_config T ");
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
            parameters[0].Value = "device_inputoutput_config";
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
