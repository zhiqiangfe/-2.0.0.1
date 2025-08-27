namespace SUNWODA_SEVB.Core.Models.Data
{

    public class WorkSpaceProjectModel
    {
        public int ID { get; set; }

        public string VMClassName { get; set; } = null!;

        public bool IsEnabled { get; set; }

        public WorkSpaceProjectModel() { }
        public WorkSpaceProjectModel(string vmClassName, bool isEnabled)
        {
            VMClassName = vmClassName;
            IsEnabled = isEnabled;
        }
    }
}
