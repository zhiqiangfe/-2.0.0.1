using SUNWODA_SEVB.Core.Models.Data;

namespace SUNWODA_SEVB.Core.Interfaces.Data
{
    public interface IUsersRepository : IRepository<UsersModel>
    {
        /// <summary>
        /// 通过ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UsersModel?> GetByIDAsync(int id);

        /// <summary>
        /// 通过ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UsersModel? GetByID(int id);

        /// <summary>
        /// 通过账号获取
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        Task<UsersModel?> GetByUserAccountAsync(string userAccount);

        /// <summary>
        /// 通过账号获取
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        UsersModel? GetByUserAccount(string userAccount);

        /// <summary>
        /// 通过用户名称获取
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<UsersModel?> GetByUserNameAsync(string username);

        /// <summary>
        /// 通过用户名称获取
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UsersModel? GetByUserName(string username);
    }
}
