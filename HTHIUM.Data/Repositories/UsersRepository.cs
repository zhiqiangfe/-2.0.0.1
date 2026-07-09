using SqlSugar;
using HTHIUM.Core.Interfaces.Data;
using HTHIUM.Core.Models.Data;
using HTHIUM.Data.Models;

namespace HTHIUM.Data.Repositories
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

        public UsersModel? GetByUserAccount(string userAccount)
        {
            return Get(model => model.UserAccount == userAccount);
        }

        public async Task<UsersModel?> GetByUserAccountAsync(string userAccount)
        {
            return await GetAsync(model => model.UserAccount == userAccount);
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
