
namespace SUNWODA_SEVB.Core.Entities
{
    public class GlobalSetting
    {
        public int ID { get; set; }

        public string Name { get; set; } = null!;

        public string Value { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public string Remark { get; set; } = null!;
        public GlobalSetting() { }

        public GlobalSetting(string name, string value, string type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
