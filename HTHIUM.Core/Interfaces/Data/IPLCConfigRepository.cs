using HTHIUM.Core.Models.Data;

namespace HTHIUM.Core.Interfaces.Data
{
    public interface IPLCConfigRepository : IRepository<PLCConfigModel>
    {
        /// <summary>
        /// 获取启用的PLC
        /// </summary>
        /// <returns></returns>
        Task<List<PLCConfigModel>> GetEnabledConfigsAsync();
    }
}
