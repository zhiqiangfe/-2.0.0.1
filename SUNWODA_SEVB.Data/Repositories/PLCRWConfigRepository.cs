using Mapster;
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
            //var modelList = await _db.Queryable<PLCRWConfig>().Where(model => model.PLCID == plcID && model.IsEnable).ToListAsync();
            //return modelList.Adapt<List<PLCRWConfigModel>>();
            return await GetListAsync(model => model.PLCID == plcID && model.IsEnable);
        }
    }
}
