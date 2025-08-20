using SUNWODA_SEVB.Tool.Extension;
using System.Windows;

namespace SUNWODA_SEVB.Tool.Helper
{
    /// <summary>
    /// 数据类型转换助手
    /// </summary>
    public static class DataTypeConverter
    {
        /// <summary>
        /// 将字符串转换为指定类型的值
        /// </summary>
        public static dynamic StringToValue(string type, string value)
        {
            type = type.ToUpper();
            return type switch
            {
                "STRING" => value,
                "FLOAT" => value.ToFloat(), // 推荐使用double处理小数（计算机处理float的精度问题）
                "DOUBLE" => value.ToDouble(),
                "DECIMAL" => value.ToDecimal(), // 指针只读
                "SBYTE" => value.ToSByte(),
                "BYTE" => value.ToByte(),
                "SHORT" => value.ToShort(),
                "USHORT" => value.ToUShort(),
                "INT" or "INTEGER" => value.ToInt(),
                "UINT" => value.ToUInt(),
                "LONG" => value.ToLong(),
                "ULONG" => value.ToULong(),
                "BOOL" or "BOOLEAN" => value.ToBool(),
                "DATETIME" => value.ToDateTime("yyyy-MM-dd HH:mm:ss"),
                "WINDOWSTATE" => value.ToEnum<WindowState>(),
                "HORIZONTALALIGNMENT" => value.ToEnum<HorizontalAlignment>(),
                "VERTICALALIGNMENT" => value.ToEnum<VerticalAlignment>(),
                _ => throw new FormatException($"不支持的数据类型: {type}"),
            };
        }

        /// <summary>
        /// 将值转换为SQL存储的字符串
        /// </summary>
        public static string ValueToString(dynamic value)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}
