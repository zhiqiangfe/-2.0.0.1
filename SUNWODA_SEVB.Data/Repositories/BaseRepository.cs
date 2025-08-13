using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using System.Linq.Expressions;
using Mapster;

namespace SUNWODA_SEVB.Data.Repositories
{
    /// <summary>
    /// 数据库的增删改查
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly ISqlSugarClient _db;

        public BaseRepository(ISqlSugarClient db)
        {
            _db = db;
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _db.Queryable<TEntity>().InSingleAsync(id);
        }

        public TEntity? GetById(object id)
        {
            return _db.Queryable<TEntity>().InSingle(id);
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Queryable<TEntity>().FirstAsync(predicate);
        }

        public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Queryable<TEntity>().First(predicate);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _db.Queryable<TEntity>().ToListAsync();
        }

        public List<TEntity> GetAll()
        {
            return _db.Queryable<TEntity>().ToList();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Queryable<TEntity>().Where(predicate).ToListAsync();
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Queryable<TEntity>().Where(predicate).ToList();
        }

        public async Task<(List<TEntity> Items, int Total)> GetPagedAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            int pageIndex = 1,
            int pageSize = 20,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool isDesc = true)
        {
            RefAsync<int> total = 0;
            var query = _db.Queryable<TEntity>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
            {
                query = isDesc ? query.OrderBy(orderBy, OrderByType.Desc) : query.OrderBy(orderBy, OrderByType.Asc);
            }

            var items = await query.ToPageListAsync(pageIndex, pageSize, total);
            return (items, total.Value);
        }

        public (List<TEntity> Items, int Total) GetPaged(
            Expression<Func<TEntity, bool>>? predicate = null,
            int pageIndex = 1,
            int pageSize = 20,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool isDesc = true)
        {
            int total = 0;
            var query = _db.Queryable<TEntity>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
            {
                query = isDesc ? query.OrderBy(orderBy, OrderByType.Desc) : query.OrderBy(orderBy, OrderByType.Asc);
            }

            var items = query.ToPageList(pageIndex, pageSize, ref total);
            return (items, total);
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            return await _db.Insertable(entity).ExecuteCommandAsync() > 0;
        }

        public bool Add(TEntity entity)
        {
            return _db.Insertable(entity).ExecuteCommand() > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return await _db.Insertable(entities.ToList()).ExecuteCommandAsync() > 0;
        }

        public bool AddRange(IEnumerable<TEntity> entities)
        {
            return _db.Insertable(entities.ToList()).ExecuteCommand() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await _db.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        public bool Update(TEntity entity)
        {
            return _db.Updateable(entity).ExecuteCommand() > 0;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            return await _db.Updateable(entities.ToList()).ExecuteCommandAsync() > 0;
        }

        public bool UpdateRange(IEnumerable<TEntity> entities)
        {
            return _db.Updateable(entities.ToList()).ExecuteCommand() > 0;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandAsync() > 0;
        }

        public bool Delete(TEntity entity)
        {
            return _db.Deleteable(entity).ExecuteCommand() > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Deleteable<TEntity>().Where(predicate).ExecuteCommandAsync() > 0;
        }

        public bool Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Deleteable<TEntity>().Where(predicate).ExecuteCommand() > 0;
        }

        public async Task<bool> DeleteByIdAsync(object id)
        {
            return await _db.Deleteable<TEntity>().In(id).ExecuteCommandAsync() > 0;
        }

        public bool DeleteById(object id)
        {
            return _db.Deleteable<TEntity>().In(id).ExecuteCommand() > 0;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Queryable<TEntity>().AnyAsync(predicate);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Queryable<TEntity>().Any(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null
                ? await _db.Queryable<TEntity>().CountAsync()
                : await _db.Queryable<TEntity>().CountAsync(predicate);
        }

        public int Count(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate == null
                ? _db.Queryable<TEntity>().Count()
                : _db.Queryable<TEntity>().Count(predicate);
        }
    }
}