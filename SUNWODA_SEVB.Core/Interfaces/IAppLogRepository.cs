
using SUNWODA_SEVB.Core.Models;

namespace SUNWODA_SEVB.Core.Interfaces
{
    /// <summary>
    /// 应用日志仓储接口
    /// </summary>
    public interface IAppLogRepository : IRepository<AppLogModel>
    {
        /// <summary>
        /// 批量插入日志
        /// </summary>
        Task<bool> BulkInsertAsync(List<AppLogModel> logs);
        /// <summary>
        /// 删除过期日志
        /// </summary>
        Task<int> DeleteOldLogsAsync(int daysToKeep);

    }
}
