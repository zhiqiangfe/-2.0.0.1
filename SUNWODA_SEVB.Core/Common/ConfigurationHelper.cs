using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace SUNWODA_SEVB.Core.Common
{
    public static class ConfigurationHelper
    {
        private static IConfiguration? _configuration;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(GetBasePath())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }

        /// <summary>
        /// 允许外部设置配置（用于统一配置源）
        /// </summary>
        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 获取正确的基础路径
        /// </summary>
        private static string GetBasePath()
        {
            // 对于WPF应用，使用应用程序基目录而不是当前目录
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string GetConnectionString(string name = "DefaultConnection")
        {
            return Configuration.GetConnectionString(name) ?? string.Empty;
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        public static string GetValue(string key)
        {
            return Configuration[key] ?? string.Empty;
        }

        /// <summary>
        /// 获取配置节
        /// </summary>
        public static T GetSection<T>(string sectionName) where T : class, new()
        {
            var section = new T();
            Configuration.GetSection(sectionName).Bind(section);
            return section;
        }
    }
}