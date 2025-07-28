using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:probably
    /// </summary>
    public partial class production_status
    {
        public production_status() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "production_status");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from production_status");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.production_status model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into production_status(");
            strSql.Append(
                "plc_config_id,user_name,ok_count,ng_count,is_baking,ppm,model,feed_count,discharge_count)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@plc_config_id,@user_name,@ok_count,@ng_count,@is_baking,@ppm,@model,@feed_count,@discharge_count)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@user_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ok_count", MySqlDbType.Int32, 11),
                new MySqlParameter("@ng_count", MySqlDbType.Int32, 11),
                new MySqlParameter("@is_baking", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ppm", MySqlDbType.Int32, 11),
                new MySqlParameter("@model", MySqlDbType.VarChar, 50),
                new MySqlParameter("@feed_count", MySqlDbType.Int32, 11),
                new MySqlParameter("@discharge_count", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.plc_config_id;
            parameters[1].Value = model.user_name;
            parameters[2].Value = model.ok_count;
            parameters[3].Value = model.ng_count;
            parameters[4].Value = model.is_baking;
            parameters[5].Value = model.ppm;
            parameters[6].Value = model.model;
            parameters[7].Value = model.feed_count;
            parameters[8].Value = model.discharge_count;

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
        public bool Update(SUNWODA_SEVB.Data.Model.production_status model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update production_status set ");
            strSql.Append("plc_config_id=@plc_config_id,");
            strSql.Append("user_name=@user_name,");
            strSql.Append("ok_count=@ok_count,");
            strSql.Append("ng_count=@ng_count,");
            strSql.Append("is_baking=@is_baking,");
            strSql.Append("ppm=@ppm,");
            strSql.Append("model=@model,");
            strSql.Append("feed_count=@feed_count,");
            strSql.Append("discharge_count=@discharge_count");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@user_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ok_count", MySqlDbType.Int32, 11),
                new MySqlParameter("@ng_count", MySqlDbType.Int32, 11),
                new MySqlParameter("@is_baking", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ppm", MySqlDbType.Int32, 11),
                new MySqlParameter("@model", MySqlDbType.VarChar, 50),
                new MySqlParameter("@feed_count", MySqlDbType.Int32, 11),
                new MySqlParameter("@discharge_count", MySqlDbType.Int32, 11),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.plc_config_id;
            parameters[1].Value = model.user_name;
            parameters[2].Value = model.ok_count;
            parameters[3].Value = model.ng_count;
            parameters[4].Value = model.is_baking;
            parameters[5].Value = model.ppm;
            parameters[6].Value = model.model;
            parameters[7].Value = model.feed_count;
            parameters[8].Value = model.discharge_count;
            parameters[9].Value = model.id;

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
            strSql.Append("delete from production_status ");
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
            strSql.Append("delete from production_status ");
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
        public SUNWODA_SEVB.Data.Model.production_status? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,plc_config_id,user_name,ok_count,ng_count,is_baking,ppm,model,feed_count,discharge_count from production_status "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.production_status model = new SUNWODA_SEVB.Data.Model.production_status();
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
        public SUNWODA_SEVB.Data.Model.production_status DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.production_status model = new SUNWODA_SEVB.Data.Model.production_status();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["plc_config_id"] != null && row["plc_config_id"].ToString() != "")
                {
                    model.plc_config_id = int.Parse(row["plc_config_id"].ToString()!);
                }
                if (row["user_name"] != null)
                {
                    model.user_name = row["user_name"].ToString()!;
                }
                if (row["ok_count"] != null && row["ok_count"].ToString() != "")
                {
                    model.ok_count = int.Parse(row["ok_count"].ToString()!);
                }
                if (row["ng_count"] != null && row["ng_count"].ToString() != "")
                {
                    model.ng_count = int.Parse(row["ng_count"].ToString()!);
                }
                if (row["is_baking"] != null)
                {
                    model.is_baking = row["is_baking"].ToString()!;
                }
                if (row["ppm"] != null && row["ppm"].ToString() != "")
                {
                    model.ppm = int.Parse(row["ppm"].ToString()!);
                }
                if (row["model"] != null)
                {
                    model.model = row["model"].ToString()!;
                }
                if (row["feed_count"] != null && row["feed_count"].ToString() != "")
                {
                    model.feed_count = int.Parse(row["feed_count"].ToString()!);
                }
                if (row["discharge_count"] != null && row["discharge_count"].ToString() != "")
                {
                    model.discharge_count = int.Parse(row["discharge_count"].ToString()!);
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
                "select id,plc_config_id,user_name,ok_count,ng_count,is_baking,ppm,model,feed_count,discharge_count "
            );
            strSql.Append(" FROM production_status ");
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
            strSql.Append("select count(1) FROM production_status ");
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
            strSql.Append(")AS Row, T.*  from production_status T ");
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
            parameters[0].Value = "probably";
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
