
namespace SUNWODA_SEVB.Core.Entities
{
    public class ProjectSetting
    {
        public int ID { get; set; }

        public string Name { get; set; } = null!;

        public string BelongToVM { get; set; } = null!;

        public string Value { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public string Remark { get; set; } = null!;
        public ProjectSetting() { }
        public ProjectSetting(string name, string belongToVM, string value, string type)
        {
            Name = name;
            BelongToVM = belongToVM;
            Value = value;
            Type = type;
        }
    }
}