using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device_production_data
    /// </summary>
    public partial class device_production_data
    {
        public device_production_data() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "device_production_data");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_production_data");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_production_data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_production_data(");
            strSql.Append("sn1,sn2,sn3,time1,time2,time3,data1,data2,data3)");
            strSql.Append(" values (");
            strSql.Append("@sn1,@sn2,@sn3,@time1,@time2,@time3,@data1,@data2,@data3)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@sn1", MySqlDbType.VarChar, 50),
                new MySqlParameter("@sn2", MySqlDbType.VarChar, 50),
                new MySqlParameter("@sn3", MySqlDbType.VarChar, 50),
                new MySqlParameter("@time1", MySqlDbType.DateTime),
                new MySqlParameter("@time2", MySqlDbType.DateTime),
                new MySqlParameter("@time3", MySqlDbType.DateTime),
                new MySqlParameter("@data1", MySqlDbType.MediumText),
                new MySqlParameter("@data2", MySqlDbType.MediumText),
                new MySqlParameter("@data3", MySqlDbType.MediumText),
            };
            parameters[0].Value = model.sn1;
            parameters[1].Value = model.sn2;
            parameters[2].Value = model.sn3;
            parameters[3].Value = model.time1;
            parameters[4].Value = model.time2;
            parameters[5].Value = model.time3;
            parameters[6].Value = model.data1;
            parameters[7].Value = model.data2;
            parameters[8].Value = model.data3;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_production_data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_production_data set ");
            strSql.Append("sn1=@sn1,");
            strSql.Append("sn2=@sn2,");
            strSql.Append("sn3=@sn3,");
            strSql.Append("time1=@time1,");
            strSql.Append("time2=@time2,");
            strSql.Append("time3=@time3,");
            strSql.Append("data1=@data1,");
            strSql.Append("data2=@data2,");
            strSql.Append("data3=@data3");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@sn1", MySqlDbType.VarChar, 50),
                new MySqlParameter("@sn2", MySqlDbType.VarChar, 50),
                new MySqlParameter("@sn3", MySqlDbType.VarChar, 50),
                new MySqlParameter("@time1", MySqlDbType.DateTime),
                new MySqlParameter("@time2", MySqlDbType.DateTime),
                new MySqlParameter("@time3", MySqlDbType.DateTime),
                new MySqlParameter("@data1", MySqlDbType.MediumText),
                new MySqlParameter("@data2", MySqlDbType.MediumText),
                new MySqlParameter("@data3", MySqlDbType.MediumText),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.sn1;
            parameters[1].Value = model.sn2;
            parameters[2].Value = model.sn3;
            parameters[3].Value = model.time1;
            parameters[4].Value = model.time2;
            parameters[5].Value = model.time3;
            parameters[6].Value = model.data1;
            parameters[7].Value = model.data2;
            parameters[8].Value = model.data3;
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
            strSql.Append("delete from device_production_data ");
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
            strSql.Append("delete from device_production_data ");
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
        public SUNWODA_SEVB.Data.Model.device_production_data? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,sn1,sn2,sn3,time1,time2,time3,data1,data2,data3 from device_production_data "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.device_production_data model = new SUNWODA_SEVB.Data.Model.device_production_data();
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
        public SUNWODA_SEVB.Data.Model.device_production_data DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_production_data model = new SUNWODA_SEVB.Data.Model.device_production_data();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["sn1"] != null)
                {
                    model.sn1 = row["sn1"].ToString()!;
                }
                if (row["sn2"] != null)
                {
                    model.sn2 = row["sn2"].ToString()!;
                }
                if (row["sn3"] != null)
                {
                    model.sn3 = row["sn3"].ToString()!;
                }
                if (row["time1"] != null && row["time1"].ToString() != "")
                {
                    model.time1 = DateTime.Parse(row["time1"].ToString()!);
                }
                if (row["time2"] != null && row["time2"].ToString() != "")
                {
                    model.time2 = DateTime.Parse(row["time2"].ToString()!);
                }
                if (row["time3"] != null && row["time3"].ToString() != "")
                {
                    model.time3 = DateTime.Parse(row["time3"].ToString()!);
                }
                if (row["data1"] != null)
                {
                    model.data1 = row["data1"].ToString()!;
                }
                if (row["data2"] != null)
                {
                    model.data2 = row["data2"].ToString()!;
                }
                if (row["data3"] != null)
                {
                    model.data3 = row["data3"].ToString()!;
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
            strSql.Append("select id,sn1,sn2,sn3,time1,time2,time3,data1,data2,data3 ");
            strSql.Append(" FROM device_production_data ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        #region gzy
        public DataSet GetList_YunXingZhuangTai(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  ");
            strSql.Append(" FROM device_andon_record ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where 1=1 " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        public DataSet GetList_ChanLiang(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,logdate,method,output_json ");
            strSql.Append(" FROM log_mes_interface ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where 1=1 " + strWhere);
            }
            //strSql.Append(" order by logdate desc");

            return DbHelperMySQL.Query(strSql.ToString());
        }

        public DataSet GetList_Device_Title(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select name,id ");
            strSql.Append(" FROM Device_Title ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where 1=1 " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }
        #endregion
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM device_production_data ");
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
            strSql.Append(")AS Row, T.*  from device_production_data T ");
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
            parameters[0].Value = "device_production_data";
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
