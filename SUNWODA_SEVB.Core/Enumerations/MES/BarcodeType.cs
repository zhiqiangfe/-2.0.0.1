using System.ComponentModel;

namespace SUNWODA_SEVB.Core.Enumerations.MES
{
    /// <summary>
    /// 条码类型枚举
    /// </summary>
    public enum BarcodeType
    {
        [Description("上盖码")]
        Cover,

        [Description("电池包条码")]
        Pack,

        [Description("重保码")]
        ReinCode,

        [Description("箱体码")]
        Box,

        [Description("模组码")]
        Module,

        [Description("电芯条码")]
        Cell
    }
}
