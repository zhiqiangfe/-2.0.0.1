using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:user_define_config
    /// </summary>
    public partial class user_define_config
    {
        public user_define_config() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "user_define_config");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from user_define_config");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32, 11) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.user_define_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_define_config(");
            strSql.Append(
                "id,config_list,state,remark,datatime,cycle,equipment_id,signal_address)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@id,@config_list,@state,@remark,@datatime,@cycle,@equipment_id,@signal_address)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
                new MySqlParameter("@config_list", MySqlDbType.VarChar, 50),
                new MySqlParameter("@state", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
                new MySqlParameter("@datatime", MySqlDbType.DateTime),
                new MySqlParameter("@cycle", MySqlDbType.Int32, 11),
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@signal_address", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.config_list;
            parameters[2].Value = model.state;
            parameters[3].Value = model.remark;
            parameters[4].Value = model.datatime;
            parameters[5].Value = model.cycle;
            parameters[6].Value = model.equipment_id;
            parameters[7].Value = model.signal_address;

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
        public bool Update(SUNWODA_SEVB.Data.Model.user_define_config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_define_config set ");
            strSql.Append("config_list=@config_list,");
            strSql.Append("state=@state,");
            strSql.Append("remark=@remark,");
            strSql.Append("datatime=@datatime,");
            strSql.Append("cycle=@cycle,");
            strSql.Append("equipment_id=@equipment_id,");
            strSql.Append("signal_address=@signal_address");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@config_list", MySqlDbType.VarChar, 50),
                new MySqlParameter("@state", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
                new MySqlParameter("@datatime", MySqlDbType.DateTime),
                new MySqlParameter("@cycle", MySqlDbType.Int32, 11),
                new MySqlParameter("@equipment_id", MySqlDbType.VarChar, 50),
                new MySqlParameter("@signal_address", MySqlDbType.VarChar, 50),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.config_list;
            parameters[1].Value = model.state;
            parameters[2].Value = model.remark;
            parameters[3].Value = model.datatime;
            parameters[4].Value = model.cycle;
            parameters[5].Value = model.equipment_id;
            parameters[6].Value = model.signal_address;
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
            strSql.Append("delete from user_define_config ");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32, 11) };
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
            strSql.Append("delete from user_define_config ");
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
        public SUNWODA_SEVB.Data.Model.user_define_config? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,config_list,state,remark,datatime,cycle,equipment_id,signal_address from user_define_config "
            );
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32, 11) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.user_define_config model = new SUNWODA_SEVB.Data.Model.user_define_config();
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
        public SUNWODA_SEVB.Data.Model.user_define_config DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.user_define_config model = new SUNWODA_SEVB.Data.Model.user_define_config();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["config_list"] != null)
                {
                    model.config_list = row["config_list"].ToString()!;
                }
                if (row["state"] != null)
                {
                    model.state = row["state"].ToString()!;
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString()!;
                }
                if (row["datatime"] != null && row["datatime"].ToString() != "")
                {
                    model.datatime = DateTime.Parse(row["datatime"].ToString()!);
                }
                if (row["cycle"] != null && row["cycle"].ToString() != "")
                {
                    model.cycle = int.Parse(row["cycle"].ToString()!);
                }
                if (row["equipment_id"] != null)
                {
                    model.equipment_id = row["equipment_id"].ToString()!;
                }
                if (row["signal_address"] != null)
                {
                    model.signal_address = row["signal_address"].ToString()!;
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
                "select id,config_list,state,remark,datatime,cycle,equipment_id,signal_address "
            );
            strSql.Append(" FROM user_define_config ");
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
            strSql.Append("select count(1) FROM user_define_config ");
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
            strSql.Append(")AS Row, T.*  from user_define_config T ");
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
            parameters[0].Value = "user_define_config";
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
