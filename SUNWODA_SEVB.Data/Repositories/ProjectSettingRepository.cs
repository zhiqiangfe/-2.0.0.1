using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models.Data;
using SUNWODA_SEVB.Data.Models;

namespace SUNWODA_SEVB.Data.Repositories
{
    //public class ProjectSettingRepository : MappingRepository<ProjectSettingModel, ProjectSetting>, IProjectSettingRepository
    //{
    //    public ProjectSettingRepository(ISqlSugarClient db) : base(db) { }

    //    public async Task<List<ProjectSettingModel>> GetByVMNameAsync(string vmName)
    //    {
    //        return await GetListAsync(model => model.BelongToVM == vmName);
    //    }

    //    public async Task<ProjectSettingModel?> GetByNameAndVMAsync(string name, string vmName)
    //    {
    //        return await GetAsync(model => model.Name == name && model.BelongToVM == vmName);
    //    }

    //    public async Task<dynamic?> GetSettingValueAsync(string name, string vmName)
    //    {
    //        var model = await GetByNameAndVMAsync(name, vmName);
    //        return model != null ? DataTypeConverter.StringToValue(model.Type, model.Value) : null;
    //    }

    //    public async Task<bool> UpdateSettingValueAsync(string name, string vmName, dynamic value)
    //    {
    //        var model = await GetByNameAndVMAsync(name, vmName);
    //        if (model != null)
    //        {
    //            model.Value = DataTypeConverter.ValueToString(value);
    //            return await UpdateAsync(model);
    //        }
    //        return false;
    //    }
    //}
}