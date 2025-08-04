
namespace SUNWODA_SEVB.Core.Entities

{
    public class DeviceModel
    {
        public int ID { get; set; }

        public string Number { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string BaseName { get; set; } = null!;

        public string LineName { get; set; } = null!;

        public string Remark { get; set; } = null!;
        public DeviceModel() { }

        public DeviceModel(string number, string name, string baseName, string lineName)
        {
            Number = number;
            Name = name;
            BaseName = baseName;
            LineName = lineName;
        }

    }
}
