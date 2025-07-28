using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:alarm_rule
    /// </summary>
    public partial class alarm_rule
    {
        public alarm_rule() { }

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string equipment_id, string upload_param_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from alarm_rule");
            strSql.Append(
                " where equipment_id=@equipment_id and upload_param_id=@upload_param_id "
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 20),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = equipment_id;
            parameters[1].Value = upload_param_id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.alarm_rule model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into alarm_rule(");
            strSql.Append(
                "equipment_id,upload_param_id,alarm_level_id,alarm_content,plc_rw_config_id,plc_address_int,plc_address_bit)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@equipment_id,@upload_param_id,@alarm_level_id,@alarm_content,@plc_rw_config_id,@plc_address_int,@plc_address_bit)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 20),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarm_level_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@alarm_content", MySqlDbType.VarChar, 255),
                new MySqlParameter("@plc_rw_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_address_int", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_address_bit", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.upload_param_id;
            parameters[2].Value = model.alarm_level_id;
            parameters[3].Value = model.alarm_content;
            parameters[4].Value = model.plc_rw_config_id;
            parameters[5].Value = model.plc_address_int;
            parameters[6].Value = model.plc_address_bit;

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
        public bool Update(SUNWODA_SEVB.Data.Model.alarm_rule model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update alarm_rule set ");
            strSql.Append("alarm_level_id=@alarm_level_id,");
            strSql.Append("alarm_content=@alarm_content,");
            strSql.Append("plc_rw_config_id=@plc_rw_config_id,");
            strSql.Append("plc_address_int=@plc_address_int,");
            strSql.Append("plc_address_bit=@plc_address_bit");
            strSql.Append(
                " where equipment_id=@equipment_id and upload_param_id=@upload_param_id "
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@alarm_level_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@alarm_content", MySqlDbType.VarChar, 255),
                new MySqlParameter("@plc_rw_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_address_int", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_address_bit", MySqlDbType.Int32, 11),
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 20),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.alarm_level_id;
            parameters[1].Value = model.alarm_content;
            parameters[2].Value = model.plc_rw_config_id;
            parameters[3].Value = model.plc_address_int;
            parameters[4].Value = model.plc_address_bit;
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
            strSql.Append("delete from alarm_rule ");
            strSql.Append(
                " where equipment_id=@equipment_id and upload_param_id=@upload_param_id "
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 20),
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
        public SUNWODA_SEVB.Data.Model.alarm_rule? GetModel(string equipment_id, string upload_param_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select equipment_id,upload_param_id,alarm_level_id,alarm_content,plc_rw_config_id,plc_address_int,plc_address_bit from alarm_rule "
            );
            strSql.Append(
                " where equipment_id=@equipment_id and upload_param_id=@upload_param_id "
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 20),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = equipment_id;
            parameters[1].Value = upload_param_id;

            SUNWODA_SEVB.Data.Model.alarm_rule model = new SUNWODA_SEVB.Data.Model.alarm_rule();
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
        public SUNWODA_SEVB.Data.Model.alarm_rule DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.alarm_rule model = new SUNWODA_SEVB.Data.Model.alarm_rule();
            if (row != null)
            {
                if (row["equipment_id"] != null)
                {
                    model.equipment_id = row["equipment_id"].ToString()!;
                }
                if (row["upload_param_id"] != null)
                {
                    model.upload_param_id = row["upload_param_id"].ToString()!;
                }
                if (row["alarm_level_id"] != null && row["alarm_level_id"].ToString() != "")
                {
                    model.alarm_level_id = int.Parse(row["alarm_level_id"].ToString()!);
                }
                if (row["alarm_content"] != null)
                {
                    model.alarm_content = row["alarm_content"].ToString()!;
                }
                if (row["plc_rw_config_id"] != null && row["plc_rw_config_id"].ToString() != "")
                {
                    model.plc_rw_config_id = int.Parse(row["plc_rw_config_id"].ToString()!);
                }
                if (row["plc_address_int"] != null && row["plc_address_int"].ToString() != "")
                {
                    model.plc_address_int = int.Parse(row["plc_address_int"].ToString()!);
                }
                if (row["plc_address_bit"] != null && row["plc_address_bit"].ToString() != "")
                {
                    model.plc_address_bit = int.Parse(row["plc_address_bit"].ToString()!);
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
                "select equipment_id,upload_param_id,alarm_level_id,alarm_content,plc_rw_config_id,plc_address_int,plc_address_bit "
            );
            strSql.Append(" FROM alarm_rule ");
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
            strSql.Append("select count(1) FROM alarm_rule ");
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
            strSql.Append(")AS Row, T.*  from alarm_rule T ");
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
            parameters[0].Value = "alarm_rule";
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
