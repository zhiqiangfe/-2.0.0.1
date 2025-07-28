using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:plc_area_config
    /// </summary>
    public partial class plc_area_config
    {
        public plc_area_config() { }

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string area_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from plc_area_config");
            strSql.Append(" where area_name=@area_name ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@area_name", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = area_name;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.plc_area_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into plc_area_config(");
            strSql.Append("area_name,area_length,brand)");
            strSql.Append(" values (");
            strSql.Append("@area_name,@area_length,@brand)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@area_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@area_length", MySqlDbType.Int32, 11),
                new MySqlParameter("@brand", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.area_name;
            parameters[1].Value = model.area_length;
            parameters[2].Value = model.brand;

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
        public bool Update(SUNWODA_SEVB.Data.Model.plc_area_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update plc_area_config set ");
            strSql.Append("area_length=@area_length,");
            strSql.Append("brand=@brand");
            strSql.Append(" where area_name=@area_name ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@area_length", MySqlDbType.Int32, 11),
                new MySqlParameter("@brand", MySqlDbType.VarChar, 50),
                new MySqlParameter("@area_name", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.area_length;
            parameters[1].Value = model.brand;
            parameters[2].Value = model.area_name;

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
        public bool Delete(string area_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from plc_area_config ");
            strSql.Append(" where area_name=@area_name ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@area_name", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = area_name;

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
        public bool DeleteList(string area_namelist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from plc_area_config ");
            strSql.Append(" where area_name in (" + area_namelist + ")  ");
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
        public SUNWODA_SEVB.Data.Model.plc_area_config? GetModel(string area_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select area_name,area_length,brand from plc_area_config ");
            strSql.Append(" where area_name=@area_name ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@area_name", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = area_name;

            SUNWODA_SEVB.Data.Model.plc_area_config model = new SUNWODA_SEVB.Data.Model.plc_area_config();
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
        public SUNWODA_SEVB.Data.Model.plc_area_config DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.plc_area_config model = new SUNWODA_SEVB.Data.Model.plc_area_config();
            if (row != null)
            {
                if (row["area_name"] != null)
                {
                    model.area_name = row["area_name"].ToString()!;
                }
                if (row["area_length"] != null && row["area_length"].ToString() != "")
                {
                    model.area_length = int.Parse(row["area_length"].ToString()!);
                }
                if (row["brand"] != null)
                {
                    model.brand = row["brand"].ToString()!;
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
            strSql.Append("select area_name,area_length,brand ");
            strSql.Append(" FROM plc_area_config ");
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
            strSql.Append("select count(1) FROM plc_area_config ");
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
                strSql.Append("order by T.area_name desc");
            }
            strSql.Append(")AS Row, T.*  from plc_area_config T ");
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
            parameters[0].Value = "plc_area_config";
            parameters[1].Value = "area_name";
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
