using Mapster;
using SqlSugar;
using HTHIUM.Core.Interfaces.Data;
using HTHIUM.Core.Models.Data;
using HTHIUM.Data.Models;

namespace HTHIUM.Data.Repositories
{
    public class MesInterfaceLogRepository : MappingRepository<MesInterfaceLogModel, MesInterfaceLog>, IMesInterfaceLogRepository
    {
        public MesInterfaceLogRepository(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<bool> BulkInsertAsync(List<MesInterfaceLogModel> logs)
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
