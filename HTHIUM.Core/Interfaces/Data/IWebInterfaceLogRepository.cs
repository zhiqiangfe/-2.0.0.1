using HTHIUM.Core.Models.Data;

namespace HTHIUM.Core.Interfaces.Data
{
    /// <summary>
    /// Web接口日志仓储接口
    /// </summary>
    public interface IWebInterfaceLogRepository : IRepository<WebInterfaceLogModel>
    {
        /// <summary>
        /// 批量插入日志
        /// </summary>
        Task<bool> BulkInsertAsync(List<WebInterfaceLogModel> logs);

        /// <summary>
        /// 删除过期日志
        /// </summary>
        Task<int> DeleteOldLogsAsync(int daysToKeep);
    }
}
