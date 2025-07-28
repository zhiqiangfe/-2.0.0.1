using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device_weight_data
    /// </summary>
    public partial class device_weight_data
    {
        public device_weight_data() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "device_weight_data");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_weight_data");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_weight_data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_weight_data(");
            strSql.Append("number,messagetag,creat_time,weight,sn,data)");
            strSql.Append(" values (");
            strSql.Append("@number,@messagetag,@creat_time,@weight,@sn,@data)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@number", MySqlDbType.Int32, 11),
                new MySqlParameter("@messagetag", MySqlDbType.VarChar, 50),
                new MySqlParameter("@creat_time", MySqlDbType.DateTime),
                new MySqlParameter("@weight", MySqlDbType.MediumText),
                new MySqlParameter("@sn", MySqlDbType.VarChar, 50),
                new MySqlParameter("@data", MySqlDbType.MediumText, 9),
            };
            parameters[0].Value = model.number;
            parameters[1].Value = model.messagetag;
            parameters[2].Value = model.creat_time;
            parameters[3].Value = model.weight;
            parameters[4].Value = model.sn;
            parameters[5].Value = model.data;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_weight_data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_weight_data set ");
            strSql.Append("number=@number,");
            strSql.Append("messagetag=@messagetag,");
            strSql.Append("creat_time=@creat_time,");
            strSql.Append("weight=@weight,");
            strSql.Append("sn=@sn,");
            strSql.Append("data=@data");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@number", MySqlDbType.Int32, 11),
                new MySqlParameter("@messagetag", MySqlDbType.VarChar, 50),
                new MySqlParameter("@creat_time", MySqlDbType.DateTime),
                new MySqlParameter("@weight", MySqlDbType.MediumText),
                new MySqlParameter("@sn", MySqlDbType.VarChar, 50),
                new MySqlParameter("@data", MySqlDbType.MediumText, 9),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.number;
            parameters[1].Value = model.messagetag;
            parameters[2].Value = model.creat_time;
            parameters[3].Value = model.weight;
            parameters[4].Value = model.sn;
            parameters[5].Value = model.data;
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
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from device_weight_data ");
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
            strSql.Append("delete from device_weight_data ");
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
        public SUNWODA_SEVB.Data.Model.device_weight_data? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,number,messagetag,creat_time,weight,sn,data from device_weight_data "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.device_weight_data model = new SUNWODA_SEVB.Data.Model.device_weight_data();
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
        public SUNWODA_SEVB.Data.Model.device_weight_data DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_weight_data model = new SUNWODA_SEVB.Data.Model.device_weight_data();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["number"] != null && row["number"].ToString() != "")
                {
                    model.number = int.Parse(row["number"].ToString()!);
                }
                if (row["messagetag"] != null)
                {
                    model.messagetag = row["messagetag"].ToString()!;
                }
                if (row["creat_time"] != null && row["creat_time"].ToString() != "")
                {
                    model.creat_time = DateTime.Parse(row["creat_time"].ToString()!);
                }
                model.weight = row["weight"].ToString()!;
                if (row["sn"] != null)
                {
                    model.sn = row["sn"].ToString()!;
                }
                model.data = row["data"].ToString()!;
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,number,messagetag,creat_time,weight,sn,data ");
            strSql.Append(" FROM device_weight_data ");
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
            strSql.Append("select count(1) FROM device_weight_data ");
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
            strSql.Append(")AS Row, T.*  from device_weight_data T ");
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
            parameters[0].Value = "device_weight_data";
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
