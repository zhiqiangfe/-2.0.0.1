using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:role
    /// </summary>
    public partial class user_role
    {
        private readonly DbHelperMySQL _dbHelper;
        public user_role(DbHelperMySQL dbHelper) { _dbHelper = dbHelper; }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return _dbHelper.GetMaxID("id", "user_role");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from user_role");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return _dbHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.user_role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_role(");
            strSql.Append(
                "role_name,mes_user_level,user_level_plc_value,permission_codes,create_time,modify_time,remark)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@role_name,@mes_user_level,@user_level_plc_value,@permission_codes,@create_time,@modify_time,@remark)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@role_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@mes_user_level", MySqlDbType.VarChar, 50),
                new MySqlParameter("@user_level_plc_value", MySqlDbType.Int32, 11),
                new MySqlParameter("@permission_codes", MySqlDbType.VarChar, 10000),
                new MySqlParameter("@create_time", MySqlDbType.DateTime),
                new MySqlParameter("@modify_time", MySqlDbType.DateTime),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 250),
            };
            parameters[0].Value = model.role_name;
            parameters[1].Value = model.mes_user_level;
            parameters[2].Value = model.user_level_plc_value;
            parameters[3].Value = model.permission_codes;
            parameters[4].Value = model.create_time;
            parameters[5].Value = model.modify_time;
            parameters[6].Value = model.remark;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(SUNWODA_SEVB.Data.Model.user_role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_role set ");
            strSql.Append("role_name=@role_name,");
            strSql.Append("mes_user_level=@mes_user_level,");
            strSql.Append("user_level_plc_value=@user_level_plc_value,");
            strSql.Append("permission_codes=@permission_codes,");
            strSql.Append("create_time=@create_time,");
            strSql.Append("modify_time=@modify_time,");
            strSql.Append("remark=@remark");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@role_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@mes_user_level", MySqlDbType.VarChar, 50),
                new MySqlParameter("@user_level_plc_value", MySqlDbType.Int32, 11),
                new MySqlParameter("@permission_codes", MySqlDbType.VarChar, 10000),
                new MySqlParameter("@create_time", MySqlDbType.DateTime),
                new MySqlParameter("@modify_time", MySqlDbType.DateTime),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 250),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.role_name;
            parameters[1].Value = model.mes_user_level;
            parameters[2].Value = model.user_level_plc_value;
            parameters[3].Value = model.permission_codes;
            parameters[4].Value = model.create_time;
            parameters[5].Value = model.modify_time;
            parameters[6].Value = model.remark;
            parameters[7].Value = model.id;

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
            strSql.Append("delete from user_role ");
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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from user_role ");
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
        public SUNWODA_SEVB.Data.Model.user_role? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,role_name,mes_user_level,user_level_plc_value,permission_codes,create_time,modify_time,remark from user_role "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.user_role model = new SUNWODA_SEVB.Data.Model.user_role();
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
        public SUNWODA_SEVB.Data.Model.user_role DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.user_role model = new SUNWODA_SEVB.Data.Model.user_role();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["role_name"] != null)
                {
                    model.role_name = row["role_name"].ToString()!;
                }
                if (row["mes_user_level"] != null)
                {
                    model.mes_user_level = row["mes_user_level"].ToString()!;
                }
                if (
                    row["user_level_plc_value"] != null
                    && row["user_level_plc_value"].ToString() != ""
                )
                {
                    model.user_level_plc_value = int.Parse(row["user_level_plc_value"].ToString()!);
                }
                if (row["permission_codes"] != null)
                {
                    model.permission_codes = row["permission_codes"].ToString()!;
                }
                if (row["create_time"] != null && row["create_time"].ToString() != "")
                {
                    model.create_time = DateTime.Parse(row["create_time"].ToString()!);
                }
                if (row["modify_time"] != null && row["modify_time"].ToString() != "")
                {
                    model.modify_time = DateTime.Parse(row["modify_time"].ToString()!);
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
            strSql.Append(
                "select id,role_name,mes_user_level,user_level_plc_value,permission_codes,create_time,modify_time,remark "
            );
            strSql.Append(" FROM user_role ");
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
            strSql.Append("select count(1) FROM user_role ");
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
            strSql.Append(")AS Row, T.*  from user_role T ");
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
            parameters[0].Value = "role";
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
