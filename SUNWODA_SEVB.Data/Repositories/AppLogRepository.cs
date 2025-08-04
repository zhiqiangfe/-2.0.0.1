using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models;
using SUNWODA_SEVB.Data.Models;
using Mapster;

namespace SUNWODA_SEVB.Data.Repositories
{
    public class AppLogRepository : MappingRepository<AppLogModel, AppLog>, IAppLogRepository
    {
        public AppLogRepository(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<bool> BulkInsertAsync(List<AppLogModel> logs)
        {
            if (logs == null || logs.Count == 0) return true;

            try
            {
                var models = logs.Adapt<List<AppLog>>();

                // 使用 SqlSugar 的批量插入
                var result = await _db.Insertable(models)
                    .ExecuteCommandAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出，避免影响应用运行
                NLog.LogManager.GetCurrentClassLogger()
                    .Error(ex, $"批量插入日志失败，日志数量：{logs.Count}");
                return false;
            }
        }

        public async Task<int> DeleteOldLogsAsync(int daysToKeep)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                return await _db.Deleteable<AppLog>()
                    .Where(log => log.LogTime < cutoffDate)
                    .ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger()
                    .Error(ex, "删除过期日志失败");
                return 0;
            }
        }
    }
}
