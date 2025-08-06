using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using HslCommunication;
using HslCommunication.Core;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Inovance;
using HslCommunication.Profinet.Keyence;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.Omron;
using HslCommunication.Profinet.Panasonic;
using HslCommunication.Profinet.Siemens;
using SUNWODA_SEVB.Core.Enumerations.PLC;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models.Data;
using SUNWODA_SEVB.Core.Models.PLC;
using SUNWODA_SEVB.Tool.Extension;
using SUNWODA_SEVB.Tool.Helper;

namespace SUNWODA_SEVB.PLC
{
    public class PLCService : IPLCService
    {
        private readonly ILoggerService<PLCService> _logger;
        private readonly IPLCConfigRepository _pLcConfigRepository;
        private readonly IPLCRWConfigRepository _plcRWConfigRepository;
        private readonly IPLCAddressConfigRepository _plcAddressConfigRepository;
        public readonly object RWAddressTableLock = new object();

        public bool IsCycleReadPLC { get; set; }

        public bool IsCycleWritePLC { get; set; }

        /// <summary>
        /// 设备列表
        /// </summary>
        public static List<PLC> PLCs { get; set; } = new List<PLC>();

        /// <summary>
        /// PLC连接状态信息
        /// </summary>
        public static ObservableCollection<ConnectInfo>? ConnectInfos { get; set; } =
            new ObservableCollection<ConnectInfo>();

        public static List<PLCRWConfigModel> PLCRWConfigModels { get; set; } =
            new List<PLCRWConfigModel> { };

        /// <summary>
        /// PLC读取数据
        /// </summary>
        public static ObservableCollection<PLCRWAddress>? PLCRWAddressTable { get; set; } =
            new ObservableCollection<PLCRWAddress>();

        public PLCService(
            ILoggerService<PLCService> loggerService,
            IGlobalSettingRepository globalSettingRepository,
            IPLCConfigRepository pLcConfigRepository,
            IPLCRWConfigRepository pLcRWConfigRepository,
            IPLCAddressConfigRepository pLcAddressConfigRepository
        )
        {
            _logger = loggerService;
            _pLcConfigRepository = pLcConfigRepository;
            _plcRWConfigRepository = pLcRWConfigRepository;
            _plcAddressConfigRepository = pLcAddressConfigRepository;
            IsCycleReadPLC = globalSettingRepository.GetSettingValueAsync("IsCycleReadPLC").Result;
            IsCycleWritePLC = globalSettingRepository
                .GetSettingValueAsync("IsCycleWritePLC")
                .Result;
        }

        /// <summary>
        /// HSL授权
        /// </summary>
        public static void HSLAuthorization()
        {
            var isActive = Authorization.SetAuthorizationCode(
                "04f14563-6515-49be-a713-3ece0d21cc3e"
            );
            if (!isActive)
                throw new Exception("HSL激活失败");
        }

        /// <summary>
        /// 添加PLC
        /// </summary>
        /// <param name="plc">PLC</param>
        public static void AddPLC(PLC plc)
        {
            if (PLCs.FirstOrDefault(it => it.Name == plc.Name) != default(PLC))
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

        /// <summary>
        /// PLC连接
        /// </summary>
        public void PLCConnect()
        {
            foreach (var plc in PLCs)
            {
                ThreadManager
                    .AddThread(
                        $"PLC[{plc.Name}]读取线程",
                        new ThreadTaskCancelSignal(),
                        (cancelSignal) =>
                        {
                            var message = "";
                            var addressRangeName = "";
                            var addressName = "";
                            while (!cancelSignal.CancelSignal)
                            {
                                Thread.Sleep(plc.CycleReadTime);
                                if (!plc.IsConnect)
                                {
                                    if (plc.TryConnect(out message))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        _logger.Error(message ?? "连接错误");
                                        Thread.Sleep(5000);
                                        continue;
                                    }
                                }

                                try
                                {
                                    // PLC读取

                                    if (IsCycleReadPLC)
                                    {
                                        foreach (
                                            var plcRWConfig in PLCRWConfigModels.Where(it =>
                                                it.PLCID == plc.ID && it.RWMode.ToUpper() == "R"
                                            )
                                        )
                                        {
                                            addressRangeName =
                                                $"{plcRWConfig.Name}:起始地址{plcRWConfig.AreaName}{plcRWConfig.StartAddress};长度{plcRWConfig.Length}";
                                            var readContent = plc.Read(
                                                $"{plcRWConfig.AreaName}{plcRWConfig.StartAddress}",
                                                (ushort)plcRWConfig.Length
                                            );
                                            if (readContent == null)
                                                continue;
                                            try
                                            {
                                                foreach (
                                                    var plcAddress in PLCRWAddressTable!.Where(it =>
                                                        it.AddressRangeId == plcRWConfig.ID
                                                    )
                                                )
                                                {
                                                    addressName = plcAddress.ParameterName;
                                                    plcAddress.MonitorValue = ConvertByBytes(
                                                        readContent.Content,
                                                        plcAddress.Type,
                                                        plc.ByteTransform,
                                                        plc.AddressWordLength,
                                                        plcAddress.Index,
                                                        plcAddress.Length
                                                    );
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _logger.Error(
                                                    $"PLC地址[{addressName}]解析数据失败",
                                                    ex
                                                );
                                                Thread.Sleep(3000);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error($"PLC地址段[{addressRangeName}]读取数据失败", ex);
                                    Thread.Sleep(3000);
                                }
                            }
                        }
                    )
                    .Start();

                ThreadManager
                    .AddThread(
                        $"PLC[{plc.Name}]写入线程",
                        new ThreadTaskCancelSignal(),
                        (cancelSignal) =>
                        {
                            var message = "";
                            var addressName = "";
                            while (!cancelSignal.CancelSignal)
                            {
                                Thread.Sleep(plc.CycleWriteTime);
                                if (!plc.IsConnect)
                                {
                                    if (plc.TryConnect(out message))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        _logger.Error(message ?? "连接错误");
                                        Thread.Sleep(5000);
                                        continue;
                                    }
                                }

                                try
                                {
                                    // PLC写入
                                    if (IsCycleWritePLC)
                                    {
                                        foreach (
                                            var plcRWConfig in PLCRWConfigModels.Where(it =>
                                                it.PLCID == plc.ID && it.RWMode.ToUpper() == "W"
                                            )
                                        )
                                        {
                                            foreach (
                                                var plcRWAddress in PLCRWAddressTable!.Where(it =>
                                                    it.AddressRangeId == plcRWConfig.ID
                                                    && it.Index < plcRWConfig.Length
                                                )
                                            )
                                            {
                                                addressName = plcRWAddress.ParameterName;
                                                OperateResult result = plc.Write(
                                                    plcRWAddress.Address,
                                                    plcRWAddress.Type,
                                                    plcRWAddress.MonitorValue
                                                );
                                                if (!result.IsSuccess)
                                                {
                                                    _logger.Error(
                                                        $"PLC地址[{addressName}]写入数据失败,地址: {plcRWAddress.Address},类型: {plcRWAddress.Type},值: {plcRWAddress.MonitorValue},原因: {result.Message}"
                                                    );
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error($"PLC地址写入错误", ex);
                                    Thread.Sleep(3000);
                                }
                            }
                        }
                    )
                    .Start();
            }
        }

        private static dynamic? ConvertByBytes(
            byte[] bytes,
            string? dataType,
            IByteTransform? byteTransform,
            int addressWordLength,
            int index,
            int variableLength = 1
        )
        {
            dynamic? resultValue = null;
            switch (dataType?.ToUpper())
            {
                case "STRING":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransString(
                            bytes,
                            index * addressWordLength,
                            variableLength,
                            Encoding.ASCII
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransString(
                            bytes,
                            index * addressWordLength,
                            variableLength,
                            Encoding.ASCII
                        );
                    }
                    break;
                case "FLOAT":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransSingle(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransSingle(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                case "DOUBLE":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransDouble(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransDouble(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                case "BYTE":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransByte(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransByte(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                case "SHORT":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransInt16(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransInt16(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                case "USHORT":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransUInt16(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransUInt16(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                case "INT":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransInt32(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransInt32(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                case "UINT":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransUInt32(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransUInt32(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                case "LONG":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransInt64(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransInt64(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                case "ULONG":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransUInt64(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransUInt64(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                case "BOOL":
                    if (variableLength == 1)
                    {
                        resultValue = byteTransform?.TransBool(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        )[0];
                    }
                    else
                    {
                        resultValue = byteTransform?.TransBool(
                            bytes,
                            index * addressWordLength,
                            variableLength
                        );
                    }
                    break;
                default:
                    break;
            }
            return resultValue;
        }

        /// <summary>
        /// 字符串转PLC品牌_规格_协议
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>PLC品牌_规格_协议</returns>
        /// <exception cref="FormatException">字符串格式错误</exception>
        public static PLCBrandSpecificationProtocal ToPLCBrandSpecificationProtocal(string? s)
        {
            s = s?.ToUpper();
            switch (s)
            {
                case "SIEMENS_S7NETS200":
                    return PLCBrandSpecificationProtocal.Siemens_S7NetS200;
                case "SIEMENS_S7NETS200SMART":
                    return PLCBrandSpecificationProtocal.Siemens_S7NetS200Smart;
                case "SIEMENS_S7NETS300":
                    return PLCBrandSpecificationProtocal.Siemens_S7NetS300;
                case "SIEMENS_S7NETS400":
                    return PLCBrandSpecificationProtocal.Siemens_S7NetS400;
                case "SIEMENS_S7NETS1200":
                    return PLCBrandSpecificationProtocal.Siemens_S7NetS1200;
                case "SIEMENS_S7NETS1500":
                    return PLCBrandSpecificationProtocal.Siemens_S7NetS1500;
                case "OMRON_FINS_TCP":
                    return PLCBrandSpecificationProtocal.Omron_Fins_TCP;
                case "MELSEC_MC_BINARY":
                    return PLCBrandSpecificationProtocal.Melsec_MC_Binary;
                case "MELSEC_MC_ASCII":
                    return PLCBrandSpecificationProtocal.Melsec_MC_Ascii;
                case "MELSEC_A1E_Binary":
                    return PLCBrandSpecificationProtocal.Melsec_A1E_Binary;
                case "PANASONIC_MEWTOCOL_OVERTCP":
                    return PLCBrandSpecificationProtocal.Panasonic_Mewtocol_OverTcp;
                case "KEYENCE_MC_BINARY":
                    return PLCBrandSpecificationProtocal.Keyence_MC_Binary;
                case "KEYENCE_MC_ASCII":
                    return PLCBrandSpecificationProtocal.Keyence_MC_Ascii;
                case "INOVANCE_AM_TCP":
                    return PLCBrandSpecificationProtocal.Inovance_AM_Tcp;
                case "INOVANCE_H3U_TCP":
                    return PLCBrandSpecificationProtocal.Inovance_H3U_Tcp;
                case "INOVANCE_H5U_TCP":
                    return PLCBrandSpecificationProtocal.Inovance_H5U_Tcp;
                case "MODBUS_TCP":
                    return PLCBrandSpecificationProtocal.Modbus_Tcp;
                default:
                    throw new FormatException("未找到PLC型号");
            }
        }

        /// <summary>
        /// 字符串转数据排列规则
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>数据排列规则</returns>
        public static DataSortRule ToDataSortRule(string? s)
        {
            s = s?.ToUpper();
            switch (s)
            {
                case "ABCD":
                    return DataSortRule.ABCD;
                case "BADC":
                    return DataSortRule.BADC;
                case "CDAB":
                    return DataSortRule.CDAB;
                case "DCBA":
                    return DataSortRule.DCBA;
                default:
                    return DataSortRule.Default;
            }
        }

        public static int GetAddressNumber(string source)
        {
            int result = 0;
            if (source.Contains("DB"))
            {
                source = Regex.Replace(source, @"[^\d.\d]", "");
                if (source.Split('.').Length >= 2) //西门子定义报警时，DB2.3.2
                {
                    result = (int)Math.Floor(decimal.Parse(source.Split('.')[1]));
                }
            }
            else
            {
                source = Regex.Replace(source, @"[^\d.\d]", "");
                if (string.IsNullOrWhiteSpace(source))
                    return 0;
                if (Regex.IsMatch(source, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = (int)Math.Floor(decimal.Parse(source));
                }
            }

            return result;
        }

        /// <summary>
        /// 初始化PLC
        /// </summary>
        public async Task<bool> InitPlcs()
        {
            try
            {
                _logger.Info("PLC开始初始化");
                HSLAuthorization();
                var plcInfoList = await _pLcConfigRepository.GetEnabledConfigsAsync();
                var plcCount = 0;
                var plcAddressCount = 0;
                foreach (var plcInfo in plcInfoList)
                {
                    if (plcInfo.IsEnable)
                    {
                        var plcBrandSpecificationProtocal = ToPLCBrandSpecificationProtocal(
                            plcInfo.BrandSpecificationProtocal
                        );
                        var dataSortRule = ToDataSortRule(plcInfo.DataSortRule);
                        AddPLC(
                            new PLC(
                                plcInfo.ID,
                                plcInfo.Name,
                                plcBrandSpecificationProtocal,
                                plcInfo.IP,
                                plcInfo.Port,
                                dataSortRule,
                                plcInfo.CycleReadTime,
                                plcInfo.CycleWriteTime
                            )
                        );
                        plcCount++;

                        var plcRWConfigList = await _plcRWConfigRepository.GetEnabledConfigsAsync(
                            plcInfo.ID
                        );
                        PLCRWConfigModels.AddRange(plcRWConfigList);
                        var plcAddressConfigList =
                            await _plcAddressConfigRepository.GetMonitorAddressesAsync(plcInfo.ID);
                        foreach (var plcAddressConfig in plcAddressConfigList)
                        {
                            var plcRWConfig = plcRWConfigList.FirstOrDefault(it =>
                                it.ID == plcAddressConfig.PLCRWID
                            );
                            if (plcAddressConfig.IsMonitor && plcRWConfig is not null)
                            {
                                PLCRWAddressTable!.Add(
                                    new PLCRWAddress(
                                        plcAddressConfig.ID,
                                        plcAddressConfig.PLCID,
                                        plcAddressConfig.PLCRWID,
                                        plcAddressConfig.CategoryID,
                                        plcAddressConfig.ParameterName,
                                        plcAddressConfig.Type,
                                        plcAddressConfig.Length,
                                        plcAddressConfig.Address,
                                        plcAddressConfig.Unit,
                                        plcAddressConfig.Remark,
                                        GetAddressNumber(plcAddressConfig.Address)
                                            - plcRWConfig.StartAddress.ToInt()
                                    )
                                );
                                plcAddressCount++;
                            }
                        }
                    }
                }
                _logger.Info($"加载PLC设备数量：{plcCount}");
                _logger.Info($"加载PLC地址数量：{plcAddressCount}");

                PLCConnect();

                ThreadManager
                    .AddThread(
                        "PLC状态监控线程",
                        new ThreadTaskCancelSignal(),
                        (cancelSignal) =>
                        {
                            foreach (var plc in PLCs)
                            {
                                ConnectInfos!.Add(new ConnectInfo(plc.Name, plc.IsConnect));
                            }
                            while (!cancelSignal.CancelSignal)
                            {
                                foreach (var plc in PLCs)
                                {
                                    ConnectInfos!
                                        .FirstOrDefault(it => it.Name == plc.Name)!
                                        .Status = plc.IsConnect;
                                }
                                Thread.Sleep(100);
                            }
                        }
                    )
                    .Start();

                _logger.Info("初始化PLC完成");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("初始化PLC失败", ex);
                return false;
            }
        }
    }
}
