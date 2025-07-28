using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:log_mes_interface_paramvalue
    /// </summary>
    public partial class log_mes_interface_paramvalue
    {
        public log_mes_interface_paramvalue() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "log_mes_interface_paramvalue");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from log_mes_interface_paramvalue");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.log_mes_interface_paramvalue model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into log_mes_interface_paramvalue(");
            strSql.Append(
                "logdate,success_flag,groupCode,operatorId,deviceSn,productSn,moNumber,testResult,paramCode,paramName,paramValue,paramResult,paramUnit)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@logdate,@success_flag,@groupCode,@operatorId,@deviceSn,@productSn,@moNumber,@testResult,@paramCode,@paramName,@paramValue,@paramResult,@paramUnit)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@logdate", MySqlDbType.DateTime),
                new MySqlParameter("@success_flag", MySqlDbType.VarChar, 50),
                new MySqlParameter("@groupCode", MySqlDbType.VarChar, 50),
                new MySqlParameter("@operatorId", MySqlDbType.VarChar, 50),
                new MySqlParameter("@deviceSn", MySqlDbType.VarChar, 50),
                new MySqlParameter("@productSn", MySqlDbType.VarChar, 50),
                new MySqlParameter("@moNumber", MySqlDbType.VarChar, 50),
                new MySqlParameter("@testResult", MySqlDbType.VarChar, 50),
                new MySqlParameter("@paramCode", MySqlDbType.Int32, 11),
                new MySqlParameter("@paramName", MySqlDbType.VarChar, 50),
                new MySqlParameter("@paramValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@paramResult", MySqlDbType.VarChar, 50),
                new MySqlParameter("@paramUnit", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.logdate;
            parameters[1].Value = model.success_flag;
            parameters[2].Value = model.groupCode;
            parameters[3].Value = model.operatorId;
            parameters[4].Value = model.deviceSn;
            parameters[5].Value = model.productSn;
            parameters[6].Value = model.moNumber;
            parameters[7].Value = model.testResult;
            parameters[8].Value = model.paramCode;
            parameters[9].Value = model.paramName;
            parameters[10].Value = model.paramValue;
            parameters[11].Value = model.paramResult;
            parameters[12].Value = model.paramUnit;

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
        public bool Update(SUNWODA_SEVB.Data.Model.log_mes_interface_paramvalue model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update log_mes_interface_paramvalue set ");
            strSql.Append("logdate=@logdate,");
            strSql.Append("success_flag=@success_flag,");
            strSql.Append("groupCode=@groupCode,");
            strSql.Append("operatorId=@operatorId,");
            strSql.Append("deviceSn=@deviceSn,");
            strSql.Append("productSn=@productSn,");
            strSql.Append("moNumber=@moNumber,");
            strSql.Append("testResult=@testResult,");
            strSql.Append("paramCode=@paramCode,");
            strSql.Append("paramName=@paramName,");
            strSql.Append("paramValue=@paramValue,");
            strSql.Append("paramResult=@paramResult,");
            strSql.Append("paramUnit=@paramUnit");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@logdate", MySqlDbType.DateTime),
                new MySqlParameter("@success_flag", MySqlDbType.VarChar, 50),
                new MySqlParameter("@groupCode", MySqlDbType.VarChar, 50),
                new MySqlParameter("@operatorId", MySqlDbType.VarChar, 50),
                new MySqlParameter("@deviceSn", MySqlDbType.VarChar, 50),
                new MySqlParameter("@productSn", MySqlDbType.VarChar, 50),
                new MySqlParameter("@moNumber", MySqlDbType.VarChar, 50),
                new MySqlParameter("@testResult", MySqlDbType.VarChar, 50),
                new MySqlParameter("@paramCode", MySqlDbType.Int32, 11),
                new MySqlParameter("@paramName", MySqlDbType.VarChar, 50),
                new MySqlParameter("@paramValue", MySqlDbType.VarChar, 50),
                new MySqlParameter("@paramResult", MySqlDbType.VarChar, 50),
                new MySqlParameter("@paramUnit", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.logdate;
            parameters[1].Value = model.success_flag;
            parameters[2].Value = model.groupCode;
            parameters[3].Value = model.operatorId;
            parameters[4].Value = model.deviceSn;
            parameters[5].Value = model.productSn;
            parameters[6].Value = model.moNumber;
            parameters[7].Value = model.testResult;
            parameters[8].Value = model.paramCode;
            parameters[9].Value = model.paramName;
            parameters[10].Value = model.paramValue;
            parameters[11].Value = model.paramResult;
            parameters[12].Value = model.paramUnit;
            parameters[13].Value = model.id;

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
            strSql.Append("delete from log_mes_interface_paramvalue ");
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
            strSql.Append("delete from log_mes_interface_paramvalue ");
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
        public SUNWODA_SEVB.Data.Model.log_mes_interface_paramvalue? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,logdate,success_flag,groupCode,operatorId,deviceSn,productSn,moNumber,testResult,paramCode,paramName,paramValue,paramResult,paramUnit from log_mes_interface_paramvalue "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.log_mes_interface_paramvalue model =
                new SUNWODA_SEVB.Data.Model.log_mes_interface_paramvalue();
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
        public SUNWODA_SEVB.Data.Model.log_mes_interface_paramvalue DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.log_mes_interface_paramvalue model =
                new SUNWODA_SEVB.Data.Model.log_mes_interface_paramvalue();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["logdate"] != null && row["logdate"].ToString() != "")
                {
                    model.logdate = DateTime.Parse(row["logdate"].ToString()!);
                }
                if (row["success_flag"] != null)
                {
                    model.success_flag = row["success_flag"].ToString()!;
                }
                if (row["groupCode"] != null)
                {
                    model.groupCode = row["groupCode"].ToString()!;
                }
                if (row["operatorId"] != null)
                {
                    model.operatorId = row["operatorId"].ToString()!;
                }
                if (row["deviceSn"] != null)
                {
                    model.deviceSn = row["deviceSn"].ToString()!;
                }
                if (row["productSn"] != null)
                {
                    model.productSn = row["productSn"].ToString()!;
                }
                if (row["moNumber"] != null)
                {
                    model.moNumber = row["moNumber"].ToString()!;
                }
                if (row["testResult"] != null)
                {
                    model.testResult = row["testResult"].ToString()!;
                }
                if (row["paramCode"] != null && row["paramCode"].ToString() != "")
                {
                    model.paramCode = int.Parse(row["paramCode"].ToString()!);
                }
                if (row["paramName"] != null)
                {
                    model.paramName = row["paramName"].ToString()!;
                }
                if (row["paramValue"] != null)
                {
                    model.paramValue = row["paramValue"].ToString()!;
                }
                if (row["paramResult"] != null)
                {
                    model.paramResult = row["paramResult"].ToString()!;
                }
                if (row["paramUnit"] != null)
                {
                    model.paramUnit = row["paramUnit"].ToString()!;
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
                "select id,logdate,success_flag,groupCode,operatorId,deviceSn,productSn,moNumber,testResult,paramCode,paramName,paramValue,paramResult,paramUnit "
            );
            strSql.Append(" FROM log_mes_interface_paramvalue ");
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
            strSql.Append("select count(1) FROM log_mes_interface_paramvalue ");
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
            strSql.Append(")AS Row, T.*  from log_mes_interface_paramvalue T ");
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
            parameters[0].Value = "log_mes_interface_paramvalue";
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
