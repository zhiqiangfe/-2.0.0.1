
using SUNWODA_SEVB.Core.Services;
using SUNWODA_SEVB.Data.DBUtility;
using SUNWODA_SEVB.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ILoggerService<DatabaseService> _logger;
        private readonly IDbHelperMySQL _dbHelper;

        public DatabaseService(ILoggerService<DatabaseService> logger, IDbHelperMySQL dbHelper)
        {
            _logger = logger;
            _dbHelper = dbHelper;
        }

        public bool Initialize()
        {
            try
            {
                _logger.Info("开始初始化数据库连接...");

                bool result = TestConnection();

                if (result)
                {
                    _logger.Info("数据库连接初始化成功");
                }
                else
                {
                    _logger.Error("数据库连接初始化失败");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.ErrorException( "数据库初始化过程中发生异常:",ex);
                return false;
            }
        }

        public bool TestConnection()
        {
            try
            {
                var result = _dbHelper.GetSingle("SELECT 1");
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.ErrorException( "测试数据库连接时发生异常", ex);
                return false;
            }
        }
    }
}
