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
    public class PLCConfigRepository : MappingRepository<PLCConfigModel, PLCConfig>, IPLCConfigRepository
    {
        public PLCConfigRepository(ISqlSugarClient db) : base(db) { }

        public async Task<List<PLCConfigModel>> GetEnabledConfigsAsync()
        {
            //var modelList = await _db.Queryable<PLCConfig>().Where(model => model.IsEnable).ToListAsync();
            //return modelList.Adapt<List<PLCConfigModel>>();
            return await GetListAsync(model => model.IsEnable);
        }
    }
}
