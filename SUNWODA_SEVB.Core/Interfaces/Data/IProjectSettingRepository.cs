using SUNWODA_SEVB.Core.Models.Data;

namespace SUNWODA_SEVB.Core.Interfaces.Data
{
    public interface IProjectSettingRepository : IRepository<ProjectSettingModel>
    {
        /// <summary>
        /// 通过ID获取项目设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectSettingModel?> GetByIDAsync(int id);

        /// <summary>
        /// 通过名称获取项目设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<ProjectSettingModel>> GetByNameAsync(string name);

        /// <summary>
        /// 通过所属VM获取项目设置
        /// </summary>
        /// <param name="belongToVM"></param>
        /// <returns></returns>
        Task<List<ProjectSettingModel>> GetByBelongToVMAsync(string belongToVM);

        /// <summary>
        /// 通过类型获取项目设置
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<List<ProjectSettingModel>> GetByTypeAsync(string type);

        /// <summary>
        /// 通过项目名称和配置名称获取项目设置
        /// </summary>
        /// <param name="projectName">项目名称（BelongToVM）</param>
        /// <param name="name">配置名称</param>
        /// <returns></returns>
        Task<List<ProjectSettingModel>> GetByProjectAndNameAsync(string projectName, string name);
    }
}
