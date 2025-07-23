using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Inovance;
using HslCommunication.Profinet.Keyence;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.Omron;
using HslCommunication.Profinet.Panasonic;
using HslCommunication.Profinet.Siemens;
using SUNWODA_SEVB.PLC.Core;
using SUNWODA_SEVB.PLC.Enumerations;

namespace SUNWODA_SEVB.PLC
{
    public class PLCEngine
    {
        /// <summary>
        /// 设备列表
        /// </summary>
        public static List<Core.PLC> PLCs { get; set; } = new List<Core.PLC>();

        public static object RWAddressTableLock = new object();
        public static ObservableCollection<PLCRWAddress> PLCRWAddressTable { get; set; } = new ObservableCollection<PLCRWAddress>();

        /// <summary>
        /// HSL授权
        /// </summary>
        public static void HSLAuthorization()
        {
            var isActive = Authorization.SetAuthorizationCode("04f14563-6515-49be-a713-3ece0d21cc3e");
            if (!isActive)
                throw new Exception("HSL激活失败");
        }

        /// <summary>
        /// 添加PLC
        /// </summary>
        /// <param name="plc">PLC</param>
        public static void AddPLC(Core.PLC plc)
        {
            if (PLCs.FirstOrDefault(it => it.Name == plc.Name) != default(Core.PLC))
                return;
            switch (plc.BrandSpecificationProtocal)
            {
                case PLCBrandSpecificationProtocal.Siemens_S7NetS1500:
                {
                    var siemens = new SiemensS7Net(SiemensPLCS.S1500, plc.IP);
                    siemens.Port = plc.Port;
                    plc.ByteTransform = siemens.ByteTransform;
                    plc.Device = siemens;
                    plc.AddressWordLength = 1;
                    break;
                }
                case PLCBrandSpecificationProtocal.Siemens_S7NetS1200:
                {
                    var siemens = new SiemensS7Net(SiemensPLCS.S1200, plc.IP);
                    siemens.Port = plc.Port;
                    plc.ByteTransform = siemens.ByteTransform;
                    plc.Device = siemens;
                    plc.AddressWordLength = 1;
                    break;
                }
                case PLCBrandSpecificationProtocal.Siemens_S7NetS400:
                {
                    var siemens = new SiemensS7Net(SiemensPLCS.S400, plc.IP);
                    siemens.Port = plc.Port;
                    siemens.Rack = 0;
                    siemens.Slot = 3;
                    plc.ByteTransform = siemens.ByteTransform;
                    plc.Device = siemens;
                    plc.AddressWordLength = 1;
                    break;
                }
                case PLCBrandSpecificationProtocal.Siemens_S7NetS300:
                {
                    var siemens = new SiemensS7Net(SiemensPLCS.S300, plc.IP);
                    siemens.Port = plc.Port;
                    plc.ByteTransform = siemens.ByteTransform;
                    plc.Device = siemens;
                    plc.AddressWordLength = 1;
                    break;
                }
                case PLCBrandSpecificationProtocal.Siemens_S7NetS200:
                {
                    var siemens = new SiemensS7Net(SiemensPLCS.S200, plc.IP);
                    siemens.Port = plc.Port;
                    plc.ByteTransform = siemens.ByteTransform;
                    plc.Device = siemens;
                    plc.AddressWordLength = 1;
                    break;
                }
                case PLCBrandSpecificationProtocal.Siemens_S7NetS200Smart:
                {
                    var siemens = new SiemensS7Net(SiemensPLCS.S200Smart, plc.IP);
                    siemens.Port = plc.Port;
                    plc.ByteTransform = siemens.ByteTransform;
                    plc.Device = siemens;
                    plc.AddressWordLength = 1;
                    break;
                }
                case PLCBrandSpecificationProtocal.Omron_Fins_TCP:
                {
                    var omron = new OmronFinsNet(plc.IP, plc.Port);
                    if (plc.ByteDataFormat != null)
                        omron.ByteTransform.DataFormat = (DataFormat)plc.ByteDataFormat;
                    plc.ByteTransform = omron.ByteTransform;
                    plc.Device = omron;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Melsec_MC_Binary:
                {
                    var melsec = new MelsecMcNet(plc.IP, plc.Port);
                    //melsec.ByteTransform.IsStringReverseByteWord = false;
                    plc.ByteTransform = melsec.ByteTransform;
                    plc.Device = melsec;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Melsec_MC_Ascii:
                {
                    var melsec = new MelsecMcAsciiNet(plc.IP, plc.Port);
                    melsec.ByteTransform.IsStringReverseByteWord = true;
                    plc.ByteTransform = melsec.ByteTransform;
                    plc.Device = melsec;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Melsec_A1E_Binary:
                {
                    var melsec = new MelsecA1ENet(plc.IP, plc.Port);
                    //melsec.ByteTransform.IsStringReverseByteWord = false;
                    plc.ByteTransform = melsec.ByteTransform;
                    plc.Device = melsec;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Panasonic_Mewtocol_OverTcp:
                {
                    var panasonic = new PanasonicMewtocolOverTcp(plc.IP, plc.Port);
                    //panasonic.ByteTransform.IsStringReverseByteWord = false;
                    plc.ByteTransform = panasonic.ByteTransform;
                    plc.Device = panasonic;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Keyence_MC_Binary:
                {
                    var keyence = new KeyenceMcNet(plc.IP, plc.Port);
                    //keyence.ByteTransform.IsStringReverseByteWord = false;
                    plc.ByteTransform = keyence.ByteTransform;
                    plc.Device = keyence;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Keyence_MC_Ascii:
                {
                    var keyence = new KeyenceMcAsciiNet(plc.IP, plc.Port);
                    keyence.ByteTransform.IsStringReverseByteWord = true;
                    plc.ByteTransform = keyence.ByteTransform;
                    plc.Device = keyence;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Inovance_AM_Tcp:
                {
                    var inovance = new InovanceTcpNet(plc.IP, plc.Port);
                    inovance.Series = InovanceSeries.AM;
                    if (plc.ByteDataFormat != null)
                        inovance.ByteTransform.DataFormat = (DataFormat)plc.ByteDataFormat;
                    plc.ByteTransform = inovance.ByteTransform;
                    plc.Device = inovance;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Inovance_H3U_Tcp:
                {
                    var inovance = new InovanceTcpNet(plc.IP, plc.Port);
                    inovance.Series = InovanceSeries.H3U;
                    if (plc.ByteDataFormat != null)
                        inovance.ByteTransform.DataFormat = (DataFormat)plc.ByteDataFormat;
                    plc.ByteTransform = inovance.ByteTransform;
                    plc.Device = inovance;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Inovance_H5U_Tcp:
                {
                    var inovance = new InovanceTcpNet(plc.IP, plc.Port);
                    inovance.Series = InovanceSeries.H5U;
                    if (plc.ByteDataFormat != null)
                        inovance.ByteTransform.DataFormat = (DataFormat)plc.ByteDataFormat;
                    plc.ByteTransform = inovance.ByteTransform;
                    plc.Device = inovance;
                    plc.AddressWordLength = 2;
                    break;
                }
                case PLCBrandSpecificationProtocal.Modbus_Tcp:
                {
                    var modbusTcp = new ModbusTcpNet(plc.IP, plc.Port);
                    if (plc.ByteDataFormat != null)
                        modbusTcp.ByteTransform.DataFormat = (DataFormat)plc.ByteDataFormat;
                    plc.ByteTransform = modbusTcp.ByteTransform;
                    plc.Device = modbusTcp;
                    plc.AddressWordLength = 2;
                    break;
                }
            }
            OperateResult? connectServer = plc.Device?.ConnectServer();
            plc.IsConnect = connectServer?.IsSuccess ?? false;
            PLCs.Add(plc);
        }
    }
}
