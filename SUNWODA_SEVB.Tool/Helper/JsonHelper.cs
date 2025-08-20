using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SUNWODA_SEVB.Tool.Helper
{
    /// <summary>
    /// JSON序列化和反序列化帮助类
    /// </summary>
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions DefaultOptions = new()
        {
            // 关键配置：允许所有 Unicode 字符，不进行转义
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = null,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>
        /// 序列化对象为JSON字符串
        /// </summary>
        public static string Serialize<T>(T obj, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Serialize(obj, options ?? DefaultOptions);
        }

        /// <summary>
        /// 反序列化JSON字符串为对象
        /// </summary>
        public static T? Deserialize<T>(string json, JsonSerializerOptions? options = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonSerializer.Deserialize<T>(json, options ?? DefaultOptions);
        }

        /// <summary>
        /// 尝试反序列化JSON字符串
        /// </summary>
        public static bool TryDeserialize<T>(string json, out T? result, JsonSerializerOptions? options = null)
        {
            result = default;
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return false;

                result = JsonSerializer.Deserialize<T>(json, options ?? DefaultOptions);
                return result != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
