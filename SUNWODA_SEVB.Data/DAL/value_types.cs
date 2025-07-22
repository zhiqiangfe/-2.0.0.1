using System;
using System.Data;
using System.Text;
using Maticsoft.DBUtility; //Please add references
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:valuetypes
    /// </summary>
    public partial class value_types
    {
        public value_types() { }

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string value_type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from value_types");
            strSql.Append(" where value_type=@value_type ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = value_type;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.value_types model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into value_types(");
            strSql.Append("value_type,remark)");
            strSql.Append(" values (");
            strSql.Append("@value_type,@remark)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.value_type;
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
        public bool Update(SUNWODA_SEVB.Data.Model.value_types model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update value_types set ");
            strSql.Append("remark=@remark");
            strSql.Append(" where value_type=@value_type ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.remark;
            parameters[1].Value = model.value_type;

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
        public bool Delete(string value_type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from value_types ");
            strSql.Append(" where value_type=@value_type ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = value_type;

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
        public bool DeleteList(string value_typelist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from value_types ");
            strSql.Append(" where value_type in (" + value_typelist + ")  ");
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
        public SUNWODA_SEVB.Data.Model.value_types? GetModel(string value_type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select value_type,remark from value_types ");
            strSql.Append(" where value_type=@value_type ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@value_type", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = value_type;

            SUNWODA_SEVB.Data.Model.value_types model = new SUNWODA_SEVB.Data.Model.value_types();
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
        public SUNWODA_SEVB.Data.Model.value_types DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.value_types model = new SUNWODA_SEVB.Data.Model.value_types();
            if (row != null)
            {
                if (row["value_type"] != null)
                {
                    model.value_type = row["value_type"].ToString()!;
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
            strSql.Append("select value_type,remark ");
            strSql.Append(" FROM value_types ");
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
            strSql.Append("select count(1) FROM value_types ");
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
                strSql.Append("order by T.value_type desc");
            }
            strSql.Append(")AS Row, T.*  from value_types T ");
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
            parameters[0].Value = "valuetypes";
            parameters[1].Value = "value_type";
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
