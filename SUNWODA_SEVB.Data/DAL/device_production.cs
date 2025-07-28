using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device
    /// </summary>
    public partial class device_production
    {
        public device_production() { }

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_production");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.VarChar, 50) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_production model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_production(");
            strSql.Append("id,plc_config_id,name,enabled,group_code,group_alias,remark)");
            strSql.Append(" values (");
            strSql.Append("@id,@plc_config_id,@name,@enabled,@group_code,@group_alias,@remark)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@group_code", MySqlDbType.VarChar, 50),
                new MySqlParameter("@group_alias", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.plc_config_id;
            parameters[2].Value = model.name;
            parameters[3].Value = model.enabled;
            parameters[4].Value = model.group_code;
            parameters[5].Value = model.group_alias;
            parameters[6].Value = model.remark;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_production model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_production set ");
            strSql.Append("plc_config_id=@plc_config_id,");
            strSql.Append("name=@name,");
            strSql.Append("enabled=@enabled,");
            strSql.Append("group_code=@group_code,");
            strSql.Append("group_alias=@group_alias,");
            strSql.Append("remark=@remark");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@group_code", MySqlDbType.VarChar, 50),
                new MySqlParameter("@group_alias", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.plc_config_id;
            parameters[1].Value = model.name;
            parameters[2].Value = model.enabled;
            parameters[3].Value = model.group_code;
            parameters[4].Value = model.group_alias;
            parameters[5].Value = model.remark;
            parameters[6].Value = model.id;

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
            strSql.Append("delete from device_production ");
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
            strSql.Append("delete from device_production ");
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
        public SUNWODA_SEVB.Data.Model.device_production? GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,plc_config_id,name,enabled,group_code,group_alias,remark from device_production "
            );
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.VarChar, 50) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.device_production model = new SUNWODA_SEVB.Data.Model.device_production();
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
        public SUNWODA_SEVB.Data.Model.device_production DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_production model = new SUNWODA_SEVB.Data.Model.device_production();
            if (row != null)
            {
                if (row["id"] != null)
                {
                    model.id = row["id"].ToString()!;
                }
                if (row["plc_config_id"] != null && row["plc_config_id"].ToString() != "")
                {
                    model.plc_config_id = int.Parse(row["plc_config_id"].ToString()!);
                }
                if (row["name"] != null)
                {
                    model.name = row["name"].ToString()!;
                }
                if (row["enabled"] != null && row["enabled"].ToString() != "")
                {
                    model.enabled = int.Parse(row["enabled"].ToString()!);
                }
                if (row["group_code"] != null)
                {
                    model.group_code = row["group_code"].ToString()!;
                }
                if (row["group_alias"] != null)
                {
                    model.group_alias = row["group_alias"].ToString()!;
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString()!;
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
            strSql.Append("select id,plc_config_id,name,enabled,group_code,group_alias,remark ");
            strSql.Append(" FROM device_production ");
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
            strSql.Append("select count(1) FROM device_production ");
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
            strSql.Append(")AS Row, T.*  from device_production T ");
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
            parameters[0].Value = "device";
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
