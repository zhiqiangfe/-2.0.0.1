using SUNWODA_SEVB.Core.Models;
using SUNWODA_SEVB.Tool.Helper;
using System.IO;

namespace SUNWODA_SEVB.Tool.Configuration
{
    /// <summary>
    /// 配置文件生成器
    /// 用于生成加密的appsettings.json文件
    /// </summary>
    public static class ConfigurationGenerator
    {
        /// <summary>
        /// 生成默认的加密配置文件
        /// </summary>
        public static void GenerateDefaultEncryptedConfig()
        {
            var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            // 创建默认配置
            var appSettings = new AppSettings
            {
                ConnectionStrings = new ConnectionStrings
                {
                    DefaultConnection = "server=127.0.0.1;database=sunwoda_demo;uid=root;pwd=root;Port=3306;Min Pool Size=5;Max Pool Size=100;Persist Security Info=True;SslMode=None;AllowPublicKeyRetrieval=True;"
                },
                ProjectSettings = new ProjectSettings
                {
                    EnableMES = true
                }
            };

            // 生成加密文件
            JsonHelper.WriteJsonFile(outputPath, appSettings, encrypt: true);

            Console.WriteLine($"加密配置文件已生成: {outputPath}");
        }

        /// <summary>
        /// 将现有的未加密配置文件转换为加密格式
        /// </summary>
        public static void ConvertToEncrypted(string sourcePath, string? outputPath = null)
        {
            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException($"源文件不存在: {sourcePath}");
            }

            // 如果没有指定输出路径，使用默认路径
            outputPath ??= Path.Combine(
                Path.GetDirectoryName(sourcePath) ?? "",
                Path.GetFileNameWithoutExtension(sourcePath) + ".encrypted" + Path.GetExtension(sourcePath)
            );

            // 读取原始配置
            var content = File.ReadAllText(sourcePath);

            // 检查是否已加密
            if (JsonHelper.IsEncrypted(content))
            {
                Console.WriteLine("文件已经是加密状态");
                return;
            }

            // 反序列化为对象
            var appSettings = JsonHelper.Deserialize<AppSettings>(content);

            if (appSettings == null)
            {
                throw new InvalidOperationException("无法解析配置文件");
            }

            // 写入加密文件
            JsonHelper.WriteJsonFile(outputPath, appSettings, encrypt: true);

            Console.WriteLine($"加密配置文件已生成: {outputPath}");
        }

        /// <summary>
        /// 解密配置文件（用于调试）
        /// </summary>
        public static void DecryptConfig(string encryptedPath, string? outputPath = null)
        {
            if (!File.Exists(encryptedPath))
            {
                throw new FileNotFoundException($"加密文件不存在: {encryptedPath}");
            }

            // 如果没有指定输出路径，使用默认路径
            outputPath ??= Path.Combine(
                Path.GetDirectoryName(encryptedPath) ?? "",
                Path.GetFileNameWithoutExtension(encryptedPath) + ".decrypted" + Path.GetExtension(encryptedPath)
            );

            // 读取加密的配置
            var appSettings = JsonHelper.ReadJsonFile<AppSettings>(encryptedPath);

            if (appSettings == null)
            {
                throw new InvalidOperationException("无法解密配置文件");
            }

            // 写入未加密的文件
            JsonHelper.WriteJsonFile(outputPath, appSettings, encrypt: false);

            Console.WriteLine($"解密配置文件已生成: {outputPath}");
        }

        /// <summary>
        /// 从代码生成配置文件（避免手动编辑）
        /// </summary>
        public static void GenerateConfigFromCode(
            string connectionString,
            bool enableMES,
            string outputPath,
            bool encrypt = true)
        {
            var appSettings = new AppSettings
            {
                ConnectionStrings = new ConnectionStrings
                {
                    DefaultConnection = connectionString
                },
                ProjectSettings = new ProjectSettings
                {
                    EnableMES = enableMES
                }
            };

            // 写入文件
            JsonHelper.WriteJsonFile(outputPath, appSettings, encrypt);

            Console.WriteLine($"{(encrypt ? "加密" : "未加密")}配置文件已生成: {outputPath}");
        }

        /// <summary>
        /// 验证配置文件是否可以正确读取
        /// </summary>
        public static bool ValidateConfigFile(string configPath)
        {
            try
            {
                if (!File.Exists(configPath))
                {
                    Console.WriteLine($"配置文件不存在: {configPath}");
                    return false;
                }

                // 尝试读取配置
                var appSettings = JsonHelper.ReadJsonFile<AppSettings>(configPath);

                if (appSettings == null)
                {
                    Console.WriteLine("无法读取配置文件");
                    return false;
                }

                // 验证必要的配置项
                if (string.IsNullOrEmpty(appSettings.ConnectionStrings?.DefaultConnection))
                {
                    Console.WriteLine("缺少数据库连接字符串");
                    return false;
                }

                var isEncrypted = JsonHelper.IsFileEncrypted(configPath);
                Console.WriteLine($"配置文件验证成功 - 加密状态: {(isEncrypted ? "已加密" : "未加密")}");
                Console.WriteLine($"数据库连接: {appSettings.ConnectionStrings.DefaultConnection}");
                Console.WriteLine($"MES启用: {appSettings.ProjectSettings?.EnableMES}");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"验证配置文件时出错: {ex.Message}");
                return false;
            }
        }
    }

    /// <summary>
    /// 配置管理器 - 用于在运行时更新配置
    /// </summary>
    public class ConfigurationManager
    {
        private readonly string _configPath;
        private readonly bool _alwaysEncrypt;

        public ConfigurationManager(string configPath, bool alwaysEncrypt = true)
        {
            _configPath = configPath;
            _alwaysEncrypt = alwaysEncrypt;
        }

        /// <summary>
        /// 更新连接字符串
        /// </summary>
        public void UpdateConnectionString(string newConnectionString)
        {
            // 读取当前配置
            var appSettings = JsonHelper.ReadJsonFile<AppSettings>(_configPath) ?? new AppSettings();

            // 更新连接字符串
            appSettings.ConnectionStrings.DefaultConnection = newConnectionString;

            // 保存配置（保持加密状态）
            var shouldEncrypt = _alwaysEncrypt || JsonHelper.IsFileEncrypted(_configPath);
            JsonHelper.WriteJsonFile(_configPath, appSettings, shouldEncrypt);
        }

        /// <summary>
        /// 更新MES启用状态
        /// </summary>
        public void UpdateMesEnabled(bool enabled)
        {
            // 读取当前配置
            var appSettings = JsonHelper.ReadJsonFile<AppSettings>(_configPath) ?? new AppSettings();

            // 更新MES设置
            appSettings.ProjectSettings.EnableMES = enabled;

            // 保存配置（保持加密状态）
            var shouldEncrypt = _alwaysEncrypt || JsonHelper.IsFileEncrypted(_configPath);
            JsonHelper.WriteJsonFile(_configPath, appSettings, shouldEncrypt);
        }

        /// <summary>
        /// 获取当前配置
        /// </summary>
        public AppSettings? GetCurrentSettings()
        {
            return JsonHelper.ReadJsonFile<AppSettings>(_configPath);
        }
    }
}
