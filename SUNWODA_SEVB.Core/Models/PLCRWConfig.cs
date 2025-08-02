

namespace SUNWODA_SEVB.Core.Entities
{

    public class PLCRWConfig
    {       
        public int ID { get; set; }

        public string Name { get; set; } = null!;

        public int PLCID { get; set; }

        public string AreaName { get; set; } = null!;

        public string StartAddress { get; set; } = null!;

        public int Length { get; set; }

        public string RWMode { get; set; } = null!;

        public int Cycle { get; set; }

        public int AddressType { get; set; }

        public bool IsEnable { get; set; }

        public PLCRWConfig() { }

        public PLCRWConfig(
            string name,
            int plcId,
            string areaName,
            string startAddress,
            ushort length,
            string rwMode,
            int addressType,
            bool isEnable
        )
        {
            Name = name;
            PLCID = plcId;
            AreaName = areaName;
            StartAddress = startAddress;
            Length = length;
            RWMode = rwMode;
            AddressType = addressType;
            IsEnable = isEnable;
        }

    }
}
