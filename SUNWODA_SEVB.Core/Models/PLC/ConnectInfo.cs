using SUNWODA_SEVB.Core.Common;

namespace SUNWODA_SEVB.Core.Models.PLC
{
    public class ConnectInfo : ModelBase
    {
        private string? name;
        public string? Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private bool status;
        public bool Status
        {
            get => status;
            set
            {
                SetProperty(ref status, value);
                StatusName = status ? "Wifi" : "WifiOff";
            }
        }

        public string statusName = "WifiOff";
        public string StatusName
        {
            get => statusName;
            set => SetProperty(ref statusName, value);
        }

        public ConnectInfo(string connectName, bool connectStatus)
        {
            Name = connectName;
            Status = connectStatus;
        }
    }
}
