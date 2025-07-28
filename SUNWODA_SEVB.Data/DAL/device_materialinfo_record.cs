using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Data.DBUtility;

namespace SUNWODA_SEVB.Data.DAL
{
    /// <summary>
    /// 数据访问类:device_materialinfo_record
    /// </summary>
    public partial class device_materialinfo_record
    {
        public device_materialinfo_record() { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "device_materialinfo_record");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from device_materialinfo_record");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SUNWODA_SEVB.Data.Model.device_materialinfo_record model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into device_materialinfo_record(");
            strSql.Append(
                "material_Code,A_OR_B,model,person_code,grade,product_length,speed,weight_cpk,weight_cov,weight_mean,weight_sigma,weight_rate,size_cpk,size_mean,size_sigma,size_rate,start_time,end_time,use_time,remark)"
            );
            strSql.Append(" values (");
            strSql.Append(
                "@material_Code,@A_OR_B,@model,@person_code,@grade,@product_length,@speed,@weight_cpk,@weight_cov,@weight_mean,@weight_sigma,@weight_rate,@size_cpk,@size_mean,@size_sigma,@size_rate,@start_time,@end_time,@use_time,@remark)"
            );
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@material_Code", MySqlDbType.VarChar, 50),
                new MySqlParameter("@A_OR_B", MySqlDbType.VarChar, 10),
                new MySqlParameter("@model", MySqlDbType.VarChar, 50),
                new MySqlParameter("@person_code", MySqlDbType.VarChar, 50),
                new MySqlParameter("@grade", MySqlDbType.VarChar, 5),
                new MySqlParameter("@product_length", MySqlDbType.VarChar, 50),
                new MySqlParameter("@speed", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_cpk", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_cov", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_mean", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_sigma", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_rate", MySqlDbType.VarChar, 50),
                new MySqlParameter("@size_cpk", MySqlDbType.VarChar, 50),
                new MySqlParameter("@size_mean", MySqlDbType.VarChar, 50),
                new MySqlParameter("@size_sigma", MySqlDbType.VarChar, 50),
                new MySqlParameter("@size_rate", MySqlDbType.VarChar, 50),
                new MySqlParameter("@start_time", MySqlDbType.DateTime),
                new MySqlParameter("@end_time", MySqlDbType.DateTime),
                new MySqlParameter("@use_time", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
            };
            parameters[0].Value = model.material_Code;
            parameters[1].Value = model.A_OR_B;
            parameters[2].Value = model.model;
            parameters[3].Value = model.person_code;
            parameters[4].Value = model.grade;
            parameters[5].Value = model.product_length;
            parameters[6].Value = model.speed;
            parameters[7].Value = model.weight_cpk;
            parameters[8].Value = model.weight_cov;
            parameters[9].Value = model.weight_mean;
            parameters[10].Value = model.weight_sigma;
            parameters[11].Value = model.weight_rate;
            parameters[12].Value = model.size_cpk;
            parameters[13].Value = model.size_mean;
            parameters[14].Value = model.size_sigma;
            parameters[15].Value = model.size_rate;
            parameters[16].Value = model.start_time;
            parameters[17].Value = model.end_time;
            parameters[18].Value = model.use_time;
            parameters[19].Value = model.remark;

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
        public bool Update(SUNWODA_SEVB.Data.Model.device_materialinfo_record model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update device_materialinfo_record set ");
            strSql.Append("material_Code=@material_Code,");
            strSql.Append("A_OR_B=@A_OR_B,");
            strSql.Append("model=@model,");
            strSql.Append("person_code=@person_code,");
            strSql.Append("grade=@grade,");
            strSql.Append("product_length=@product_length,");
            strSql.Append("speed=@speed,");
            strSql.Append("weight_cpk=@weight_cpk,");
            strSql.Append("weight_cov=@weight_cov,");
            strSql.Append("weight_mean=@weight_mean,");
            strSql.Append("weight_sigma=@weight_sigma,");
            strSql.Append("weight_rate=@weight_rate,");
            strSql.Append("size_cpk=@size_cpk,");
            strSql.Append("size_mean=@size_mean,");
            strSql.Append("size_sigma=@size_sigma,");
            strSql.Append("size_rate=@size_rate,");
            strSql.Append("start_time=@start_time,");
            strSql.Append("end_time=@end_time,");
            strSql.Append("use_time=@use_time,");
            strSql.Append("remark=@remark");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@material_Code", MySqlDbType.VarChar, 50),
                new MySqlParameter("@A_OR_B", MySqlDbType.VarChar, 10),
                new MySqlParameter("@model", MySqlDbType.VarChar, 50),
                new MySqlParameter("@person_code", MySqlDbType.VarChar, 50),
                new MySqlParameter("@grade", MySqlDbType.VarChar, 5),
                new MySqlParameter("@product_length", MySqlDbType.VarChar, 50),
                new MySqlParameter("@speed", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_cpk", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_cov", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_mean", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_sigma", MySqlDbType.VarChar, 50),
                new MySqlParameter("@weight_rate", MySqlDbType.VarChar, 50),
                new MySqlParameter("@size_cpk", MySqlDbType.VarChar, 50),
                new MySqlParameter("@size_mean", MySqlDbType.VarChar, 50),
                new MySqlParameter("@size_sigma", MySqlDbType.VarChar, 50),
                new MySqlParameter("@size_rate", MySqlDbType.VarChar, 50),
                new MySqlParameter("@start_time", MySqlDbType.DateTime),
                new MySqlParameter("@end_time", MySqlDbType.DateTime),
                new MySqlParameter("@use_time", MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 100),
                new MySqlParameter("@id", MySqlDbType.Int32, 11),
            };
            parameters[0].Value = model.material_Code;
            parameters[1].Value = model.A_OR_B;
            parameters[2].Value = model.model;
            parameters[3].Value = model.person_code;
            parameters[4].Value = model.grade;
            parameters[5].Value = model.product_length;
            parameters[6].Value = model.speed;
            parameters[7].Value = model.weight_cpk;
            parameters[8].Value = model.weight_cov;
            parameters[9].Value = model.weight_mean;
            parameters[10].Value = model.weight_sigma;
            parameters[11].Value = model.weight_rate;
            parameters[12].Value = model.size_cpk;
            parameters[13].Value = model.size_mean;
            parameters[14].Value = model.size_sigma;
            parameters[15].Value = model.size_rate;
            parameters[16].Value = model.start_time;
            parameters[17].Value = model.end_time;
            parameters[18].Value = model.use_time;
            parameters[19].Value = model.remark;
            parameters[20].Value = model.id;

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
            strSql.Append("delete from device_materialinfo_record ");
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
            strSql.Append("delete from device_materialinfo_record ");
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
        public SUNWODA_SEVB.Data.Model.device_materialinfo_record? GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,material_Code,A_OR_B,model,person_code,grade,product_length,speed,weight_cpk,weight_cov,weight_mean,weight_sigma,weight_rate,size_cpk,size_mean,size_sigma,size_rate,start_time,end_time,use_time,remark from device_materialinfo_record "
            );
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = { new MySqlParameter("@id", MySqlDbType.Int32) };
            parameters[0].Value = id;

            SUNWODA_SEVB.Data.Model.device_materialinfo_record model =
                new SUNWODA_SEVB.Data.Model.device_materialinfo_record();
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
        public SUNWODA_SEVB.Data.Model.device_materialinfo_record DataRowToModel(DataRow row)
        {
            SUNWODA_SEVB.Data.Model.device_materialinfo_record model =
                new SUNWODA_SEVB.Data.Model.device_materialinfo_record();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString()!);
                }
                if (row["material_Code"] != null)
                {
                    model.material_Code = row["material_Code"].ToString();
                }
                if (row["A_OR_B"] != null)
                {
                    model.A_OR_B = row["A_OR_B"].ToString();
                }
                if (row["model"] != null)
                {
                    model.model = row["model"].ToString();
                }
                if (row["person_code"] != null)
                {
                    model.person_code = row["person_code"].ToString();
                }
                if (row["grade"] != null)
                {
                    model.grade = row["grade"].ToString();
                }
                if (row["product_length"] != null)
                {
                    model.product_length = row["product_length"].ToString();
                }
                if (row["speed"] != null)
                {
                    model.speed = row["speed"].ToString();
                }
                if (row["weight_cpk"] != null)
                {
                    model.weight_cpk = row["weight_cpk"].ToString();
                }
                if (row["weight_cov"] != null)
                {
                    model.weight_cov = row["weight_cov"].ToString();
                }
                if (row["weight_mean"] != null)
                {
                    model.weight_mean = row["weight_mean"].ToString();
                }
                if (row["weight_sigma"] != null)
                {
                    model.weight_sigma = row["weight_sigma"].ToString();
                }
                if (row["weight_rate"] != null)
                {
                    model.weight_rate = row["weight_rate"].ToString();
                }
                if (row["size_cpk"] != null)
                {
                    model.size_cpk = row["size_cpk"].ToString();
                }
                if (row["size_mean"] != null)
                {
                    model.size_mean = row["size_mean"].ToString();
                }
                if (row["size_sigma"] != null)
                {
                    model.size_sigma = row["size_sigma"].ToString();
                }
                if (row["size_rate"] != null)
                {
                    model.size_rate = row["size_rate"].ToString();
                }
                if (row["start_time"] != null && row["start_time"].ToString() != "")
                {
                    model.start_time = DateTime.Parse(row["start_time"].ToString()!);
                }
                if (row["end_time"] != null && row["end_time"].ToString() != "")
                {
                    model.end_time = DateTime.Parse(row["end_time"].ToString()!);
                }
                if (row["use_time"] != null)
                {
                    model.use_time = row["use_time"].ToString();
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString();
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
                "select id,material_Code,A_OR_B,model,person_code,grade,product_length,speed,weight_cpk,weight_cov,weight_mean,weight_sigma,weight_rate,size_cpk,size_mean,size_sigma,size_rate,start_time,end_time,use_time,remark "
            );
            strSql.Append(" FROM device_materialinfo_record ");
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
            strSql.Append("select count(1) FROM device_materialinfo_record ");
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
            strSql.Append(")AS Row, T.*  from device_materialinfo_record T ");
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
            parameters[0].Value = "device_materialinfo_record";
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
