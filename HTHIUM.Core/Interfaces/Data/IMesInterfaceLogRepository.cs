using HTHIUM.Core.Models.Data;

namespace HTHIUM.Core.Interfaces.Data
{
    /// <summary>
    /// MES接口日志仓储接口
    /// </summary>
    public interface IMesInterfaceLogRepository : IRepository<MesInterfaceLogModel>
    {
        /// <summary>
        /// 批量插入日志
        /// </summary>
        Task<bool> BulkInsertAsync(List<MesInterfaceLogModel> logs);

        /// <summary>
        /// 删除过期日志
        /// </summary>
        Task<int> DeleteOldLogsAsync(int daysToKeep);
    }
}
