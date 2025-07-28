using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:plc_rw_config
    /// </summary>
    public partial class plc_rw_config
    {
        private readonly DbHelperMySQL _dbHelper;

        public plc_rw_config(DbHelperMySQL dbHelper) { _dbHelper = dbHelper; }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return _dbHelper.GetMaxID("id", "plc_rw_config");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from plc_rw_config");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return _dbHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.plc_rw_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into plc_rw_config(");
            strSql.Append(
                "name,plc_config_id,area_name,start_address,length,rw,cycle,last_time,enabled,address_type)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@name,@plc_config_id,@area_name,@start_address,@length,@rw,@cycle,@last_time,@enabled,@address_type)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@area_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@start_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@length", MySqlDbType.Int32, 11),
                new MySqlParameter("@rw", MySqlDbType.VarChar, 20),
                new MySqlParameter("@cycle", MySqlDbType.Int32, 11),
                new MySqlParameter("@last_time", MySqlDbType.DateTime),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@address_type", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.name;
            parameters[1].Value = model.plc_config_id;
            parameters[2].Value = model.area_name;
            parameters[3].Value = model.start_address;
            parameters[4].Value = model.length;
            parameters[5].Value = model.rw;
            parameters[6].Value = model.cycle;
            parameters[7].Value = model.last_time;
            parameters[8].Value = model.enabled;
            parameters[9].Value = model.address_type;

            int rows = _dbHelper.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Update(SUNWODA_SEVB.Data.Model.plc_rw_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update plc_rw_config set ");
            strSql.Append("name=@name,");
            strSql.Append("plc_config_id=@plc_config_id,");
            strSql.Append("area_name=@area_name,");
            strSql.Append("start_address=@start_address,");
            strSql.Append("length=@length,");
            strSql.Append("rw=@rw,");
            strSql.Append("cycle=@cycle,");
            strSql.Append("last_time=@last_time,");
            strSql.Append("enabled=@enabled,");
            strSql.Append("address_type=@address_type");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@area_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@start_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@length", MySqlDbType.Int32, 11),
                new MySqlParameter("@rw", MySqlDbType.VarChar, 20),
                new MySqlParameter("@cycle", MySqlDbType.Int32, 11),
                new MySqlParameter("@last_time", MySqlDbType.DateTime),
                new MySqlParameter("@enabled", MySqlDbType.Int32, 11),
                new MySqlParameter("@address_type", MySqlDbType.Int32, 11),
                new MySqlParameter("@id", MySqlDbType.Int32, 10),
            };
            parameters[0].Value = model.name;
            parameters[1].Value = model.plc_config_id;
            parameters[2].Value = model.area_name;
            parameters[3].Value = model.start_address;
            parameters[4].Value = model.length;
            parameters[5].Value = model.rw;
            parameters[6].Value = model.cycle;
            parameters[7].Value = model.last_time;
            parameters[8].Value = model.enabled;
            parameters[9].Value = model.address_type;
            parameters[10].Value = model.id;

            int rows = _dbHelper.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append("delete from plc_rw_config ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            int rows = _dbHelper.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append("delete from plc_rw_config ");
            strSql.Append(" where id in (" + idlist + ")  ");
            int rows = _dbHelper.ExecuteSql(strSql.ToString());
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
        public SUNWODA_SEVB.Data.Model.plc_rw_config? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,name,plc_config_id,area_name,start_address,length,rw,cycle,last_time,enabled,address_type from plc_rw_config "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.plc_rw_config model = new SUNWODA_SEVB.Data.Model.plc_rw_config();
            DataSet ds = _dbHelper.Query(strSql.ToString(), parameters);
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
        public SUNWODA_SEVB.Data.Model.plc_rw_config DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.plc_rw_config model = new SUNWODA_SEVB.Data.Model.plc_rw_config();
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
                if (row["plc_config_id"] != null && row["plc_config_id"].ToString() != "")
                {
                    model.plc_config_id = int.Parse(row["plc_config_id"].ToString()!);
                }
                if (row["area_name"] != null)
                {
                    model.area_name = row["area_name"].ToString()!;
                }
                if (row["start_address"] != null)
                {
                    model.start_address = row["start_address"].ToString()!;
                }
                if (row["length"] != null && row["length"].ToString() != "")
                {
                    model.length = int.Parse(row["length"].ToString()!);
                }
                if (row["rw"] != null)
                {
                    model.rw = row["rw"].ToString()!;
                }
                if (row["cycle"] != null && row["cycle"].ToString() != "")
                {
                    model.cycle = int.Parse(row["cycle"].ToString()!);
                }
                if (row["last_time"] != null && row["last_time"].ToString() != "")
                {
                    model.last_time = DateTime.Parse(row["last_time"].ToString()!);
                }
                if (row["enabled"] != null && row["enabled"].ToString() != "")
                {
                    model.enabled = int.Parse(row["enabled"].ToString()!);
                }
                if (row["address_type"] != null && row["address_type"].ToString() != "")
                {
                    model.address_type = int.Parse(row["address_type"].ToString()!);
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
                "select id,name,plc_config_id,area_name,start_address,length,rw,cycle,last_time,enabled,address_type "
            );
            strSql.Append(" FROM plc_rw_config ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return _dbHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM plc_rw_config ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object? obj = _dbHelper.GetSingle(strSql.ToString());
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
            strSql.Append(")AS Row, T.*  from plc_rw_config T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return _dbHelper.Query(strSql.ToString());
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
            parameters[0].Value = "plc_rw_config";
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
