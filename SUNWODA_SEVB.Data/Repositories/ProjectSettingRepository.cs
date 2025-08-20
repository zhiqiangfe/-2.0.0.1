using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces.Data;
using SUNWODA_SEVB.Core.Models.Data;
using SUNWODA_SEVB.Data.Models;
using SUNWODA_SEVB.Tool.Helper;

namespace SUNWODA_SEVB.Data.Repositories
{
    public class ProjectSettingRepository : MappingRepository<ProjectSettingModel, ProjectSetting>, IProjectSettingRepository
    {
        public ProjectSettingRepository(ISqlSugarClient db) : base(db) { }


        #region 接口实现方法

        /// <summary>
        /// 通过ID获取项目设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProjectSettingModel?> GetByIDAsync(int id)
        {
            return await GetAsync(model => model.ID == id);
        }

        /// <summary>
        /// 通过名称获取项目设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<ProjectSettingModel>> GetByNameAsync(string name)
        {
            return await GetListAsync(model => model.Name == name);
        }

        /// <summary>
        /// 通过所属VM获取项目设置
        /// </summary>
        /// <param name="belongToVM"></param>
        /// <returns></returns>
        public async Task<List<ProjectSettingModel>> GetByBelongToVMAsync(string belongToVM)
        {
            return await GetListAsync(model => model.BelongToVM == belongToVM);
        }

        /// <summary>
        /// 通过类型获取项目设置
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<ProjectSettingModel>> GetByTypeAsync(string type)
        {
            return await GetListAsync(model => model.Type == type);
        }

        /// <summary>
        /// 通过项目名称和配置名称获取项目设置
        /// </summary>
        /// <param name="projectName">项目名称（BelongToVM）</param>
        /// <param name="name">配置名称</param>
        /// <returns></returns>
        public async Task<List<ProjectSettingModel>> GetByProjectAndNameAsync(string projectName, string name)
        {
            return await GetListAsync(model => model.BelongToVM == projectName && model.Name == name);
        }

        #endregion

        #region 自定义扩展方法

        public async Task<ProjectSettingModel?> GetByNameAndVMAsync(string name, string vmName)
        {
            return await GetAsync(model => model.Name == name && model.BelongToVM == vmName);
        }

        public async Task<dynamic?> GetSettingValueAsync(string name, string vmName)
        {
            var model = await GetByNameAndVMAsync(name, vmName);
            return model != null ? DataTypeConverter.StringToValue(model.Type, model.Value) : null;
        }

        public async Task<bool> UpdateSettingValueAsync(string name, string vmName, dynamic value)
        {
            var model = await GetByNameAndVMAsync(name, vmName);
            if (model != null)
            {
                model.Value = DataTypeConverter.ValueToString(value);
                return await UpdateAsync(model);
            }
            return false;
        }

        #endregion
    }
}