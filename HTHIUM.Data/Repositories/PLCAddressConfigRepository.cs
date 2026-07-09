using Mapster;
using SqlSugar;
using HTHIUM.Core.Entities;
using HTHIUM.Core.Interfaces.Data;
using HTHIUM.Core.Models.Data;
using HTHIUM.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHIUM.Data.Repositories
{
    public class PLCAddressConfigRepository : MappingRepository<PLCAddressConfigModel, PLCAddressConfig>, IPLCAddressConfigRepository
    {
        public PLCAddressConfigRepository(ISqlSugarClient db) : base(db) { }

        public async Task<List<PLCAddressConfigModel>> GetMonitorAddressesAsync(int plcID)
        {
            //var modelList = await _db.Queryable<PLCAddressConfig>().Where(model => model.PLCID == plcID && model.IsMonitor).ToListAsync();
            //return modelList.Adapt<List<PLCAddressConfigModel>>();
            return await GetListAsync(model => model.PLCID == plcID && model.IsMonitor);
        }
    }
}
