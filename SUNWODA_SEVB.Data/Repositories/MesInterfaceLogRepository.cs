

using Mapster;
using SqlSugar;
using SUNWODA_SEVB.Core.Entities;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Data.Models;

namespace SUNWODA_SEVB.Data.Repositories
{
    public class MesInterfaceLogRepository : MappingRepository<Core.Entities.MesInterfaceLogModel, Models.MesInterfaceLog>, IMesInterfaceLogRepository
    {
        public MesInterfaceLogRepository(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<bool> BulkInsertAsync(List<Core.Entities.MesInterfaceLogModel> logs)
        {
            if (logs == null || logs.Count == 0) return true;

            var models = logs.Adapt<List<Models.MesInterfaceLog>>();
            return await _db.Insertable(models).ExecuteCommandAsync() > 0;
        }

        public async Task<int> DeleteOldLogsAsync(int daysToKeep)
        {
            var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
            return await _db.Deleteable<Models.MesInterfaceLog>()
                .Where(log => log.StartTime < cutoffDate)
                .ExecuteCommandAsync();
        }
    }
}
