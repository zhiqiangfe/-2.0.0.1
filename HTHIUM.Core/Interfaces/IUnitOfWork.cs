using System;
using System.Collections.Generic;
using System.Linq;
using HTHIUM.Core.Interfaces.Data;

namespace HTHIUM.Core.Interfaces
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 开始事务
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// 提交事务
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// 回滚事务
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// 获取仓储
        /// </summary>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, new();
    }
}
