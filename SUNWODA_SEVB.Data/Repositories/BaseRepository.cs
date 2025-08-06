using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using System.Linq.Expressions;

namespace SUNWODA_SEVB.Data.Repositories
{
    /// <summary>
    /// 数据库的增删改查（基础版无映射）
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class BaseRepository<TModel> : IRepository<TModel> where TModel : class, new()
    {
        protected readonly ISqlSugarClient _db;

        public BaseRepository(ISqlSugarClient db)
        {
            _db = db;
        }

        public async Task<TModel?> GetByIdAsync(object id)
        {
            return await _db.Queryable<TModel>().InSingleAsync(id);
        }

        public async Task<TModel?> GetAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await _db.Queryable<TModel>().FirstAsync(predicate);
        }

        public async Task<List<TModel>> GetAllAsync()
        {
            return await _db.Queryable<TModel>().ToListAsync();
        }

        public async Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await _db.Queryable<TModel>().Where(predicate).ToListAsync();
        }

        public async Task<(List<TModel> Items, int Total)> GetPagedAsync(
            Expression<Func<TModel, bool>>? predicate = null,
            int pageIndex = 1,
            int pageSize = 20,
            Expression<Func<TModel, object>>? orderBy = null,
            bool isDesc = true)
        {
            RefAsync<int> total = 0;
            var query = _db.Queryable<TModel>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
            {
                query = isDesc ? query.OrderBy(orderBy, OrderByType.Desc) : query.OrderBy(orderBy, OrderByType.Asc);
            }

            var items = await query.ToPageListAsync(pageIndex, pageSize, total);
            return (items, total.Value);
        }

        public async Task<bool> AddAsync(TModel entity)
        {
            return await _db.Insertable(entity).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TModel> entities)
        {
            return await _db.Insertable(entities.ToList()).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TModel entity)
        {
            return await _db.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<TModel> entities)
        {
            return await _db.Updateable(entities.ToList()).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteAsync(TModel entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await _db.Deleteable<TModel>().Where(predicate).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(object id)
        {
            return await _db.Deleteable<TModel>().In(id).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await _db.Queryable<TModel>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TModel, bool>>? predicate = null)
        {
            return predicate == null
                ? await _db.Queryable<TModel>().CountAsync()
                : await _db.Queryable<TModel>().CountAsync(predicate);
        }
    }
}