using Mapster;
using SqlSugar;
using HTHIUM.Core.Interfaces.Data;
using HTHIUM.Core.Models.Data;
using HTHIUM.Data.Models;

namespace HTHIUM.Data.Repositories
{
    public class WebInterfaceLogRepository : MappingRepository<WebInterfaceLogModel, WebInterfaceLog>, IWebInterfaceLogRepository
    {
        public WebInterfaceLogRepository(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<bool> BulkInsertAsync(List<WebInterfaceLogModel> logs)
        {
            if (logs == null || logs.Count == 0) return true;

            var models = logs.Adapt<List<WebInterfaceLog>>();
            return await _db.Insertable(models).ExecuteCommandAsync() > 0;
        }

        public async Task<int> DeleteOldLogsAsync(int daysToKeep)
        {
            var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
            return await _db.Deleteable<WebInterfaceLog>()
                .Where(log => log.LogDate < cutoffDate)
                .ExecuteCommandAsync();
        }
    }
}
