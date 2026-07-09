using HTHIUM.Core.Interfaces.Data;
using HTHIUM.Core.Models.Data;

namespace HTHIUM.Core.Interfaces
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
