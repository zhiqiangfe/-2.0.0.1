using SUNWODA_SEVB.Core.Models.PLC;
using System.Collections.ObjectModel;

namespace SUNWODA_SEVB.Core.Interfaces
{
    public interface IPLCService
    {
        /// <summary>
        /// 初始化PLC
        /// </summary>
        Task<bool> InitializeAsync(CancellationToken cancellationToken);
    }
}
