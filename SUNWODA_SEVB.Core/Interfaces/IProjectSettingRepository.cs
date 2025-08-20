using SUNWODA_SEVB.Core.Models.Data;

namespace SUNWODA_SEVB.Core.Interfaces
{
    public interface IProjectSettingRepository : IRepository<ProjectSettingModel>
    {
        /// <summary>
        /// 通过ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectSettingModel?> GetByIDAsync(int id);

        /// <summary>
        /// 通过ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProjectSettingModel? GetByID(int id);

        /// <summary>
        /// 通过项目名称获取该项目所有变量
        /// </summary>
        /// <param name="vmName"></param>
        /// <returns></returns>
        Task<List<ProjectSettingModel>?> GetByVMNameAsync(string vmName);

        /// <summary>
        /// 通过项目名称获取该项目所有变量
        /// </summary>
        /// <param name="vmName"></param>
        /// <returns></returns>
        List<ProjectSettingModel>? GetByVMName(string vmName);

        /// <summary>
        /// 通过项目名称和变量名称获取
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vmName"></param>
        /// <returns></returns>
        Task<ProjectSettingModel?> GetByNameAndVMAsync(string name, string vmName);

        /// <summary>
        /// 通过项目名称和变量名称获取
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vmName"></param>
        /// <returns></returns>
        ProjectSettingModel? GetByNameAndVM(string name, string vmName);

        /// <summary>
        /// 通过变量名称获取配置值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vmName"></param>
        /// <returns></returns>
        Task<dynamic?> GetSettingValueAsync(string name, string vmName);

        /// <summary>
        /// 通过变量名称获取配置值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vmName"></param>
        /// <returns></returns>
        dynamic? GetSettingValue(string name, string vmName);

        /// <summary>
        /// 更新配置值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vmName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> UpdateSettingValueAsync(string name, string vmName, dynamic value);

        /// <summary>
        /// 更新配置值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vmName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool UpdateSettingValue(string name, string vmName, dynamic value);
    }
}
