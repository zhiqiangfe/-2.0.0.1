using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.Net;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.Omron;
using HslCommunication.Profinet.Siemens;
using SUNWODA_SEVB.PLC.Enumerations;

namespace SUNWODA_SEVB.PLC.Core
{
    public class PLC
    {
        private DataSortRule byteDataSortRule;
        public object CommunicationObjLock = new object();

        /// <summary>
        /// 编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 品牌_型号_通讯协议
        /// </summary>
        public PLCBrandSpecificationProtocal BrandSpecificationProtocal { get; set; }

        /// <summary>
        /// 数据格式
        /// </summary>
        internal DataFormat? ByteDataFormat { get; private set; } = null;

        /// <summary>
        /// 数据排列规则
        /// </summary>
        public DataSortRule ByteDataSortRule
        {
            get => byteDataSortRule;
            set
            {
                byteDataSortRule = value;
                switch (byteDataSortRule)
                {
                    case DataSortRule.Default:
                        ByteDataFormat = null;
                        break;
                    case DataSortRule.ABCD:
                        ByteDataFormat = DataFormat.ABCD;
                        break;
                    case DataSortRule.BADC:
                        ByteDataFormat = DataFormat.BADC;
                        break;
                    case DataSortRule.CDAB:
                        ByteDataFormat = DataFormat.CDAB;
                        break;
                    case DataSortRule.DCBA:
                        ByteDataFormat = DataFormat.DCBA;
                        break;
                }
            }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public DeviceTcpNet? Device { get; set; }

        /// <summary>
        /// 上位机读取指令一次最低读取多少个字符。
        /// 西门子PLC一次最少读取一个字节，为1个字符；
        /// 三菱PLC一次最少读取一个寄存器，也就是2个字符
        /// </summary>
        public int AddressWordLength { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnect { get; set; }

        /// <summary>
        /// 二进制数据转换器
        /// </summary>
        public IByteTransform? ByteTransform { get; set; }

        /// <summary>
        /// 循环读取时间(ms)
        /// </summary>
        public int CycleReadTime { get; set; }

        /// <summary>
        /// 循环写入时间(ms)
        /// </summary>
        public int CycleWriteTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">PLC编号</param>
        /// <param name="name">PLC名称</param>
        /// <param name="brandSpecificationProtocal">品牌_规格_协议，如：Omron_Fins_TCP</param>
        /// <param name="ip">IP地址</param>
        /// <param name="byteDataSortRule">数据排列规则</param>
        /// <param name="cycleReadTime">循环读写时间</param>
        public PLC(
            int id,
            string name,
            PLCBrandSpecificationProtocal brandSpecificationProtocal,
            string ip,
            DataSortRule byteDataSortRule = DataSortRule.Default,
            int cycleReadTime = 500,
            int cycleWriteTime = 500
        )
        {
            ID = id;
            Name = name;
            BrandSpecificationProtocal = brandSpecificationProtocal;
            IP = ip;
            ByteDataSortRule = byteDataSortRule;
            if (cycleReadTime < 25)
                cycleReadTime = 25;
            CycleReadTime = cycleReadTime;
            if (cycleWriteTime < 25)
                cycleWriteTime = 25;
            CycleWriteTime = cycleWriteTime;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">PLC编号</param>
        /// <param name="name">PLC名称</param>
        /// <param name="brandSpecificationProtocal">品牌_规格_协议，如：Omron_Fins_TCP</param>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="byteDataSortRule">数据排列规则</param>
        /// <param name="cycleReadTime">循环读写时间</param>
        public PLC(
            int id,
            string name,
            PLCBrandSpecificationProtocal brandSpecificationProtocal,
            string ip,
            int port,
            DataSortRule byteDataSortRule = DataSortRule.Default,
            int cycleReadTime = 500,
            int cycleWriteTime = 500
        )
            : this(id, name, brandSpecificationProtocal, ip, byteDataSortRule, cycleReadTime, cycleWriteTime)
        {
            Port = port;
        }

        ~PLC()
        {
            Dispose();
        }

        private void Dispose()
        {
            if (Device == null)
                return;
            Device.ConnectClose();
            Device.Dispose();
        }

        /// <summary>
        /// 尝试重连
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TryConnect(out string? message)
        {
            message = "";
            if (!IsConnect)
            {
                var result = Device?.ConnectServer();
                message = result?.ToMessageShowString();
                IsConnect = result?.IsSuccess ?? false;
            }
            return IsConnect;
        }

        public OperateResult<byte[]>? Read(string areaStartAddress, ushort length)
        {
            lock (CommunicationObjLock)
            {
                OperateResult<byte[]>? result = Device?.Read(areaStartAddress, length);
                IsConnect = result?.IsSuccess ?? false;
                if (!IsConnect)
                    throw new Exception($"读取PLC字节数组失败，原因：{result?.Message}");
                    //LogHelper.Error($"读取PLC字节数组失败，原因：{result?.Message}");
                return IsConnect ? result : null;
            }
        }

        public OperateResult? Write(string address, string valueType, dynamic value)
        {
            OperateResult? result = new OperateResult();
            if (value != null)
            {
                lock (CommunicationObjLock)
                {
                    if (
                        TypeKeywordNameToStructName(valueType) == value.GetType().Name.ToUpper()
                        || valueType.ToUpper() == value.GetType().Name.ToUpper()
                    )
                    {
                        result = Device?.Write(address, value);
                        IsConnect = result?.IsSuccess ?? false;
                    }
                }
            }
            return result;
        }

        private string TypeKeywordNameToStructName(string s)
        {
            string resultValue = "STRING";
            switch (s.ToUpper())
            {
                case "FLOAT":
                    resultValue = "SINGLE";
                    break;
                case "DOUBLE":
                    resultValue = "DOUBLE";
                    break;
                case "BYTE":
                    resultValue = "BYTE";
                    break;
                case "SHORT":
                    resultValue = "INT16";
                    break;
                case "USHORT":
                    resultValue = "UINT16";
                    break;
                case "INT":
                    resultValue = "INT32";
                    break;
                case "UINT":
                    resultValue = "UINT32";
                    break;
                case "LONG":
                    resultValue = "INT64";
                    break;
                case "ULONG":
                    resultValue = "UINT64";
                    break;
                case "BOOL":
                    resultValue = "BOOLEAN";
                    break;
                case "STRING":
                default:
                    break;
            }
            return resultValue;
        }
    }
}
