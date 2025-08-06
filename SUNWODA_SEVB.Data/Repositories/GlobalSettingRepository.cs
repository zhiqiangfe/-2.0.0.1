using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models.Data;
using SUNWODA_SEVB.Data.Models;
using SUNWODA_SEVB.Data.Helpers;

namespace SUNWODA_SEVB.Data.Repositories
{
    public class GlobalSettingRepository : MappingRepository<GlobalSettingModel, GlobalSetting>, IGlobalSettingRepository
    {
        public GlobalSettingRepository(ISqlSugarClient db) : base(db) { }

        public async Task<GlobalSettingModel?> GetByIDAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<GlobalSettingModel?> GetByNameAsync(string name)
        {
            return await GetAsync(model => model.Name == name);
        }

        public async Task<dynamic?> GetSettingValueAsync(string name)
        {
            var model = await GetByNameAsync(name);
            return model != null ? DataTypeConverter.StringToValue(model.Type, model.Value) : null;
        }

        public async Task<bool> UpdateSettingValueAsync(string name, dynamic value)
        {
            var model = await GetByNameAsync(name);
            if (model != null)
            {
                model.Value = DataTypeConverter.ValueToString(value);
                return await UpdateAsync(model);
            }
            return false;
        }
    }
}