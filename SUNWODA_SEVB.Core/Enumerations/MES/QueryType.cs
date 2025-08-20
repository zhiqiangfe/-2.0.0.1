using System.ComponentModel;

namespace SUNWODA_SEVB.Core.Enumerations.MES
{
    /// <summary>
    /// 查询类型枚举
    /// </summary>
    public enum QueryType
    {
        [Description("通过父条码查子条码")]
        ByParent = 1,

        [Description("查询本身")]
        Self = 2
    }
}
