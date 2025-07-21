using System;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;
using MySql.Data.MySqlClient;

namespace SUNWODA_SEVB.Data.DAL
{
    public partial class device_instation_data
    {
        public device_instation_data() { }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "device_instation_data");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string barcode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_instation_data");
            strSql.Append(" where barcode=@barcode");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@barcode", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = barcode;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_instation_data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_instation_data(");
            strSql.Append("barcode,instationtime,remark)");
            strSql.Append(" values (");
            strSql.Append("@barcode,@instationtime,@remark)");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@barcode", MySqlDbType.VarChar, 50),
                new MySqlParameter("@instationtime", MySqlDbType.DateTime),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
            };
            parameters[0].Value = model.barcode;
            parameters[1].Value = model.instationtime;
            parameters[2].Value = model.remark;

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
        /// 得到一个对象实体
        /// </summary>
        public SUNWODA_SEVB.Data.Model.device_instation_data? GetModel(string barcode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from device_instation_data ");
            strSql.Append(" where barcode=@barcode");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@barcode", MySqlDbType.VarChar, 50),
            };
            parameters[0].Value = barcode;

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
        public SUNWODA_SEVB.Data.Model.device_instation_data DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_instation_data model = new SUNWODA_SEVB.Data.Model.device_instation_data();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["barcode"] != null)
                {
                    model.barcode = row["barcode"].ToString()!;
                }
                if (row["instationtime"] != null && row["instationtime"].ToString() != "")
                {
                    model.instationtime = DateTime.Parse(row["instationtime"].ToString()!);
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString()!;
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
            strSql.Append("select id,barcode,instationtime,remark ");
            strSql.Append(" FROM device_instation_data ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }
    }
}
