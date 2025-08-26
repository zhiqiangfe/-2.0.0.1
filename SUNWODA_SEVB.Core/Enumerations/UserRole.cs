using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Core.Enumerations
{
    public enum UserRole
    {
        Guest = 0b0,
        Engineer = 0b01,
        Admin = 0b10,
        SuperAdmin = 0b100
    }
}
