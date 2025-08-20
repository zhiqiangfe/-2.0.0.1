using SUNWODA_SEVB.Core.Models.Data;

namespace SUNWODA_SEVB.Core.Interfaces.Data
{
    public interface IUsersRepository : IRepository<UsersModel>
    {
        /// <summary>
        /// 通过ID获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UsersModel?> GetByIDAsync(int id);

        /// <summary>
        /// 通过用户名获取用户 (用户名通常是唯一的)
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UsersModel?> GetByUserNameAsync(string userName);

        /// <summary>
        /// 通过角色ID获取用户列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<UsersModel>> GetByRoleIDAsync(int roleId);
    }
}
