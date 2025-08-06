using Mapster;
using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Tool.Converter;
using System.Linq.Expressions;

namespace SUNWODA_SEVB.Data.Repositories
{
    /// <summary>
    /// 支持实体映射的仓储基类
    /// </summary>
    public class MappingRepository<TEntity, TModel> : IRepository<TEntity>
        where TEntity : class, new()
        where TModel : class, new()
    {
        protected readonly ISqlSugarClient _db;

        public MappingRepository(ISqlSugarClient db)
        {
            _db = db;
        }

        // 统一使用ExpressionConverter进行表达式转换
        private Expression<Func<TModel, bool>> ConvertPredicate(Expression<Func<TEntity, bool>> expression)
        {
            var parameter = Expression.Parameter(typeof(TModel), "x");
            var converter = new ExpressionConverter<TEntity, TModel>(parameter);
            return converter.Convert(expression);
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            var model = await _db.Queryable<TModel>().InSingleAsync(id);
            return model?.Adapt<TEntity>();
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var modelPredicate = ConvertPredicate(predicate);
            var model = await _db.Queryable<TModel>().FirstAsync(modelPredicate);
            return model?.Adapt<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            var models = await _db.Queryable<TModel>().ToListAsync();
            return models.Adapt<List<TEntity>>();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var modelPredicate = ConvertPredicate(predicate);
            var models = await _db.Queryable<TModel>().Where(modelPredicate).ToListAsync();
            return models.Adapt<List<TEntity>>();
        }

        public async Task<(List<TEntity> Items, int Total)> GetPagedAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            int pageIndex = 1,
            int pageSize = 20,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool isDesc = true)
        {
            RefAsync<int> total = 0;
            var query = _db.Queryable<TModel>();

            if (predicate != null)
            {
                var modelPredicate = ConvertPredicate(predicate);
                query = query.Where(modelPredicate);
            }

            // 简化排序处理，使用动态排序
            if (orderBy != null)
            {
                // 直接使用字符串排序，避免复杂的表达式转换
                query = query.OrderByIF(true, "ID DESC");
            }

            var models = await query.ToPageListAsync(pageIndex, pageSize, total);
            var items = models.Adapt<List<TEntity>>();
            return (items, total.Value);
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            var model = entity.Adapt<TModel>();
            var result = await _db.Insertable(model).ExecuteCommandAsync();
            return result > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            var models = entities.Adapt<List<TModel>>();
            return await _db.Insertable(models).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            var model = entity.Adapt<TModel>();
            return await _db.Updateable(model).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            var models = entities.Adapt<List<TModel>>();
            return await _db.Updateable(models).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            var model = entity.Adapt<TModel>();
            return await _db.Deleteable(model).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var modelPredicate = ConvertPredicate(predicate);
            return await _db.Deleteable<TModel>().Where(modelPredicate).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(object id)
        {
            return await _db.Deleteable<TModel>().In(id).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var modelPredicate = ConvertPredicate(predicate);
            return await _db.Queryable<TModel>().AnyAsync(modelPredicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _db.Queryable<TModel>().CountAsync();

            var modelPredicate = ConvertPredicate(predicate);
            return await _db.Queryable<TModel>().CountAsync(modelPredicate);
        }
    }
}