using SUNWODA_SEVB.Core.Models.PLC;
using System.Collections.ObjectModel;

namespace SUNWODA_SEVB.Core.Interfaces
{
    public interface IPLCService
    {
        static ObservableCollection<ConnectInfo>? ConnectInfos { get; set; }

        static ObservableCollection<PLCRWAddress>? PLCRWAddressTable { get; set; }
        /// <summary>
        /// 初始化PLC
        /// </summary>
        Task<bool> InitPlcs();
    }
}
