using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.DBUtility
{
    /// <summary>
    /// 对外暴露的最小数据库操作接口；需要更多方法自己往里加
    /// </summary>
    public interface IDbHelperMySQL
    {
        int ExecuteSql(string sql);
        int ExecuteSql(string sql, params MySqlParameter[] parameters);
        object? GetSingle(string sql);
        DataSet Query(string sql);
        DataSet Query(string sql, params MySqlParameter[] parameters);
        int ExecuteSqlTran(List<string> sqlList);
        int ExecuteSqlTran(List<CommandInfo> cmdList);
    }
}
