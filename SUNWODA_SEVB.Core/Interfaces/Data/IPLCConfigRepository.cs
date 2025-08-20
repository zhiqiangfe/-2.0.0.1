using SUNWODA_SEVB.Core.Models.Data;

namespace SUNWODA_SEVB.Core.Interfaces
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
