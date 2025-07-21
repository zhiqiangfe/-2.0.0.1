using System;
using System.Data;
using System.Text;
using Maticsoft.DBUtility; //Please add references
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:alarm_level
    /// </summary>
    public partial class alarm_level
    {
        public alarm_level() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "alarm_level");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from alarm_level");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.alarm_level model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into alarm_level(");
            strSql.Append("level_name,remark)");
            strSql.Append(" values (");
            strSql.Append("@level_name,@remark)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@level_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.level_name;
            parameters[1].Value = model.remark;

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
        public bool Update(SUNWODA_SEVB.Data.Model.alarm_level model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update alarm_level set ");
            strSql.Append("level_name=@level_name,");
            strSql.Append("remark=@remark");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@level_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.level_name;
            parameters[1].Value = model.remark;
            parameters[2].Value = model.id;

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
            strSql.Append("delete from alarm_level ");
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
            strSql.Append("delete from alarm_level ");
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
        public SUNWODA_SEVB.Data.Model.alarm_level? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,level_name,remark from alarm_level ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.alarm_level model = new SUNWODA_SEVB.Data.Model.alarm_level();
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
        public SUNWODA_SEVB.Data.Model.alarm_level DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.alarm_level model = new SUNWODA_SEVB.Data.Model.alarm_level();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["level_name"] != null)
                {
                    model.level_name = row["level_name"].ToString()!;
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
            strSql.Append("select id,level_name,remark ");
            strSql.Append(" FROM alarm_level ");
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
            strSql.Append("select count(1) FROM alarm_level ");
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
            strSql.Append(")AS Row, T.*  from alarm_level T ");
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
            parameters[0].Value = "alarm_level";
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
