//using HslCommunication;
//using HslCommunication.Core;
//using SUNWODA_SEVB.PLC.Core;
//using SUNWODA_SEVB.PLC.Enumerations;
//using SUNWODA_SEVB.Tool.Helper;
//using System.Collections.ObjectModel;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace SUNWODA_SEVB.PLC
//{
//    public class PLCService
//    {
//        /// <summary>
//        /// PLC连接状态信息
//        /// </summary>
//        public static ObservableCollection<ConnectInfo> ConnectInfos { get; set; } = new ObservableCollection<ConnectInfo>();

//        /// <summary>
//        /// 初始化PLC
//        /// </summary>
//        public void InitPlcs()
//        {
//            PLCEngine.HSLAuthorization();
//            var plcInfoList = DataBaseService.Search<PlcConfig>();
//            var plcCount = 0;
//            foreach (var plcInfo in plcInfoList)
//            {
//                if (plcInfo.IsEnable)
//                {
//                    var plcBrandSpecificationProtocal = ToPLCBrandSpecificationProtocal(plcInfo.BrandSpecificationProtocal);
//                    var dataSortRule = ToDataSortRule(plcInfo.DataSortRule);
//                    PLCEngine.AddPLC(
//                        new PLC.Core.PLC(
//                            plcInfo.Id,
//                            plcInfo.Name,
//                            plcBrandSpecificationProtocal,
//                            plcInfo.IP,
//                            plcInfo.Port,
//                            dataSortRule,
//                            plcInfo.CycleReadTime,
//                            plcInfo.CycleWriteTime
//                        )
//                    );
//                    plcCount++;
//                }
//            }
//            //LogHelper.Info($"加载PLC设备数量：{plcCount}");
//            var plcRWAddressConfigList = DataBaseService.Search<PlcRWAddressConfig>();
//            var plcAddressRangeConfigList = DataBaseService.Search<PlcAddressRangeConfig>();
//            var plcRWAddressCount = 0;
//            foreach (var plcRWAddressConfig in plcRWAddressConfigList)
//            {
//                var plcAddressRangeConfig = plcAddressRangeConfigList.FirstOrDefault(it => it.Id == plcRWAddressConfig.AddressRangeId);
//                if (plcRWAddressConfig.IsMonitor)
//                {
//                    PLCEngine.PLCRWAddressTable.Add(
//                        new PLC.Core.PLCRWAddress(
//                            plcRWAddressConfig.Id,
//                            plcRWAddressConfig.PlcId,
//                            plcRWAddressConfig.AddressRangeId,
//                            plcRWAddressConfig.CategoryId,
//                            plcRWAddressConfig.ParameterName,
//                            plcRWAddressConfig.Type,
//                            plcRWAddressConfig.Length,
//                            plcRWAddressConfig.Address,
//                            plcRWAddressConfig.Unit,
//                            plcRWAddressConfig.Remark,
//                            GetAddressNumber(plcRWAddressConfig.Address) - plcAddressRangeConfig.StartAddress.ToInt()
//                        )
//                    );
//                    plcRWAddressCount++;
//                }
//            }
//            //LogHelper.Info($"加载PLC地址数量：{plcRWAddressCount}");
//            PLCConnect();

//            ThreadManager
//                .AddThread(
//                    "PLC状态监控线程",
//                    new ThreadTaskCancelSignal(),
//                    (cancelSignal) =>
//                    {
//                        foreach (var plc in PLCEngine.PLCs)
//                        {
//                            ConnectInfos.Add(new ConnectInfo(plc.Name, plc.IsConnect));
//                        }
//                        while (!cancelSignal.CancelSignal)
//                        {
//                            foreach (var plc in PLCEngine.PLCs)
//                            {
//                                ConnectInfos.FirstOrDefault(it => it.Name == plc.Name)!.Status = plc.IsConnect;
//                            }
//                            Thread.Sleep(100);
//                        }
//                    }
//                )
//                .Start();
//        }

//        /// <summary>
//        /// PLC连接
//        /// </summary>
//        public static void PLCConnect()
//        {
//            foreach (var plc in PLCEngine.PLCs)
//            {
//                ThreadManager
//                    .AddThread(
//                        $"PLC[{plc.Name}]读取线程",
//                        new ThreadTaskCancelSignal(),
//                        (cancelSignal) =>
//                        {
//                            var message = "";
//                            var addressRangeName = "";
//                            var addressName = "";
//                            while (!cancelSignal.CancelSignal)
//                            {
//                                Thread.Sleep(plc.CycleReadTime);
//                                if (!plc.IsConnect)
//                                {
//                                    if (plc.TryConnect(out message))
//                                    {
//                                        continue;
//                                    }
//                                    else
//                                    {
//                                        LogHelper.Error(message);
//                                        Thread.Sleep(5000);
//                                        continue;
//                                    }
//                                }

//                                try
//                                {
//                                    // PLC读取
//                                    if (ConfigService.GetGlobalConfig("IsCycleWritePLC").GetValue())
//                                    {
//                                        foreach (
//                                            var plcAddressRangeConfig in DataBaseService
//                                                .Search<PlcAddressRangeConfig>()
//                                                .Where(it => it.PlcId == plc.ID && it.RWMode.ToUpper() != "NOTRW")
//                                        )
//                                        {
//                                            addressRangeName =
//                                                $"{plcAddressRangeConfig.Name}:起始地址{plcAddressRangeConfig.AreaName}{plcAddressRangeConfig.StartAddress};长度{plcAddressRangeConfig.Length}";
//                                            var readContent = plc.Read(
//                                                $"{plcAddressRangeConfig.AreaName}{plcAddressRangeConfig.StartAddress}",
//                                                plcAddressRangeConfig.Length
//                                            );
//                                            if (readContent == null)
//                                                continue;
//                                            try
//                                            {
//                                                if (plcAddressRangeConfig.RWMode.ToUpper() == "R")
//                                                {
//                                                    foreach (
//                                                        var plcRWAddress in PLCEngine.PLCRWAddressTable.Where(it =>
//                                                            it.AddressRangeId == plcAddressRangeConfig.Id
//                                                        )
//                                                    )
//                                                    {
//                                                        addressName = plcRWAddress.ParameterName;
//                                                        plcRWAddress.MonitorValue = ConvertByBytes(
//                                                            readContent.Content,
//                                                            plcRWAddress.Type,
//                                                            plc.ByteTransform,
//                                                            plc.AddressWordLength,
//                                                            plcRWAddress.Index,
//                                                            plcRWAddress.Length
//                                                        );
//                                                    }
//                                                }
//                                            }
//                                            catch (Exception ex)
//                                            {
//                                                LogHelper.Error($"PLC地址[{addressName}]解析数据失败", ex);
//                                                Thread.Sleep(3000);
//                                            }
//                                        }
//                                    }
//                                }
//                                catch (Exception ex)
//                                {
//                                    LogHelper.Error($"PLC地址段[{addressRangeName}]读取数据失败", ex);
//                                    Thread.Sleep(3000);
//                                }
//                            }
//                        }
//                    )
//                    .Start();

//                ThreadManager
//                    .AddThread(
//                        $"PLC[{plc.Name}]写入线程",
//                        new ThreadTaskCancelSignal(),
//                        (cancelSignal) =>
//                        {
//                            var message = "";
//                            var addressName = "";
//                            while (!cancelSignal.CancelSignal)
//                            {
//                                Thread.Sleep(plc.CycleWriteTime);
//                                if (!plc.IsConnect)
//                                {
//                                    if (plc.TryConnect(out message))
//                                    {
//                                        continue;
//                                    }
//                                    else
//                                    {
//                                        LogHelper.Error(message);
//                                        Thread.Sleep(5000);
//                                        continue;
//                                    }
//                                }

//                                try
//                                {
//                                    // PLC写入
//                                    if (ConfigService.GetGlobalConfig("IsCycleWritePLC").GetValue())
//                                    {
//                                        foreach (
//                                            var plcAddressRangeConfig in DataBaseService
//                                                .Search<PlcAddressRangeConfig>()
//                                                .Where(it => it.PlcId == plc.ID && it.RWMode.ToUpper() == "W")
//                                        )
//                                        {
//                                            foreach (
//                                                var plcRWAddress in PLCEngine.PLCRWAddressTable.Where(it =>
//                                                    it.AddressRangeId == plcAddressRangeConfig.Id && it.Index < plcAddressRangeConfig.Length
//                                                )
//                                            )
//                                            {
//                                                addressName = plcRWAddress.ParameterName;
//                                                OperateResult result = plc.Write(plcRWAddress.Address, plcRWAddress.Type, plcRWAddress.MonitorValue);
//                                                if (!result.IsSuccess)
//                                                {
//                                                    LogHelper.Error(
//                                                        $"PLC地址[{addressName}]写入数据失败,地址: {plcRWAddress.Address},类型: {plcRWAddress.Type},值: {plcRWAddress.MonitorValue},原因: {result.Message}"
//                                                    );
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                                catch (Exception ex)
//                                {
//                                    LogHelper.Error($"PLC地址写入错误", ex);
//                                    Thread.Sleep(3000);
//                                }
//                            }
//                        }
//                    )
//                    .Start();
//            }
//        }

//        private static dynamic? ConvertByBytes(
//            byte[] bytes,
//            string? dataType,
//            IByteTransform? byteTransform,
//            int addressWordLength,
//            int index,
//            int variableLength = 1
//        )
//        {
//            dynamic? resultValue = null;
//            switch (dataType?.ToUpper())
//            {
//                case "STRING":
//                    resultValue = byteTransform?.TransString(bytes, index * addressWordLength, variableLength, Encoding.ASCII);
//                    break;
//                case "FLOAT":
//                        resultValue = byteTransform?.TransSingle(bytes, index * addressWordLength, variableLength);
//                    break;
//                case "DOUBLE":

//                        resultValue = byteTransform?.TransDouble(bytes, index * addressWordLength, variableLength);
//                    break;
//                case "BYTE":

//                        resultValue = byteTransform?.TransByte(bytes, index * addressWordLength, variableLength);
//                    break;
//                case "SHORT":
//                        resultValue = byteTransform?.TransInt16(bytes, index * addressWordLength, variableLength);
//                    break;
//                case "USHORT":
//                        resultValue = byteTransform?.TransUInt16(bytes, index * addressWordLength, variableLength);
//                    break;
//                case "INT":
//                        resultValue = byteTransform?.TransInt32(bytes, index * addressWordLength, variableLength);
//                    break;
//                case "UINT":
//                        resultValue = byteTransform?.TransUInt32(bytes, index * addressWordLength, variableLength);
//                    break;
//                case "LONG":
//                        resultValue = byteTransform?.TransInt64(bytes, index * addressWordLength, variableLength);
//                    break;
//                case "ULONG":
//                        resultValue = byteTransform?.TransUInt64(bytes, index * addressWordLength, variableLength);
//                    break;
//                case "BOOL":
//                        resultValue = byteTransform?.TransBool(bytes, index * addressWordLength, variableLength);
//                    break;
//                default:
//                    break;
//            }
//            return resultValue;
//        }

//        /// <summary>
//        /// 字符串转PLC品牌_规格_协议
//        /// </summary>
//        /// <param name="s">字符串</param>
//        /// <returns>PLC品牌_规格_协议</returns>
//        /// <exception cref="FormatException">字符串格式错误</exception>
//        public static PLCBrandSpecificationProtocal ToPLCBrandSpecificationProtocal(string? s)
//        {
//            s = s?.ToUpper();
//            switch (s)
//            {
//                case "SIEMENS_S7NETS200":
//                    return PLCBrandSpecificationProtocal.Siemens_S7NetS200;
//                case "SIEMENS_S7NETS200SMART":
//                    return PLCBrandSpecificationProtocal.Siemens_S7NetS200Smart;
//                case "SIEMENS_S7NETS300":
//                    return PLCBrandSpecificationProtocal.Siemens_S7NetS300;
//                case "SIEMENS_S7NETS400":
//                    return PLCBrandSpecificationProtocal.Siemens_S7NetS400;
//                case "SIEMENS_S7NETS1200":
//                    return PLCBrandSpecificationProtocal.Siemens_S7NetS1200;
//                case "SIEMENS_S7NETS1500":
//                    return PLCBrandSpecificationProtocal.Siemens_S7NetS1500;
//                case "OMRON_FINS_TCP":
//                    return PLCBrandSpecificationProtocal.Omron_Fins_TCP;
//                case "MELSEC_MC_BINARY":
//                    return PLCBrandSpecificationProtocal.Melsec_MC_Binary;
//                case "MELSEC_MC_ASCII":
//                    return PLCBrandSpecificationProtocal.Melsec_MC_Ascii;
//                case "MELSEC_A1E_Binary":
//                    return PLCBrandSpecificationProtocal.Melsec_A1E_Binary;
//                case "PANASONIC_MEWTOCOL_OVERTCP":
//                    return PLCBrandSpecificationProtocal.Panasonic_Mewtocol_OverTcp;
//                case "KEYENCE_MC_BINARY":
//                    return PLCBrandSpecificationProtocal.Keyence_MC_Binary;
//                case "KEYENCE_MC_ASCII":
//                    return PLCBrandSpecificationProtocal.Keyence_MC_Ascii;
//                case "INOVANCE_AM_TCP":
//                    return PLCBrandSpecificationProtocal.Inovance_AM_Tcp;
//                case "INOVANCE_H3U_TCP":
//                    return PLCBrandSpecificationProtocal.Inovance_H3U_Tcp;
//                case "INOVANCE_H5U_TCP":
//                    return PLCBrandSpecificationProtocal.Inovance_H5U_Tcp;
//                case "MODBUS_TCP":
//                    return PLCBrandSpecificationProtocal.Modbus_Tcp;
//                default:
//                    throw new FormatException("未找到PLC型号");
//            }
//        }

//        /// <summary>
//        /// 字符串转数据排列规则
//        /// </summary>
//        /// <param name="s">字符串</param>
//        /// <returns>数据排列规则</returns>
//        public static DataSortRule ToDataSortRule(string? s)
//        {
//            s = s?.ToUpper();
//            switch (s)
//            {
//                case "ABCD":
//                    return DataSortRule.ABCD;
//                case "BADC":
//                    return DataSortRule.BADC;
//                case "CDAB":
//                    return DataSortRule.CDAB;
//                case "DCBA":
//                    return DataSortRule.DCBA;
//                default:
//                    return DataSortRule.Default;
//            }
//        }

//        public static int GetAddressNumber(string source)
//        {
//            int result = 0;
//            if (source.Contains("DB"))
//            {
//                source = Regex.Replace(source, @"[^\d.\d]", "");
//                if (source.Split('.').Length >= 2) //西门子定义报警时，DB2.3.2
//                {
//                    result = (int)Math.Floor(decimal.Parse(source.Split('.')[1]));
//                }
//            }
//            else
//            {
//                source = Regex.Replace(source, @"[^\d.\d]", "");
//                if (string.IsNullOrWhiteSpace(source))
//                    return 0;
//                if (Regex.IsMatch(source, @"^[+-]?\d*[.]?\d*$"))
//                {
//                    result = (int)Math.Floor(decimal.Parse(source));
//                }
//            }

//            return result;
//        }

//        public static void UpdateRWAddress()
//        {
//            lock (PLCEngine.RWAddressTableLock)
//            {
//                PLCEngine.PLCRWAddressTable.Clear();
//                var plcRWAddressConfigList = DataBaseService.Search<PlcRWAddressConfig>();
//                var plcAddressRangeConfigList = DataBaseService.Search<PlcAddressRangeConfig>();
//                foreach (var plcRWAddressConfig in plcRWAddressConfigList)
//                {
//                    var plcAddressRangeConfig = plcAddressRangeConfigList.FirstOrDefault(it => it.Id == plcRWAddressConfig.AddressRangeId);
//                    if (plcRWAddressConfig.IsMonitor)
//                    {
//                        PLCEngine.PLCRWAddressTable.Add(
//                            new PLC.Core.PLCRWAddress(
//                                plcRWAddressConfig.Id,
//                                plcRWAddressConfig.PlcId,
//                                plcRWAddressConfig.AddressRangeId,
//                                plcRWAddressConfig.CategoryId,
//                                plcRWAddressConfig.ParameterName,
//                                plcRWAddressConfig.Type,
//                                plcRWAddressConfig.Length,
//                                plcRWAddressConfig.Address,
//                                plcRWAddressConfig.Unit,
//                                plcRWAddressConfig.Remark,
//                                GetAddressNumber(plcRWAddressConfig.Address) - plcAddressRangeConfig.StartAddress.ToInt()
//                            )
//                        );
//                    }
//                }
//            }
//        }
//    }
//}
