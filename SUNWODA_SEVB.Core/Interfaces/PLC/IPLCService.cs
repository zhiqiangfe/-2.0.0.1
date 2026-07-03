using SUNWODA_SEVB.Core.Models.PLC;
using System.Collections.ObjectModel;

namespace SUNWODA_SEVB.Core.Interfaces.PLC
{
    public interface IPLCService
    {
        IReadOnlyDictionary<string, ConnectInfo> ConnectionStatus { get; }
        IReadOnlyDictionary<int, PLCRWAddress> RWAddresses { get; }


        /// <summary>
        /// 初始化PLC
        /// </summary>
        Task<bool> InitializeAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 写入指定PLC地址的值
        /// </summary>
        Task<bool> WriteValueAsync(int addressId, object value, CancellationToken cancellationToken = default);
    }
}
