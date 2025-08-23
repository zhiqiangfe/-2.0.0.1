using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.Data;
using SUNWODA_SEVB.Data.Repositories;

namespace SUNWODA_SEVB.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISqlSugarClient _db;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(ISqlSugarClient db)
        {
            _db = db;
        }

        public async Task BeginTransactionAsync()
        {
            await _db.Ado.BeginTranAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _db.Ado.CommitTranAsync();
            }
            catch
            {
                await _db.Ado.RollbackTranAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            await _db.Ado.RollbackTranAsync();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, new()
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new BaseRepository<TEntity>(_db);
            }
            return (IRepository<TEntity>)_repositories[type];
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
