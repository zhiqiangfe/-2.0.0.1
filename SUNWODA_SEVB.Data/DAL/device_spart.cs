using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device_spart
    /// </summary>
    public partial class device_spart
    {
        public device_spart() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "device_spart");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_spart");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_spart model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_spart(");
            strSql.Append(
                "equipment_id,upload_param_id,param_name,type,spart_expected_life,mes_download_used_life,used_life,limit_control,percent_warning,param_value_rate,change_date,change_user)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@equipment_id,@upload_param_id,@param_name,@type,@spart_expected_life,@mes_download_used_life,@used_life,@limit_control,@percent_warning,@param_value_rate,@change_date,@change_user)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@type", MySqlDbType.VarChar, 20),
                new MySqlParameter("@spart_expected_life", MySqlDbType.Int32, 11),
                new MySqlParameter("@mes_download_used_life", MySqlDbType.Float),
                new MySqlParameter("@used_life", MySqlDbType.Float),
                new MySqlParameter("@limit_control", MySqlDbType.VarChar, 50),
                new MySqlParameter("@percent_warning", MySqlDbType.Float, 11),
                new MySqlParameter("@param_value_rate", MySqlDbType.Float),
                new MySqlParameter("@change_date", MySqlDbType.DateTime),
                new MySqlParameter("@change_user", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.upload_param_id;
            parameters[2].Value = model.param_name;
            parameters[3].Value = model.type;
            parameters[4].Value = model.spart_expected_life;
            parameters[5].Value = model.mes_download_used_life;
            parameters[6].Value = model.used_life;
            parameters[7].Value = model.limit_control;
            parameters[8].Value = model.percent_warning;
            parameters[9].Value = model.param_value_rate;
            parameters[10].Value = model.change_date;
            parameters[11].Value = model.change_user;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_spart model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_spart set ");
            strSql.Append("equipment_id=@equipment_id,");
            strSql.Append("upload_param_id=@upload_param_id,");
            strSql.Append("param_name=@param_name,");
            strSql.Append("type=@type,");
            strSql.Append("spart_expected_life=@spart_expected_life,");
            strSql.Append("mes_download_used_life=@mes_download_used_life,");
            strSql.Append("used_life=@used_life,");
            strSql.Append("limit_control=@limit_control,");
            strSql.Append("percent_warning=@percent_warning,");
            strSql.Append("param_value_rate=@param_value_rate,");
            strSql.Append("change_date=@change_date,");
            strSql.Append("change_user=@change_user");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@param_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@type", MySqlDbType.VarChar, 20),
                new MySqlParameter("@spart_expected_life", MySqlDbType.Int32, 11),
                new MySqlParameter("@mes_download_used_life", MySqlDbType.Float),
                new MySqlParameter("@used_life", MySqlDbType.Float),
                new MySqlParameter("@limit_control", MySqlDbType.VarChar, 50),
                new MySqlParameter("@percent_warning", MySqlDbType.Float, 11),
                new MySqlParameter("@param_value_rate", MySqlDbType.Float),
                new MySqlParameter("@change_date", MySqlDbType.DateTime),
                new MySqlParameter("@change_user", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.upload_param_id;
            parameters[2].Value = model.param_name;
            parameters[3].Value = model.type;
            parameters[4].Value = model.spart_expected_life;
            parameters[5].Value = model.mes_download_used_life;
            parameters[6].Value = model.used_life;
            parameters[7].Value = model.limit_control;
            parameters[8].Value = model.percent_warning;
            parameters[9].Value = model.param_value_rate;
            parameters[10].Value = model.change_date;
            parameters[11].Value = model.change_user;
            parameters[12].Value = model.id;

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
            strSql.Append("delete from device_spart ");
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
            strSql.Append("delete from device_spart ");
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
        public SUNWODA_SEVB.Data.Model.device_spart? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,equipment_id,upload_param_id,param_name,type,spart_expected_life,mes_download_used_life,used_life,limit_control,percent_warning,param_value_rate,change_date,change_user from device_spart "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.device_spart model = new SUNWODA_SEVB.Data.Model.device_spart();
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
        public SUNWODA_SEVB.Data.Model.device_spart DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_spart model = new SUNWODA_SEVB.Data.Model.device_spart();
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
                if (
                    row["spart_expected_life"] != null
                    && row["spart_expected_life"].ToString() != ""
                )
                {
                    model.spart_expected_life = int.Parse(row["spart_expected_life"].ToString()!);
                }
                if (
                    row["mes_download_used_life"] != null
                    && row["mes_download_used_life"].ToString() != ""
                )
                {
                    model.mes_download_used_life = decimal.Parse(
                        row["mes_download_used_life"].ToString()!
                    );
                }
                if (row["used_life"] != null && row["used_life"].ToString() != "")
                {
                    model.used_life = decimal.Parse(row["used_life"].ToString()!);
                }
                if (row["limit_control"] != null)
                {
                    model.limit_control = row["limit_control"].ToString()!;
                }
                if (row["percent_warning"] != null && row["percent_warning"].ToString() != "")
                {
                    model.percent_warning = decimal.Parse(row["percent_warning"].ToString()!);
                }
                if (row["param_value_rate"] != null && row["param_value_rate"].ToString() != "")
                {
                    model.param_value_rate = decimal.Parse(row["param_value_rate"].ToString()!);
                }
                if (row["change_date"] != null && row["change_date"].ToString() != "")
                {
                    model.change_date = DateTime.Parse(row["change_date"].ToString()!);
                }
                if (row["change_user"] != null)
                {
                    model.change_user = row["change_user"].ToString()!;
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
                "select id,equipment_id,upload_param_id,param_name,type,spart_expected_life,mes_download_used_life,used_life,limit_control,percent_warning,param_value_rate,change_date,change_user "
            );
            strSql.Append(" FROM device_spart ");
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
            strSql.Append("select count(1) FROM device_spart ");
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
            strSql.Append(")AS Row, T.*  from device_spart T ");
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
            parameters[0].Value = "device_spart";
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
