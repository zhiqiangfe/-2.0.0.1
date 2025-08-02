using System;

namespace SUNWODA_SEVB.Core.Interfaces
{
    /// <summary>
    /// 数据库服务接口
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        bool Initialize();

        /// <summary>
        /// 备份数据库
        /// </summary>
        void Backup(string backupPath);

        /// <summary>
        /// 恢复数据库
        /// </summary>
        void Restore(string backupPath);
    }
}
