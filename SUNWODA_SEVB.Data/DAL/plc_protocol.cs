using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:plc_protocol
    /// </summary>
    public partial class plc_protocol
    {
        public plc_protocol() { }

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string protocol_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from plc_protocol");
            strSql.Append(" where protocol_name=@protocol_name ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@protocol_name", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = protocol_name;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.plc_protocol model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into plc_protocol(");
            strSql.Append("protocol_name,model,remark)");
            strSql.Append(" values (");
            strSql.Append("@protocol_name,@model,@remark)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@protocol_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@model", MySqlDbType.VarChar, 100),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
            };
            parameters[0].Value = model.protocol_name;
            parameters[1].Value = model.model;
            parameters[2].Value = model.remark;

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
        public bool Update(SUNWODA_SEVB.Data.Model.plc_protocol model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update plc_protocol set ");
            strSql.Append("model=@model,");
            strSql.Append("remark=@remark");
            strSql.Append(" where protocol_name=@protocol_name ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@model", MySqlDbType.VarChar, 100),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
                new MySqlParameter("@protocol_name", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.model;
            parameters[1].Value = model.remark;
            parameters[2].Value = model.protocol_name;

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
        public bool Delete(string protocol_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from plc_protocol ");
            strSql.Append(" where protocol_name=@protocol_name ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@protocol_name", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = protocol_name;

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
        public bool DeleteList(string protocol_namelist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from plc_protocol ");
            strSql.Append(" where protocol_name in (" + protocol_namelist + ")  ");
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
        public SUNWODA_SEVB.Data.Model.plc_protocol? GetModel(string protocol_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select protocol_name,model,remark from plc_protocol ");
            strSql.Append(" where protocol_name=@protocol_name ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@protocol_name", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = protocol_name;

            SUNWODA_SEVB.Data.Model.plc_protocol model = new SUNWODA_SEVB.Data.Model.plc_protocol();
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
        public SUNWODA_SEVB.Data.Model.plc_protocol DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.plc_protocol model = new SUNWODA_SEVB.Data.Model.plc_protocol();
            if (row != null)
            {
                if (row["protocol_name"] != null)
                {
                    model.protocol_name = row["protocol_name"].ToString()!;
                }
                if (row["model"] != null)
                {
                    model.model = row["model"].ToString()!;
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
            strSql.Append("select protocol_name,model,remark ");
            strSql.Append(" FROM plc_protocol ");
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
            strSql.Append("select count(1) FROM plc_protocol ");
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
                strSql.Append("order by T.protocol_name desc");
            }
            strSql.Append(")AS Row, T.*  from plc_protocol T ");
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
            parameters[0].Value = "plc_protocol";
            parameters[1].Value = "protocol_name";
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
