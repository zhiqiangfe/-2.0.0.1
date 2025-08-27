
namespace SUNWODA_SEVB.Core.Models.Data
{
    public class MESSettingModel
    {
        public int ID { get; set; }
        public string ProfileName { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Remark { get; set; } = null!;
        public MESSettingModel() { }
    }
}
