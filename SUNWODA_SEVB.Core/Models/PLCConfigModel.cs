
namespace SUNWODA_SEVB.Core.Entities
{

    public class PLCConfigModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public int DeviceID { get; set; }
        public string IP { get; set; } = null!;
        public int Port { get; set; }
        public string BrandSpecificationProtocal { get; set; } = null!;
        public string DataSortRule { get; set; } = null!;
        public int CycleReadTime { get; set; }
        public int CycleWriteTime { get; set; }
        public string Remark { get; set; } = null!;
        public bool IsEnable { get; set; }
        public PLCConfigModel()
        {
            CycleReadTime = 500;
            CycleWriteTime = 500;
        }
        public PLCConfigModel(string name, int deviceId, string ip, int port, string brandSpecificationProtocal, bool isEnable)
            : this()
        {
            Name = name;
            DeviceID = deviceId;
            IP = ip;
            Port = port;
            BrandSpecificationProtocal = brandSpecificationProtocal;
            IsEnable = isEnable;
        }
    }
}
