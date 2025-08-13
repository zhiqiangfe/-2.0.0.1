using System.Globalization;
using System.Windows;

namespace SUNWODA_SEVB.Tool.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 字符串转单精度浮点数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>单精度浮点数</returns>
        public static float ToFloat(this string s)
        {
            return float.TryParse(s, out float result) ? result : float.NaN;
        }

        /// <summary>
        /// 字符串转双精度浮点数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>双精度浮点数</returns>
        public static double ToDouble(this string s)
        {
            return double.TryParse(s, out double result) ? result : double.NaN;
        }

        /// <summary>
        /// 字符串转十进制浮点数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>十进制浮点数</returns>
        public static decimal ToDecimal(this string s)
        {
            return decimal.TryParse(s, out decimal result) ? result : 0;
        }

        /// <summary>
        /// 字符串转8位有符号整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>8位有符号整数</returns>
        public static sbyte ToSByte(this string s)
        {
            return sbyte.TryParse(s, out sbyte result) ? result : (sbyte)0;
        }

        /// <summary>
        /// 字符串转8位无符号整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>8位无符号整数</returns>
        public static byte ToByte(this string s)
        {
            return byte.TryParse(s, out byte result) ? result : (byte)0;
        }

        /// <summary>
        /// 字符串转16位有符号整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>16位有符号整数</returns>
        public static short ToShort(this string s)
        {
            return short.TryParse(s, out short result) ? result : (short)0;
        }

        /// <summary>
        /// 字符串转16位无符号整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>16位无符号整数</returns>
        public static ushort ToUShort(this string s)
        {
            return ushort.TryParse(s, out ushort result) ? result : (ushort)0;
        }

        /// <summary>
        /// 字符串转32位有符号整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>整数</returns>
        public static int ToInt(this string s)
        {
            return int.TryParse(s, out int result) ? result : 0;
        }

        /// <summary>
        /// 字符串转32位无符号整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>32位无符号整数</returns>
        public static uint ToUInt(this string s)
        {
            return uint.TryParse(s, out uint result) ? result : 0;
        }

        /// <summary>
        /// 字符串转64位有符号整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>64位有符号整数</returns>
        public static long ToLong(this string s)
        {
            return long.TryParse(s, out long result) ? result : 0;
        }

        /// <summary>
        /// 字符串转64位无符号整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>64位无符号整数</returns>
        public static ulong ToULong(this string s)
        {
            return ulong.TryParse(s, out ulong result) ? result : 0;
        }

        /// <summary>
        /// 字符串转布尔值
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>布尔值</returns>
        public static bool ToBool(this string s)
        {
            return bool.TryParse(s, out bool result) ? result : false;
        }

        /// <summary>
        /// 字符串转时间
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="format">时间格式</param>
        /// <returns>时间</returns>
        public static DateTime ToDateTime(this string s, string format)
        {
            return DateTime.TryParseExact(
                s,
                format,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out DateTime result
            )
                ? result
                : DateTime.Now;
        }

        /// <summary>
        /// 字符串转窗口状态
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>窗口状态</returns>
        public static WindowState ToWindowSate(this string s)
        {
            WindowState mainWindowState = WindowState.Normal;
            switch (s.ToUpper())
            {
                case "MINIMUM":
                    mainWindowState = WindowState.Minimized;
                    break;
                case "MAXIMUM":
                    mainWindowState = WindowState.Maximized;
                    break;
                case "NORMAL":
                default:
                    break;
            }
            return mainWindowState;
        }

        /// <summary>
        /// 尾部追加
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="appendStr">追加字符串</param>
        /// <returns>追加后的字符串</returns>
        public static string AppendEnd(this string s, string appendStr)
        {
            return s + appendStr;
        }

        /// <summary>
        /// 头部追加
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="appendStr">追加字符串</param>
        /// <returns>追加后的字符串</returns>
        public static string AppendStart(this string s, string appendStr)
        {
            return appendStr + s;
        }
    }
}
