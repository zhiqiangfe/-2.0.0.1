using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models.Data;
using SUNWODA_SEVB.Data.Models;

namespace SUNWODA_SEVB.Data.Repositories
{
    public class UsersRepository : MappingRepository<UsersModel, Users>, IUsersRepository
    {
        public UsersRepository(ISqlSugarClient db) : base(db) { }

        public UsersModel? GetByID(int id)
        {
            return GetById(id);
        }

        public async Task<UsersModel?> GetByIDAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public UsersModel? GetByUserName(string username)
        {
            return Get(model => model.UserName == username);
        }

        public async Task<UsersModel?> GetByUserNameAsync(string username)
        {
            return await GetAsync(model => model.UserName == username);
        }
    }
}
