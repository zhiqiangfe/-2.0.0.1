using SUNWODA_SEVB.Core.Interfaces.Data;
using SUNWODA_SEVB.Core.Models.Data;

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
