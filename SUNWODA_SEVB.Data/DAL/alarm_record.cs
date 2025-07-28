using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:alarm_record
    /// </summary>
    public partial class alarm_record
    {
        public alarm_record() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "alarm_record");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from alarm_record");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.alarm_record model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into alarm_record(");
            strSql.Append(
                "equipment_id,upload_param_id,alarm_time,dispose_time,create_by,mhandler,duration)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@equipment_id,@upload_param_id,@alarm_time,@dispose_time,@create_by,@mhandler,@duration)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarm_time", MySqlDbType.DateTime),
                new MySqlParameter("@dispose_time", MySqlDbType.DateTime),
                new MySqlParameter("@create_by", MySqlDbType.VarChar, 50),
                new MySqlParameter("@mhandler", MySqlDbType.VarChar, 50),
                new MySqlParameter("@duration", MySqlDbType.Float, 11),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.upload_param_id;
            parameters[2].Value = model.alarm_time;
            parameters[3].Value = model.dispose_time;
            parameters[4].Value = model.create_by;
            parameters[5].Value = model.mhandler;
            parameters[6].Value = model.duration;

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
        public bool Update(SUNWODA_SEVB.Data.Model.alarm_record model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update alarm_record set ");
            strSql.Append("equipment_id=@equipment_id,");
            strSql.Append("upload_param_id=@upload_param_id,");
            strSql.Append("alarm_time=@alarm_time,");
            strSql.Append("dispose_time=@dispose_time,");
            strSql.Append("create_by=@create_by,");
            strSql.Append("mhandler=@mhandler,");
            strSql.Append("duration=@duration");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@upload_param_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@alarm_time", MySqlDbType.DateTime),
                new MySqlParameter("@dispose_time", MySqlDbType.DateTime),
                new MySqlParameter("@create_by", MySqlDbType.VarChar, 50),
                new MySqlParameter("@mhandler", MySqlDbType.VarChar, 50),
                new MySqlParameter("@duration", MySqlDbType.Float, 11),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.upload_param_id;
            parameters[2].Value = model.alarm_time;
            parameters[3].Value = model.dispose_time;
            parameters[4].Value = model.create_by;
            parameters[5].Value = model.mhandler;
            parameters[6].Value = model.duration;
            parameters[7].Value = model.id;

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
            strSql.Append("delete from alarm_record ");
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
            strSql.Append("delete from alarm_record ");
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
        public SUNWODA_SEVB.Data.Model.alarm_record? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,equipment_id,upload_param_id,alarm_time,dispose_time,create_by,mhandler,duration from alarm_record "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.alarm_record model = new SUNWODA_SEVB.Data.Model.alarm_record();
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
        public SUNWODA_SEVB.Data.Model.alarm_record DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.alarm_record model = new SUNWODA_SEVB.Data.Model.alarm_record();
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
                if (row["alarm_time"] != null && row["alarm_time"].ToString() != "")
                {
                    model.alarm_time = DateTime.Parse(row["alarm_time"].ToString()!);
                }
                if (row["dispose_time"] != null && row["dispose_time"].ToString() != "")
                {
                    model.dispose_time = DateTime.Parse(row["dispose_time"].ToString()!);
                }
                if (row["create_by"] != null)
                {
                    model.create_by = row["create_by"].ToString()!;
                }
                if (row["mhandler"] != null)
                {
                    model.mhandler = row["mhandler"].ToString()!;
                }
                if (row["duration"] != null && row["duration"].ToString() != "")
                {
                    model.duration = decimal.Parse(row["duration"].ToString()!);
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
                "select id,equipment_id,upload_param_id,alarm_time,dispose_time,create_by,mhandler,duration "
            );
            strSql.Append(" FROM alarm_record ");
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
            strSql.Append("select count(1) FROM alarm_record ");
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
            strSql.Append(")AS Row, T.*  from alarm_record T ");
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
            parameters[0].Value = "alarm_record";
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
