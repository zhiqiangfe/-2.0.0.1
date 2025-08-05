using SqlSugar;
using SUNWODA_SEVB.Core.Entities;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.Repositories
{
    public class PLCAddressConfigRepository : MappingRepository<PLCAddressConfigModel, PLCAddressConfig>, IPLCAddressConfigRepository
    {
        public PLCAddressConfigRepository(ISqlSugarClient db) : base(db) { }

        public async Task<List<PLCAddressConfigModel>> GetMonitorAddressesAsync(int plcID)
        {
            return await GetListAsync(model => model.PLCID == plcID && model.IsMonitor);
        }
    }
}
