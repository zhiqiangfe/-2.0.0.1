namespace HTHIUM.Core.Models.Data
{
    public class GlobalSettingModel
    {
        public int ID { get; set; }

        public string Name { get; set; } = null!;

        public string Value { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public int RoleRank { get; set; }

        public string Remark { get; set; } = null!;
        public GlobalSettingModel() { }

        public GlobalSettingModel(string name, string value, string type, int roleRank)
        {
            Name = name;
            Value = value;
            Type = type;
            RoleRank = roleRank;
        }
    }
}
