using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models.Data;
using SUNWODA_SEVB.Data.Models;

namespace SUNWODA_SEVB.Data.Repositories
{
    public class PLCRWConfigRepository : MappingRepository<PLCRWConfigModel, PLCRWConfig>, IPLCRWConfigRepository
    {
        public PLCRWConfigRepository(ISqlSugarClient db) : base(db) { }

        public async Task<List<PLCRWConfigModel>> GetEnabledConfigsAsync(int plcID)
        {
            return await GetListAsync(model => model.PLCID == plcID && model.IsEnable);
        }
    }
}
