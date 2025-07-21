using System;
using System.Data;
using System.Text;
using Maticsoft.DBUtility; //Please add references
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device_alarm
    /// </summary>
    public partial class device_alarm
    {
        public device_alarm() { }

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_alarm");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_alarm model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_alarm(");
            strSql.Append(
                "alarmcode_ime,alarmcode_vendor,groupremark,alarmclassify,alarmdescription,alarmlevel,alarmpart,collectionremark,value_type,variable_length,plc_rw_config_id,plc_address,description,is_monitor,enabled,remark,datatime)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@alarmcode_ime,@alarmcode_vendor,@groupremark,@alarmclassify,@alarmdescription,@alarmlevel,@alarmpart,@collectionremark,@value_type,@variable_length,@plc_rw_config_id,@plc_address,@description,@is_monitor,@enabled,@remark,@datatime)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@alarmcode_ime", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarmcode_vendor", MySqlDbType.VarChar, 50),
                new MySqlParameter("@groupremark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarmclassify", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarmdescription", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarmlevel", MySqlDbType.Int32, 11),
                new MySqlParameter("@alarmpart", MySqlDbType.VarChar, 50),
                new MySqlParameter("@collectionremark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@variable_length", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_rw_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@description", MySqlDbType.VarChar, 200),
                new MySqlParameter("@is_monitor", MySqlDbType.Bit),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@datatime", MySqlDbType.DateTime),
            };
            parameters[0].Value = model.alarmcode_ime;
            parameters[1].Value = model.alarmcode_vendor;
            parameters[2].Value = model.groupremark;
            parameters[3].Value = model.alarmclassify;
            parameters[4].Value = model.alarmdescription;
            parameters[5].Value = model.alarmlevel;
            parameters[6].Value = model.alarmpart;
            parameters[7].Value = model.collectionremark;
            parameters[8].Value = model.value_type;
            parameters[9].Value = model.variable_length;
            parameters[10].Value = model.plc_rw_config_id;
            parameters[11].Value = model.plc_address;
            parameters[12].Value = model.description;
            parameters[13].Value = model.is_monitor;
            parameters[14].Value = model.enabled;
            parameters[15].Value = model.remark;
            parameters[16].Value = model.datatime;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_alarm model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_alarm set ");
            strSql.Append("alarmcode_ime=@alarmcode_ime,");
            strSql.Append("alarmcode_vendor=@alarmcode_vendor,");
            strSql.Append("groupremark=@groupremark,");
            strSql.Append("alarmclassify=@alarmclassify,");
            strSql.Append("alarmdescription=@alarmdescription,");
            strSql.Append("alarmlevel=@alarmlevel,");
            strSql.Append("alarmpart=@alarmpart,");
            strSql.Append("collectionremark=@collectionremark,");
            strSql.Append("value_type=@value_type,");
            strSql.Append("variable_length=@variable_length,");
            strSql.Append("plc_rw_config_id=@plc_rw_config_id,");
            strSql.Append("plc_address=@plc_address,");
            strSql.Append("description=@description,");
            strSql.Append("enabled=@enabled,");
            strSql.Append("remark=@remark ");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
                new MySqlParameter("@alarmcode_ime", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarmcode_vendor", MySqlDbType.VarChar, 50),
                new MySqlParameter("@groupremark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarmclassify", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarmdescription", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarmlevel", MySqlDbType.Int32, 11),
                new MySqlParameter("@alarmpart", MySqlDbType.VarChar, 50),
                new MySqlParameter("@collectionremark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@variable_length", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_rw_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@description", MySqlDbType.VarChar, 200),
                new MySqlParameter("@is_monitor", MySqlDbType.Bit),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
            };
            parameters[1].Value = model.alarmcode_ime;
            parameters[2].Value = model.alarmcode_vendor;
            parameters[3].Value = model.groupremark;
            parameters[4].Value = model.alarmclassify;
            parameters[5].Value = model.alarmdescription;
            parameters[6].Value = model.alarmlevel;
            parameters[7].Value = model.alarmpart;
            parameters[8].Value = model.collectionremark;
            parameters[9].Value = model.value_type;
            parameters[10].Value = model.variable_length;
            parameters[11].Value = model.plc_rw_config_id;
            parameters[12].Value = model.plc_address;
            parameters[13].Value = model.description;
            parameters[14].Value = model.is_monitor;
            parameters[15].Value = model.enabled;
            parameters[16].Value = model.remark;
            parameters[0].Value = model.id;

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
            strSql.Append("delete from device_alarm ");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32, 11) };
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
            strSql.Append("delete from device_alarm ");
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
        public SUNWODA_SEVB.Data.Model.device_alarm? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,alarmcode_ime,alarmcode_vendor,groupremark,alarmclassify,alarmdescription,alarmlevel,alarmpart,collectionremark,value_type,variable_length,plc_rw_config_id,plc_address,description,is_monitor,enabled,remark,datatime from device_alarm "
            );
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32, 11) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.device_alarm model = new SUNWODA_SEVB.Data.Model.device_alarm();
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
        public SUNWODA_SEVB.Data.Model.device_alarm DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_alarm model = new SUNWODA_SEVB.Data.Model.device_alarm();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["alarmcode_ime"] != null)
                {
                    model.alarmcode_ime = row["alarmcode_ime"].ToString()!;
                }
                if (row["alarmcode_vendor"] != null)
                {
                    model.alarmcode_vendor = row["alarmcode_vendor"].ToString()!;
                }
                if (row["groupremark"] != null)
                {
                    model.groupremark = row["groupremark"].ToString()!;
                }
                if (row["alarmclassify"] != null)
                {
                    model.alarmclassify = row["alarmclassify"].ToString()!;
                }
                if (row["alarmdescription"] != null)
                {
                    model.alarmdescription = row["alarmdescription"].ToString()!;
                }
                if (row["alarmlevel"] != null && row["alarmlevel"].ToString() != "")
                {
                    model.alarmlevel = int.Parse(row["alarmlevel"].ToString()!);
                }
                if (row["alarmpart"] != null)
                {
                    model.alarmpart = row["alarmpart"].ToString()!;
                }
                if (row["collectionremark"] != null)
                {
                    model.collectionremark = row["collectionremark"].ToString()!;
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
                if (row["description"] != null)
                {
                    model.description = row["description"].ToString()!;
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
                if (row["enabled"] != null && row["enabled"].ToString() != "")
                {
                    model.enabled = int.Parse(row["enabled"].ToString()!);
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
                "select id,alarmcode_ime,alarmcode_vendor,groupremark,alarmclassify,alarmdescription,alarmlevel,alarmpart,collectionremark,value_type,variable_length,plc_rw_config_id,plc_address,description,is_monitor,enabled,remark,datatime "
            );
            strSql.Append(" FROM device_alarm ");
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
            strSql.Append("select count(1) FROM device_alarm ");
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
            strSql.Append(")AS Row, T.*  from device_alarm T ");
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
            parameters[0].Value = "device_alarm";
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
