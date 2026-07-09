
namespace HTHIUM.Core.Interfaces.Data
{
    public static class GlobalSettingRepositoryExtensions
    {
        /// <summary>
        /// 获取配置值并转换为指定类型
        /// </summary>
        public static async Task<T> GetSettingValueAsync<T>(
            this IGlobalSettingRepository repository,
            string name,
            T defaultValue = default!)
        {
            try
            {
                var value = await repository.GetSettingValueAsync(name);

                if (value == null)
                    return defaultValue;

                // 特殊处理 string 类型
                if (typeof(T) == typeof(string))
                    return (T)(object)(value.ToString() ?? string.Empty);

                // 处理可空类型
                var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

                // 转换值
                return (T)Convert.ChangeType(value, targetType);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 同步版本 - 获取配置值并转换为指定类型
        /// </summary>
        public static T GetSettingValue<T>(
            this IGlobalSettingRepository repository,
            string name,
            T defaultValue = default!)
        {
            try
            {
                var value = repository.GetSettingValue(name);

                if (value == null)
                    return defaultValue;

                // 特殊处理 string 类型
                if (typeof(T) == typeof(string))
                    return (T)(object)(value.ToString() ?? string.Empty);

                // 处理可空类型
                var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

                // 转换值
                return (T)Convert.ChangeType(value, targetType);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
