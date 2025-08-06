using Mapster;
using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models.Data;
using SUNWODA_SEVB.Data.Models;
using SUNWODA_SEVB.Data.Helpers;
using SUNWODA_SEVB.Tool.Extension;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
            var model = await _db.Queryable<GlobalSetting>().FirstAsync(model => model.Name == name);
            return model?.Adapt<GlobalSettingModel>();
            //return await GetAsync(model => model.Name == name);
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
            else
            {
                return false;
            }
        }

        public dynamic StringToAny(string type, string value)
        {
            type = type.ToUpper();
            switch (type)
            {
                case "STRING":
                    return value;
                case "FLOAT":
                    return value.ToFloat();
                case "DOUBLE":
                    return value.ToDouble();
                case "DECIMAL":
                    return value.ToDecimal();
                case "SBYTE":
                    return value.ToSByte();
                case "BYTE":
                    return value.ToByte();
                case "SHORT":
                    return value.ToShort();
                case "USHORT":
                    return value.ToUShort();
                case "INT":
                    return value.ToInt();
                case "UINT":
                    return value.ToUInt();
                case "LONG":
                    return value.ToLong();
                case "ULONG":
                    return value.ToLong();
                case "BOOL":
                    return value.ToBool();
                case "DATETIME":
                    return value.ToDateTime("yyyy-MM-dd HH:mm:ss");
                case "WINDOWSTATE":
                    return value.ToWindowSate();
                default:
                    throw new FormatException("无效类型");
            }
        }
    }
}
