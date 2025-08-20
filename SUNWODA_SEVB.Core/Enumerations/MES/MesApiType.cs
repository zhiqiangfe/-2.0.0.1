using System.ComponentModel;

namespace SUNWODA_SEVB.Core.Enumerations.MES
{
    /// <summary>
    /// MES API类型枚举
    /// </summary>
    public enum MesApiType
    {
        [Description("客户端API")]
        Client,

        [Description("EVC/EVB API")]
        EVCEVB,

        [Description("IME API")]
        IME,

        [Description("Web API")]
        Web,

        [Description("DIPS API")]
        DIPS
    }
}
