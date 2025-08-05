using SUNWODA_SEVB.Core.Models.PLC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
