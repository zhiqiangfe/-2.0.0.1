using System;
using System.Data;
using System.Text;
using Maticsoft.DBUtility; //Please add references
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device_alert_config
    /// </summary>
    public partial class device_alert_config
    {
        public device_alert_config() { }

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string equipment_id, string upload_param_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_alert_config");
            strSql.Append(
                " where equipment_id=@equipment_id and upload_param_id=@upload_param_id "
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = equipment_id;
            parameters[1].Value = upload_param_id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_alert_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_alert_config(");
            strSql.Append(
                "equipment_id,plc_config_id,upload_param_id,param_name,alert_level,alert_address,data_time)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@equipment_id,@plc_config_id,@upload_param_id,@param_name,@alert_level,@alert_address,@data_time)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alert_level", MySqlDbType.VarChar, 20),
                new MySqlParameter("@alert_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@data_time", MySqlDbType.DateTime),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.plc_config_id;
            parameters[2].Value = model.upload_param_id;
            parameters[3].Value = model.param_name;
            parameters[4].Value = model.alert_level;
            parameters[5].Value = model.alert_address;
            parameters[6].Value = model.data_time;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_alert_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_alert_config set ");
            strSql.Append("plc_config_id=@plc_config_id,");
            strSql.Append("param_name=@param_name,");
            strSql.Append("alert_level=@alert_level,");
            strSql.Append("alert_address=@alert_address,");
            strSql.Append("data_time=@data_time");
            strSql.Append(
                " where equipment_id=@equipment_id and upload_param_id=@upload_param_id "
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@param_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alert_level", MySqlDbType.VarChar, 20),
                new MySqlParameter("@alert_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@data_time", MySqlDbType.DateTime),
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.plc_config_id;
            parameters[1].Value = model.param_name;
            parameters[2].Value = model.alert_level;
            parameters[3].Value = model.alert_address;
            parameters[4].Value = model.data_time;
            parameters[5].Value = model.equipment_id;
            parameters[6].Value = model.upload_param_id;

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
        public bool Delete(string equipment_id, string upload_param_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from device_alert_config ");
            strSql.Append(
                " where equipment_id=@equipment_id and upload_param_id=@upload_param_id "
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = equipment_id;
            parameters[1].Value = upload_param_id;

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
        public SUNWODA_SEVB.Data.Model.device_alert_config? GetModel(
            string equipment_id,
            string upload_param_id
        )
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select equipment_id,plc_config_id,upload_param_id,param_name,alert_level,alert_address,data_time from device_alert_config,id "
            );
            strSql.Append(
                " where equipment_id=@equipment_id and upload_param_id=@upload_param_id "
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = equipment_id;
            parameters[1].Value = upload_param_id;

            SUNWODA_SEVB.Data.Model.device_alert_config model = new SUNWODA_SEVB.Data.Model.device_alert_config();
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
        public SUNWODA_SEVB.Data.Model.device_alert_config DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_alert_config model = new SUNWODA_SEVB.Data.Model.device_alert_config();
            if (row != null)
            {
                if (row["id"] != null)
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
                if (row["upload_param_id"] != null)
                {
                    model.upload_param_id = row["upload_param_id"].ToString()!;
                }
                if (row["param_name"] != null)
                {
                    model.param_name = row["param_name"].ToString()!;
                }
                if (row["alert_level"] != null)
                {
                    model.alert_level = row["alert_level"].ToString()!;
                }
                if (row["alert_address"] != null)
                {
                    model.alert_address = row["alert_address"].ToString()!;
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
            strSql.Append(
                "select equipment_id,plc_config_id,upload_param_id,param_name,alert_level,alert_address,data_time,plc_rw_config_id,id "
            );
            strSql.Append(" FROM device_alert_config ");
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
            strSql.Append("select count(1) FROM device_alert_config ");
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
                strSql.Append("order by T.upload_param_id desc");
            }
            strSql.Append(")AS Row, T.*  from device_alert_config T ");
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
            parameters[0].Value = "device_alert_config";
            parameters[1].Value = "upload_param_id";
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
