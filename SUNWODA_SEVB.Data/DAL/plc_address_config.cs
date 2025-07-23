using Maticsoft.DBUtility;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.DAL
{
    public partial class plc_address_config
    {
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "plc_address_config");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from plc_address_config");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.plc_address_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into plc_address_config(");
            strSql.Append(
                "plc_config_id,plc_rw_config_id,category_id,parameter_name,type,length,address,unit,remark,is_monitor)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@plc_config_id,@plc_rw_config_id,@category_id,@parameter_name,@type,@length,@address,@unit,@remark,@is_monitor)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_rw_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@category_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@parameter_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@length", MySqlDbType.Int32, 11),
                new MySqlParameter("@address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@unit", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@is_monitor", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.plc_config_id;
            parameters[1].Value = model.plc_rw_config_id;
            parameters[2].Value = model.category_id;
            parameters[3].Value = model.parameter_name;
            parameters[4].Value = model.type;
            parameters[5].Value = model.length;
            parameters[6].Value = model.address;
            parameters[7].Value = model.unit;
            parameters[8].Value = model.remark;
            parameters[9].Value = model.is_monitor;

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
        public bool Update(SUNWODA_SEVB.Data.Model.plc_address_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update plc_address_config set ");
            strSql.Append("plc_config_id=@plc_config_id,");
            strSql.Append("plc_rw_config_id=@plc_rw_config_id,");
            strSql.Append("category_id=@category_id,");
            strSql.Append("parameter_name=@parameter_name,");
            strSql.Append("type=@type,");
            strSql.Append("length=@length,");
            strSql.Append("address=@address,");
            strSql.Append("unit=@unit,");
            strSql.Append("remark=@remark,");
            strSql.Append("is_monitor=@is_monitor,");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@plc_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@plc_rw_config_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@category_id", MySqlDbType.Int32, 11),
                new MySqlParameter("@parameter_name", MySqlDbType.VarChar, 50),
                new MySqlParameter("@type", MySqlDbType.VarChar, 50),
                new MySqlParameter("@length", MySqlDbType.Int32, 11),
                new MySqlParameter("@address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@unit", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 50),
                new MySqlParameter("@is_monitor", MySqlDbType.Int32, 11),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.plc_config_id;
            parameters[1].Value = model.plc_rw_config_id;
            parameters[2].Value = model.category_id;
            parameters[3].Value = model.parameter_name;
            parameters[4].Value = model.type;
            parameters[5].Value = model.length;
            parameters[6].Value = model.address;
            parameters[7].Value = model.unit;
            parameters[8].Value = model.remark;
            parameters[9].Value = model.is_monitor;
            parameters[10].Value = model.id;

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
            strSql.Append("delete from plc_address_config ");
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
            strSql.Append("delete from plc_address_config ");
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
        public SUNWODA_SEVB.Data.Model.plc_address_config? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,plc_config_id,plc_rw_config_id,category_id,parameter_name,type,length,address,unit,remark,is_monitor from plc_address_config "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.plc_address_config model = new SUNWODA_SEVB.Data.Model.plc_address_config();
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
        public SUNWODA_SEVB.Data.Model.plc_address_config DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.plc_address_config model = new SUNWODA_SEVB.Data.Model.plc_address_config();
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
                if (row["plc_rw_config_id"] != null && row["plc_rw_config_id"].ToString() != "")
                {
                    model.plc_rw_config_id = int.Parse(row["plc_rw_config_id"].ToString()!);
                }
                if (row["category_id"] != null && row["category_id"].ToString() != "")
                {
                    model.category_id = int.Parse(row["category_id"].ToString()!);
                }
                if (row["parameter_name"] != null)
                {
                    model.parameter_name = row["parameter_name"].ToString()!;
                }
                if (row["type"] != null)
                {
                    model.type = row["type"].ToString()!;
                }
                if (row["length"] != null && row["length"].ToString() != "")
                {
                    model.length = int.Parse(row["length"].ToString()!);
                }
                if (row["address"] != null)
                {
                    model.address = row["address"].ToString()!;
                }
                if (row["unit"] != null)
                {
                    model.unit = row["unit"].ToString()!;
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString()!;
                }
                if (row["is_monitor"] != null && row["is_monitor"].ToString() != "")
                {
                    model.is_monitor = int.Parse(row["is_monitor"].ToString()!);
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
                "select id,plc_config_id,plc_rw_config_id,category_id,parameter_name,type,length,address,unit,remark,is_monitor "
            );
            strSql.Append(" FROM plc_address_config ");
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
            strSql.Append("select count(1) FROM plc_address_config ");
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
            strSql.Append(")AS Row, T.*  from plc_address_config T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQL.Query(strSql.ToString());
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}
