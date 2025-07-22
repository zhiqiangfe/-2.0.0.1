using System;
using System.Data;
using System.Text;
using Maticsoft.DBUtility;
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:plc_config
    /// </summary>
    public partial class plc_config
    {
        public plc_config() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "plc_config");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from plc_config");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.plc_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into plc_config(");
            strSql.Append(
                "name,device_id,ip,port,brand_specification_protocal,data_sort_rule,state,remark,enabled)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@plc_name,@equipment_id,@ip,@port,@brand_specification_protocal,@data_sort_rule,@state,@remark,@enabled)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@plc_name", MySqlDbType.VarChar, 30),
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 30),
                new MySqlParameter("@ip", MySqlDbType.VarChar, 20),
                new MySqlParameter("@port", MySqlDbType.VarChar, 50),
                new MySqlParameter("@brand_specification_protocal", MySqlDbType.VarChar, 50),
                new MySqlParameter("@data_sort_rule", MySqlDbType.VarChar, 50),
                new MySqlParameter("@state", MySqlDbType.Int32, 11),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.name;
            parameters[1].Value = model.device_id;
            parameters[2].Value = model.ip;
            parameters[3].Value = model.port;
            parameters[4].Value = model.brand_specification_protocal;
            parameters[5].Value = model.data_sort_rule;
            parameters[6].Value = model.state;
            parameters[7].Value = model.remark;
            parameters[8].Value = model.enabled;

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
        public bool Update(SUNWODA_SEVB.Data.Model.plc_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update plc_config set ");
            strSql.Append("name=@name,");
            strSql.Append("device_id=@device_id,");
            strSql.Append("ip=@ip,");
            strSql.Append("port=@port,");
            strSql.Append("brand_specification_protocal=@brand_specification_protocal,");
            strSql.Append("data_sort_rule=@data_sort_rule,");
            strSql.Append("state=@state,");
            strSql.Append("remark=@remark,");
            strSql.Append("enabled=@enabled");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@name", MySqlDbType.VarChar, 30),
                new MySqlParameter("@device_id", MySqlDbType.VarChar, 30),
                new MySqlParameter("@ip", MySqlDbType.VarChar, 20),
                new MySqlParameter("@port", MySqlDbType.VarChar, 50),
                new MySqlParameter("@brand_specification_protocal", MySqlDbType.VarChar, 50),
                new MySqlParameter("@data_sort_rule", MySqlDbType.VarChar, 50),
                new MySqlParameter("@state", MySqlDbType.Int32, 11),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.name;
            parameters[1].Value = model.device_id;
            parameters[2].Value = model.ip;
            parameters[3].Value = model.port;
            parameters[4].Value = model.brand_specification_protocal;
            parameters[5].Value = model.data_sort_rule;
            parameters[6].Value = model.state;
            parameters[7].Value = model.remark;
            parameters[8].Value = model.enabled;
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
            strSql.Append("delete from plc_config ");
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
            strSql.Append("delete from plc_config ");
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
        public SUNWODA_SEVB.Data.Model.plc_config? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,name,device_id,ip,port,brand_specification_protocal,data_sort_rule,state,remark,enabled from plc_config "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.plc_config model = new SUNWODA_SEVB.Data.Model.plc_config();
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
        public SUNWODA_SEVB.Data.Model.plc_config DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.plc_config model = new SUNWODA_SEVB.Data.Model.plc_config();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["name"] != null)
                {
                    model.name = row["name"].ToString()!;
                }
                if (row["device_id"] != null)
                {
                    model.device_id = row["device_id"].ToString()!;
                }
                if (row["ip"] != null)
                {
                    model.ip = row["ip"].ToString()!;
                }
                if (row["port"] != null)
                {
                    model.port = row["port"].ToString()!;
                }
                if (row["brand_specification_protocal"] != null)
                {
                    model.brand_specification_protocal = row["brand_specification_protocal"].ToString()!;
                }
                if (row["data_sort_rule"] != null)
                {
                    model.data_sort_rule = row["data_sort_rule"].ToString()!;
                }
                if (row["state"] != null && row["state"].ToString() != "")
                {
                    model.state = int.Parse(row["state"].ToString()!);
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString()!;
                }
                if (row["enabled"] != null && row["enabled"].ToString() != "")
                {
                    model.enabled = int.Parse(row["enabled"].ToString()!);
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
                "select id,name,device_id,ip,port,brand_specification_protocal,data_sort_rule,state,remark,enabled "
            );
            strSql.Append(" FROM plc_config ");
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
            strSql.Append("select count(1) FROM plc_config ");
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
            strSql.Append(")AS Row, T.*  from plc_config T ");
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
            parameters[0].Value = "plc_config";
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
