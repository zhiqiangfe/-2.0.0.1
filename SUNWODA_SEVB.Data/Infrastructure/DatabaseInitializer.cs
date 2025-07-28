using MySql.Data.MySqlClient;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Data.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.Infrastructure
{
    public class DatabaseInitializer
    {
        private static bool _isInitialized = false;
        private static readonly object _lock = new object();

        public static bool Initialize()
        {
            if (_isInitialized)
                return true;

            lock (_lock)
            {
                if (_isInitialized)
                    return true;

                try
                {
                    string connectionString = ConfigurationHelper.GetConnectionString();

                    // 设置DbHelperMySQL的连接字符串
                    DbHelperMySQL.connectionString = connectionString;

                    // 测试连接
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        _isInitialized = true;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"数据库初始化失败: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
