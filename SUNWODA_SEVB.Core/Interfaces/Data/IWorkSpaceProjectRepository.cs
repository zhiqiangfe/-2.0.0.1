using SUNWODA_SEVB.Core.Models.Data;

namespace SUNWODA_SEVB.Core.Interfaces.Data
{
    public interface IWorkSpaceProjectRepository : IRepository<WorkSpaceProjectModel>
    {
        /// <summary>
        /// 通过ID获取工作区项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkSpaceProjectModel?> GetByIDAsync(int id);

        /// <summary>
        /// 通过VM类名获取工作区项目
        /// </summary>
        /// <param name="vmClassName"></param>
        /// <returns></returns>
        Task<List<WorkSpaceProjectModel>> GetByVMClassNameAsync(string vmClassName);

        /// <summary>
        /// 根据是否启用状态获取工作区项目
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        Task<List<WorkSpaceProjectModel>> GetByIsEnabledAsync(bool isEnabled);
    }
}
