using SUNWODA_SEVB.Core.Entities;

namespace SUNWODA_SEVB.Core.Interfaces
{
    public interface IPLCAddressConfigRepository : IRepository<PLCAddressConfigModel>
    {
        /// <summary>
        /// 获取指定PLC配置的所有地址
        /// </summary>
        /// <param name="plcID"></param>
        /// <returns></returns>
        Task<List<PLCAddressConfigModel>> GetMonitorAddressesAsync(int plcID);
    }
}
