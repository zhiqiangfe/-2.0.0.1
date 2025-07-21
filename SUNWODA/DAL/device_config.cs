using System;
using System.Data;
using System.Text;
using Maticsoft.DBUtility;
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device_config
    /// </summary>
    public partial class device_config
    {
        public device_config() { }

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_config");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.VarChar, 50) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_config(");
            strSql.Append(
                "id,devicename,labchinese,labenglish,workship,prefixofvariables,plc_config_id,group_devicealarm,description,enabled,remark,datatime)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@id,@devicename,@labchinese,@labenglish,@workship,@prefixofvariables,@plc_config_id,@group_devicealarm,@description,@enabled,@remark,@datatime)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@devicename", MySqlDbType.VarChar, 50),
                new MySqlParameter("@labchinese", MySqlDbType.VarChar, 50),
                new MySqlParameter("@labenglish", MySqlDbType.VarChar, 50),
                new MySqlParameter("@workship", MySqlDbType.VarChar, 50),
                new MySqlParameter("@prefixofvariables", MySqlDbType.VarChar, 50),
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@group_devicealarm", MySqlDbType.VarChar, 50),
                new MySqlParameter("@description", MySqlDbType.VarChar, 200),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@datatime", MySqlDbType.DateTime),
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.devicename;
            parameters[2].Value = model.labchinese;
            parameters[3].Value = model.labenglish;
            parameters[4].Value = model.workship;
            parameters[5].Value = model.prefixofvariables;
            parameters[6].Value = model.plc_config_id;
            parameters[7].Value = model.group_devicealarm;
            parameters[8].Value = model.description;
            parameters[9].Value = model.enabled;
            parameters[10].Value = model.remark;
            parameters[11].Value = model.datatime;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_config set ");
            strSql.Append("devicename=@devicename,");
            strSql.Append("labchinese=@labchinese,");
            strSql.Append("labenglish=@labenglish,");
            strSql.Append("workship=@workship,");
            strSql.Append("prefixofvariables=@prefixofvariables,");
            strSql.Append("plc_config_id=@plc_config_id,");
            strSql.Append("group_devicealarm=@group_devicealarm,");
            strSql.Append("description=@description,");
            strSql.Append("enabled=@enabled,");
            strSql.Append("remark=@remark,");
            strSql.Append("datatime=@datatime");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@devicename", MySqlDbType.VarChar, 50),
                new MySqlParameter("@labchinese", MySqlDbType.VarChar, 50),
                new MySqlParameter("@labenglish", MySqlDbType.VarChar, 50),
                new MySqlParameter("@workship", MySqlDbType.VarChar, 50),
                new MySqlParameter("@prefixofvariables", MySqlDbType.VarChar, 50),
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@group_devicealarm", MySqlDbType.VarChar, 50),
                new MySqlParameter("@description", MySqlDbType.VarChar, 200),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@datatime", MySqlDbType.DateTime),
                new MySqlParameter("@id", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.devicename;
            parameters[1].Value = model.labchinese;
            parameters[2].Value = model.labenglish;
            parameters[3].Value = model.workship;
            parameters[4].Value = model.prefixofvariables;
            parameters[5].Value = model.plc_config_id;
            parameters[6].Value = model.group_devicealarm;
            parameters[7].Value = model.description;
            parameters[8].Value = model.enabled;
            parameters[9].Value = model.remark;
            parameters[10].Value = model.datatime;
            parameters[11].Value = model.id;

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
        public bool Update(string oldId, SUNWODA_SEVB.Data.Model.device_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_config set ");
            strSql.Append("devicename=@devicename,");
            strSql.Append("labchinese=@labchinese,");
            strSql.Append("labenglish=@labenglish,");
            strSql.Append("workship=@workship,");
            strSql.Append("prefixofvariables=@prefixofvariables,");
            strSql.Append("plc_config_id=@plc_config_id,");
            strSql.Append("group_devicealarm=@group_devicealarm,");
            strSql.Append("description=@description,");
            strSql.Append("enabled=@enabled,");
            strSql.Append("remark=@remark,");
            strSql.Append("datatime=@datatime,");
            strSql.Append("id=@id");
            strSql.Append(" where id=@oldId");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@devicename", MySqlDbType.VarChar, 50),
                new MySqlParameter("@labchinese", MySqlDbType.VarChar, 50),
                new MySqlParameter("@labenglish", MySqlDbType.VarChar, 50),
                new MySqlParameter("@workship", MySqlDbType.VarChar, 50),
                new MySqlParameter("@prefixofvariables", MySqlDbType.VarChar, 50),
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@group_devicealarm", MySqlDbType.VarChar, 50),
                new MySqlParameter("@description", MySqlDbType.VarChar, 200),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@datatime", MySqlDbType.DateTime),
                new MySqlParameter("@id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@oldId", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.devicename;
            parameters[1].Value = model.labchinese;
            parameters[2].Value = model.labenglish;
            parameters[3].Value = model.workship;
            parameters[4].Value = model.prefixofvariables;
            parameters[5].Value = model.plc_config_id;
            parameters[6].Value = model.group_devicealarm;
            parameters[7].Value = model.description;
            parameters[8].Value = model.enabled;
            parameters[9].Value = model.remark;
            parameters[10].Value = model.datatime;
            parameters[11].Value = model.id;
            parameters[12].Value = oldId;

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
        public bool Delete(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from device_config ");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.VarChar, 50) };
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
            strSql.Append("delete from device_config ");
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
        public SUNWODA_SEVB.Data.Model.device_config? GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,devicename,labchinese,labenglish,workship,prefixofvariables,plc_config_id,group_devicealarm,description,enabled,remark,datatime from device_config "
            );
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.VarChar, 50) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.device_config model = new SUNWODA_SEVB.Data.Model.device_config();
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
        public SUNWODA_SEVB.Data.Model.device_config DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_config model = new SUNWODA_SEVB.Data.Model.device_config();
            if (row != null)
            {
                if (row["id"] != null)
                {
                    model.id = row["id"].ToString()!;
                }
                if (row["devicename"] != null)
                {
                    model.devicename = row["devicename"].ToString()!;
                }
                if (row["labchinese"] != null)
                {
                    model.labchinese = row["labchinese"].ToString()!;
                }
                if (row["labenglish"] != null)
                {
                    model.labenglish = row["labenglish"].ToString()!;
                }
                if (row["workship"] != null)
                {
                    model.workship = row["workship"].ToString()!;
                }
                if (row["prefixofvariables"] != null)
                {
                    model.prefixofvariables = row["prefixofvariables"].ToString()!;
                }
                if (row["plc_config_id"] != null && row["plc_config_id"].ToString() != "")
                {
                    model.plc_config_id = int.Parse(row["plc_config_id"].ToString()!);
                }
                if (row["group_devicealarm"] != null)
                {
                    model.group_devicealarm = row["group_devicealarm"].ToString()!;
                }
                if (row["description"] != null)
                {
                    model.description = row["description"].ToString()!;
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
                "select id,devicename,labchinese,labenglish,workship,prefixofvariables,plc_config_id,group_devicealarm,description,enabled,remark,datatime "
            );
            strSql.Append(" FROM device_config ");
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
            strSql.Append("select count(1) FROM device_config ");
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
            strSql.Append(")AS Row, T.*  from device_config T ");
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
            parameters[0].Value = "device_config";
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
