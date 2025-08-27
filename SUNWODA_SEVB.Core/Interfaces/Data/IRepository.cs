using System.Linq.Expressions;

namespace SUNWODA_SEVB.Core.Interfaces.Data
{
    /// <summary>
    /// 仓储基础接口
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        // 查询
        Task<TEntity?> GetByIdAsync(object id);
        TEntity? GetById(object id);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity? Get(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
        List<TEntity> GetAll();
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
        Task<(List<TEntity> Items, int Total)> GetPagedAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            int pageIndex = 1,
            int pageSize = 20,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool isDesc = true);
        (List<TEntity> Items, int Total) GetPaged(
            Expression<Func<TEntity, bool>>? predicate = null,
            int pageIndex = 1,
            int pageSize = 20,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool isDesc = true);

        // 添加
        Task<bool> AddAsync(TEntity entity);
        bool Add(TEntity entity);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
        bool AddRange(IEnumerable<TEntity> entities);

        // 更新
        Task<bool> UpdateAsync(TEntity entity);
        bool Update(TEntity entity);
        Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities);
        bool UpdateRange(IEnumerable<TEntity> entities);

        // 删除
        Task<bool> DeleteAsync(TEntity entity);
        bool Delete(TEntity entity);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        bool Delete(Expression<Func<TEntity, bool>> predicate);
        Task<bool> DeleteByIdAsync(object id);
        bool DeleteById(object id);

        // 存在判断
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
        int Count(Expression<Func<TEntity, bool>>? predicate = null);
    }
}
