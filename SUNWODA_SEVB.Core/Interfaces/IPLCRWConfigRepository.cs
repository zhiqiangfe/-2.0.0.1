using SUNWODA_SEVB.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Core.Interfaces
{
    public interface IPLCRWConfigRepository : IRepository<PLCRWConfigModel>
    {
        /// <summary>
        /// 获取指定PLC的所有启用地址段
        /// </summary>
        /// <param name="plcID"></param>
        /// <returns></returns>
        Task<List<PLCRWConfigModel>> GetEnabledConfigsAsync(int plcID);
    }
}
