namespace HTHIUM.Core.Models.Data
{
    public class ProjectSettingModel
    {
        public int ID { get; set; }

        public string Name { get; set; } = null!;

        public string BelongToVM { get; set; } = null!;

        public string Value { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public int RoleRank { get; set; }

        public string Remark { get; set; } = null!;
        public ProjectSettingModel() { }
        public ProjectSettingModel(string name, string belongToVM, string value, string type, int roleRank)
        {
            Name = name;
            BelongToVM = belongToVM;
            Value = value;
            Type = type;
            RoleRank = roleRank;
        }
    }
}