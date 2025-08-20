using SUNWODA_SEVB.Core.Models.Data;

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

        /// <summary>
        /// 获取数据库大小（MB）
        /// </summary>
        Task<double> GetDatabaseSizeAsync();

        /// <summary>
        /// 按大小删除日志，删除到指定大小以下
        /// </summary>
        /// <param name="targetSizeMB">目标大小（MB）</param>
        /// <returns>删除的记录数</returns>
        Task<int> DeleteLogsBySize(double targetSizeMB);
    }
}
