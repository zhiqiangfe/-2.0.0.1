using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:input_variable_history
    /// </summary>
    public partial class input_variable_history
    {
        public input_variable_history() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "input_variable_history");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from input_variable_history");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.input_variable_history model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into input_variable_history(");
            strSql.Append(
                "equipment_id,SendParamID,UploadParamID,ParamName,Type,ParamValueRatio,Model,HistoryMaxValue,HistoryMinValue,HistoryStandardValue,ChangeMonitorValue,ActualValue,BycellOutputValue,DataTime,LogFrom,DownloadRemark)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@equipment_id,@SendParamID,@UploadParamID,@ParamName,@Type,@ParamValueRatio,@Model,@HistoryMaxValue,@HistoryMinValue,@HistoryStandardValue,@ChangeMonitorValue,@ActualValue,@BycellOutputValue,@DataTime,@LogFrom,@DownloadRemark)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@SendParamID", MySqlDbType.VarChar, 50),
                new MySqlParameter("@UploadParamID", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ParamName", MySqlDbType.VarChar, 50),
                new MySqlParameter("@Type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ParamValueRatio", MySqlDbType.VarChar, 50),
                new MySqlParameter("@Model", MySqlDbType.VarChar, 50),
                new MySqlParameter("@HistoryMaxValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@HistoryMinValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@HistoryStandardValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ChangeMonitorValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ActualValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@BycellOutputValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@DataTime", MySqlDbType.DateTime),
                new MySqlParameter("@LogFrom", MySqlDbType.VarChar, 50),
                new MySqlParameter("@DownloadRemark", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.SendParamID;
            parameters[2].Value = model.UploadParamID;
            parameters[3].Value = model.ParamName;
            parameters[4].Value = model.Type;
            parameters[5].Value = model.ParamValueRatio;
            parameters[6].Value = model.Model;
            parameters[7].Value = model.HistoryMaxValue;
            parameters[8].Value = model.HistoryMinValue;
            parameters[9].Value = model.HistoryStandardValue;
            parameters[10].Value = model.ChangeMonitorValue;
            parameters[11].Value = model.ActualValue;
            parameters[12].Value = model.BycellOutputValue;
            parameters[13].Value = model.DataTime;
            parameters[14].Value = model.LogFrom;
            parameters[15].Value = model.DownloadRemark;

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
        public bool Update(SUNWODA_SEVB.Data.Model.input_variable_history model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update input_variable_history set ");
            strSql.Append("equipment_id=@equipment_id,");
            strSql.Append("SendParamID=@SendParamID,");
            strSql.Append("UploadParamID=@UploadParamID,");
            strSql.Append("ParamName=@ParamName,");
            strSql.Append("Type=@Type,");
            strSql.Append("ParamValueRatio=@ParamValueRatio,");
            strSql.Append("Model=@Model,");
            strSql.Append("HistoryMaxValue=@HistoryMaxValue,");
            strSql.Append("HistoryMinValue=@HistoryMinValue,");
            strSql.Append("HistoryStandardValue=@HistoryStandardValue,");
            strSql.Append("ChangeMonitorValue=@ChangeMonitorValue,");
            strSql.Append("ActualValue=@ActualValue,");
            strSql.Append("BycellOutputValue=@BycellOutputValue,");
            strSql.Append("DataTime=@DataTime,");
            strSql.Append("LogFrom=@LogFrom,");
            strSql.Append("DownloadRemark=@DownloadRemark");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@SendParamID", MySqlDbType.VarChar, 50),
                new MySqlParameter("@UploadParamID", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ParamName", MySqlDbType.VarChar, 50),
                new MySqlParameter("@Type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ParamValueRatio", MySqlDbType.VarChar, 50),
                new MySqlParameter("@Model", MySqlDbType.VarChar, 50),
                new MySqlParameter("@HistoryMaxValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@HistoryMinValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@HistoryStandardValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ChangeMonitorValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@ActualValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@BycellOutputValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@DataTime", MySqlDbType.DateTime),
                new MySqlParameter("@LogFrom", MySqlDbType.VarChar, 50),
                new MySqlParameter("@DownloadRemark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.equipment_id;
            parameters[1].Value = model.SendParamID;
            parameters[2].Value = model.UploadParamID;
            parameters[3].Value = model.ParamName;
            parameters[4].Value = model.Type;
            parameters[5].Value = model.ParamValueRatio;
            parameters[6].Value = model.Model;
            parameters[7].Value = model.HistoryMaxValue;
            parameters[8].Value = model.HistoryMinValue;
            parameters[9].Value = model.HistoryStandardValue;
            parameters[10].Value = model.ChangeMonitorValue;
            parameters[11].Value = model.ActualValue;
            parameters[12].Value = model.BycellOutputValue;
            parameters[13].Value = model.DataTime;
            parameters[14].Value = model.LogFrom;
            parameters[15].Value = model.DownloadRemark;
            parameters[16].Value = model.id;

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
            strSql.Append("delete from input_variable_history ");
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
            strSql.Append("delete from input_variable_history ");
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
        public SUNWODA_SEVB.Data.Model.input_variable_history? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,equipment_id,SendParamID,UploadParamID,ParamName,Type,ParamValueRatio,Model,HistoryMaxValue,HistoryMinValue,HistoryStandardValue,ChangeMonitorValue,ActualValue,BycellOutputValue,DataTime,LogFrom,DownloadRemark from input_variable_history "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.input_variable_history model = new SUNWODA_SEVB.Data.Model.input_variable_history();
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
        public SUNWODA_SEVB.Data.Model.input_variable_history DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.input_variable_history model = new SUNWODA_SEVB.Data.Model.input_variable_history();
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
                if (row["SendParamID"] != null)
                {
                    model.SendParamID = row["SendParamID"].ToString()!;
                }
                if (row["UploadParamID"] != null)
                {
                    model.UploadParamID = row["UploadParamID"].ToString()!;
                }
                if (row["ParamName"] != null)
                {
                    model.ParamName = row["ParamName"].ToString()!;
                }
                if (row["Type"] != null)
                {
                    model.Type = row["Type"].ToString()!;
                }
                if (row["ParamValueRatio"] != null)
                {
                    model.ParamValueRatio = row["ParamValueRatio"].ToString()!;
                }
                if (row["Model"] != null)
                {
                    model.Model = row["Model"].ToString()!;
                }
                if (row["HistoryMaxValue"] != null)
                {
                    model.HistoryMaxValue = row["HistoryMaxValue"].ToString()!;
                }
                if (row["HistoryMinValue"] != null)
                {
                    model.HistoryMinValue = row["HistoryMinValue"].ToString()!;
                }
                if (row["HistoryStandardValue"] != null)
                {
                    model.HistoryStandardValue = row["HistoryStandardValue"].ToString()!;
                }
                if (row["ChangeMonitorValue"] != null)
                {
                    model.ChangeMonitorValue = row["ChangeMonitorValue"].ToString()!;
                }
                if (row["ActualValue"] != null)
                {
                    model.ActualValue = row["ActualValue"].ToString()!;
                }
                if (row["BycellOutputValue"] != null)
                {
                    model.BycellOutputValue = row["BycellOutputValue"].ToString()!;
                }
                if (row["DataTime"] != null && row["DataTime"].ToString() != "")
                {
                    model.DataTime = DateTime.Parse(row["DataTime"].ToString()!);
                }
                if (row["LogFrom"] != null)
                {
                    model.LogFrom = row["LogFrom"].ToString()!;
                }
                if (row["DownloadRemark"] != null)
                {
                    model.DownloadRemark = row["DownloadRemark"].ToString()!;
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
                "select id,equipment_id,SendParamID,UploadParamID,ParamName,Type,ParamValueRatio,Model,HistoryMaxValue,HistoryMinValue,HistoryStandardValue,ChangeMonitorValue,ActualValue,BycellOutputValue,DataTime,LogFrom,DownloadRemark "
            );
            strSql.Append(" FROM input_variable_history ");
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
            strSql.Append("select count(1) FROM input_variable_history ");
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
            strSql.Append(")AS Row, T.*  from input_variable_history T ");
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
            parameters[0].Value = "input_variable_history";
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
