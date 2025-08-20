using System.ComponentModel;

namespace SUNWODA_SEVB.Core.Enumerations.MES
{
    /// <summary>
    /// MES测试结果枚举
    /// </summary>
    public enum MesTestResult
    {
        [Description("OK")]
        OK = 0,

        [Description("NG")]
        NG = 1,

        [Description("未知")]
        Unknown = -1
    }
}
