using SUNWODA_SEVB.Core.Models.Data;

namespace SUNWODA_SEVB.Core.Interfaces.Data
{
    public interface IWorkSpaceProjectRepository : IRepository<WorkSpaceProjectModel>
    {
        /// <summary>
        /// 通过ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkSpaceProjectModel?> GetByIDAsync(int id);

        /// <summary>
        /// 通过ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WorkSpaceProjectModel? GetByID(int id);

        /// <summary>
        /// 通过视图模型类名获取
        /// </summary>
        /// <param name="vmClassName"></param>
        /// <returns></returns>
        Task<WorkSpaceProjectModel?> GetByVMClassNameAsync(string vmClassName);

        /// <summary>
        /// 通过视图模型类名获取
        /// </summary>
        /// <param name="vmClassName"></param>
        /// <returns></returns>
        WorkSpaceProjectModel? GetByVMClassName(string vmClassName);

        /// <summary>
        /// 通过视图模型类名获取项目是否启用
        /// </summary>
        /// <param name="vmClassName"></param>
        /// <returns></returns>
        Task<bool> GetIsEnabledAsync(string vmClassName);

        /// <summary>
        /// 通过视图模型类名获取项目是否启用
        /// </summary>
        /// <param name="vmClassName"></param>
        /// <returns></returns>
        bool GetIsEnabled(string vmClassName);

        /// <summary>
        /// 设置项目是否启用
        /// </summary>
        /// <param name="vmClassName"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        Task<bool> UpdateIsEnabledAsync(string vmClassName, bool isEnabled);

        /// <summary>
        /// 设置项目是否启用
        /// </summary>
        /// <param name="vmClassName"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        bool UpdateIsEnabled(string vmClassName, bool isEnabled);
    }
}
