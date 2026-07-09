using Mapster;
using SqlSugar;
using HTHIUM.Core.Interfaces.Data;
using HTHIUM.Core.Models.Data;
using HTHIUM.Data.Models;
using HTHIUM.Tool.Helper;

namespace HTHIUM.Data.Repositories
{
    public class GlobalSettingRepository : MappingRepository<GlobalSettingModel, GlobalSetting>, IGlobalSettingRepository
    {
        public GlobalSettingRepository(ISqlSugarClient db) : base(db) { }

        public async Task<GlobalSettingModel?> GetByIDAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public GlobalSettingModel? GetByID(int id)
        {
            return GetById(id);
        }

        public async Task<GlobalSettingModel?> GetByNameAsync(string name)
        {
            return await GetAsync(model => model.Name == name);
        }

        public GlobalSettingModel? GetByName(string name)
        {
            return Get(model => model.Name == name);
        }

        public async Task<dynamic?> GetSettingValueAsync(string name)
        {
            var model = await GetByNameAsync(name);
            return model != null ? DataTypeConverter.StringToValue(model.Type, model.Value) : null;
        }

        public dynamic? GetSettingValue(string name)
        {
            var model = GetByName(name);
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
            else
            {
                return false;
            }
        }

        public bool UpdateSettingValue(string name, dynamic value)
        {
            var model = GetByName(name);
            if (model != null)
            {
                model.Value = DataTypeConverter.ValueToString(value);
                return Update(model);
            }
            else
            {
                return false;
            }
        }
    }
}
