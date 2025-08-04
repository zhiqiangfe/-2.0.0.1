
namespace SUNWODA_SEVB.Core.Entities 
{

    public class WorkSpaceProjectModel
    {
        public int ID { get; set; }

        public string VMClassName { get; set; } = null!;

        public bool IsEnabled { get; set; }

        public bool IsInitShow { get; set; }
        public WorkSpaceProjectModel() { }
        public WorkSpaceProjectModel(string vmClassName, bool isEnabled, bool isInitShow)
        {
            VMClassName = vmClassName;
            IsEnabled = isEnabled;
            IsInitShow = isInitShow;
        }
    }
}
