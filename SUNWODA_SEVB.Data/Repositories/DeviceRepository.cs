using Mapster;
using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models.Data;
using SUNWODA_SEVB.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.Repositories
{
    public class DeviceRepository : MappingRepository<DeviceModel, Device>, IDeviceRepository
    {
        public DeviceRepository(ISqlSugarClient db) : base(db) { }

        public async Task<List<DeviceModel>> GetByBaseNameAsync(string baseName)
        {
            return await GetListAsync(model => model.BaseName == baseName);
        }

        public async Task<DeviceModel?> GetByIDAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<List<DeviceModel>> GetByLineNameAsync(string lineName)
        {
            return await GetListAsync(model => model.LineName == lineName);
        }

        public async Task<List<DeviceModel>> GetByNameAsync(string name)
        {
            return await GetListAsync(model => model.Name == name);
        }

        public async Task<List<DeviceModel>> GetByNumberAsync(string number)
        {
            return await GetListAsync(model => model.Number == number);
        }
    }
}
