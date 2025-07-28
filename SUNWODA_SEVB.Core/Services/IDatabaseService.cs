using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Core.Services
{
    public interface IDatabaseService
    {
        bool Initialize();
        bool TestConnection();
    }
}
