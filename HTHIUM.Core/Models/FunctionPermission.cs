using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTHIUM.Core.Common;

namespace HTHIUM.Core.Models
{
    public class FunctionPermission : ModelBase
    {
        private string? functionName;
        public string? FunctionName
        {
            get => functionName;
            set => SetProperty(ref functionName, value);
        }

        private bool guest;
        public bool Guest
        {
            get => guest;
            set => SetProperty(ref guest, value);
        }

        private bool engineer;
        public bool Engineer
        {
            get => engineer;
            set => SetProperty(ref engineer, value);
        }

        private bool admin;
        public bool Admin
        {
            get => admin;
            set => SetProperty(ref admin, value);
        }
    }
}
