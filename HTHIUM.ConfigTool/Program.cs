using HTHIUM.Tool.Configuration;
using HTHIUM.Tool.Helper;

namespace HTHIUM.ConfigTool
{
    /// <summary>
    /// 配置文件加密工具
    /// 可以作为独立的控制台程序运行，用于管理配置文件的加密
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("      SUNWODA 配置文件加密工具");
            Console.WriteLine("========================================");
            Console.WriteLine();

            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            GenerateNewEncryptedConfig();
                            break;
                        case "2":
                            EncryptExistingConfig();
                            break;
                        case "3":
                            DecryptConfig();
                            break;
                        case "4":
                            ValidateConfig();
                            break;
                        case "5":
                            UpdateConnectionString();
                            break;
                        case "6":
                            GenerateFromCode();
                            break;
                        case "7":
                            ShowCurrentConfig();
                            break;
                        case "0":
                            Console.WriteLine("退出程序...");
                            return;
                        default:
                            Console.WriteLine("无效的选择，请重试。");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"错误: {ex.Message}");
                }

                Console.WriteLine();
                Console.WriteLine("按任意键继续...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("请选择操作:");
            Console.WriteLine("1. 生成新的加密配置文件（默认设置）");
            Console.WriteLine("2. 加密现有的配置文件");
            Console.WriteLine("3. 解密配置文件（调试用）");
            Console.WriteLine("4. 验证配置文件");
            Console.WriteLine("5. 更新数据库连接字符串");
            Console.WriteLine("6. 从代码生成配置文件");
            Console.WriteLine("7. 显示当前配置");
            Console.WriteLine("0. 退出");
            Console.Write("\n请输入选择: ");
        }

        static void GenerateNewEncryptedConfig()
        {
            Console.WriteLine("\n生成新的加密配置文件...");
            ConfigurationGenerator.GenerateDefaultEncryptedConfig();
            Console.WriteLine("完成！");
        }

        static void EncryptExistingConfig()
        {
            Console.Write("\n请输入要加密的配置文件路径 (默认: appsettings.json): ");
            var sourcePath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                sourcePath = "appsettings.json";
            }

            Console.Write("请输入输出文件路径 (留空使用默认): ");
            var outputPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                outputPath = null;
            }

            ConfigurationGenerator.ConvertToEncrypted(sourcePath, outputPath);
        }

        static void DecryptConfig()
        {
            Console.Write("\n请输入要解密的配置文件路径: ");
            var encryptedPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(encryptedPath))
            {
                Console.WriteLine("路径不能为空");
                return;
            }

            Console.Write("请输入输出文件路径 (留空使用默认): ");
            var outputPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                outputPath = null;
            }

            ConfigurationGenerator.DecryptConfig(encryptedPath, outputPath);
        }

        static void ValidateConfig()
        {
            Console.Write("\n请输入要验证的配置文件路径 (默认: appsettings.json): ");
            var configPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(configPath))
            {
                configPath = "appsettings.json";
            }

            var isValid = ConfigurationGenerator.ValidateConfigFile(configPath);
            Console.WriteLine(isValid ? "配置文件有效！" : "配置文件无效！");
        }

        static void UpdateConnectionString()
        {
            Console.Write("\n请输入配置文件路径 (默认: appsettings.json): ");
            var configPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(configPath))
            {
                configPath = "appsettings.json";
            }

            if (!File.Exists(configPath))
            {
                Console.WriteLine($"文件不存在: {configPath}");
                return;
            }

            Console.WriteLine("\n请输入新的数据库连接字符串:");
            Console.WriteLine("格式: server=<服务器>;database=<数据库>;uid=<用户>;pwd=<密码>;Port=<端口>;...");
            Console.Write("连接字符串: ");
            var connectionString = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("连接字符串不能为空");
                return;
            }

            var manager = new ConfigurationManager(configPath, alwaysEncrypt: true);
            manager.UpdateConnectionString(connectionString);

            Console.WriteLine("连接字符串已更新！");
        }

        static void GenerateFromCode()
        {
            Console.WriteLine("\n从代码生成配置文件");

            Console.WriteLine("\n请输入数据库连接信息:");
            Console.Write("服务器地址 (默认: 127.0.0.1): ");
            var server = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(server)) server = "127.0.0.1";

            Console.Write("数据库名 (默认: sunwoda_demo): ");
            var database = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(database)) database = "sunwoda_demo";

            Console.Write("用户名 (默认: root): ");
            var uid = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(uid)) uid = "root";

            Console.Write("密码 (默认: root): ");
            var pwd = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(pwd)) pwd = "root";

            Console.Write("端口 (默认: 3306): ");
            var port = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(port)) port = "3306";

            var connectionString = $"server={server};database={database};uid={uid};pwd={pwd};Port={port};Persist Security Info=True;SslMode=None;AllowPublicKeyRetrieval=True;";

            Console.Write("\n启用MES功能？(Y/N, 默认: Y): ");
            var enableMesInput = Console.ReadLine();
            var enableMES = string.IsNullOrWhiteSpace(enableMesInput) ||
                           enableMesInput.Equals("Y", StringComparison.OrdinalIgnoreCase);

            Console.Write("\n输出文件路径 (默认: appsettings.json): ");
            var outputPath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(outputPath)) outputPath = "appsettings.json";

            Console.Write("\n加密配置文件？(Y/N, 默认: Y): ");
            var encryptInput = Console.ReadLine();
            var encrypt = string.IsNullOrWhiteSpace(encryptInput) ||
                         encryptInput.Equals("Y", StringComparison.OrdinalIgnoreCase);

            ConfigurationGenerator.GenerateConfigFromCode(
                connectionString,
                enableMES,
                outputPath,
                encrypt);
        }

        static void ShowCurrentConfig()
        {
            Console.Write("\n请输入配置文件路径 (默认: appsettings.json): ");
            var configPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(configPath))
            {
                configPath = "appsettings.json";
            }

            if (!File.Exists(configPath))
            {
                Console.WriteLine($"文件不存在: {configPath}");
                return;
            }

            var manager = new ConfigurationManager(configPath);
            var settings = manager.GetCurrentSettings();

            if (settings == null)
            {
                Console.WriteLine("无法读取配置");
                return;
            }

            var isEncrypted = JsonHelper.IsFileEncrypted(configPath);

            Console.WriteLine("\n========== 当前配置 ==========");
            Console.WriteLine($"文件: {configPath}");
            Console.WriteLine($"加密状态: {(isEncrypted ? "已加密" : "未加密")}");
            Console.WriteLine($"\n连接字符串:");
            Console.WriteLine($"  {settings.ConnectionStrings.DefaultConnection}");
            Console.WriteLine($"\n项目设置:");
            Console.WriteLine($"  MES功能: {(settings.ProjectSettings.EnableMES ? "启用" : "禁用")}");
            Console.WriteLine("==============================");
        }
    }

    /// <summary>
    /// 用于在应用程序启动时的配置初始化示例
    /// </summary>
    public class StartupConfigExample
    {
        public static void EnsureConfigExists()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            // 如果配置文件不存在，生成默认的加密配置
            if (!File.Exists(configPath))
            {
                Console.WriteLine("配置文件不存在，正在生成默认配置...");

                // 生成默认的加密配置
                var appSettings = JsonHelper.GenerateDefaultAppSettings();
                JsonHelper.WriteJsonFile(configPath, appSettings, encrypt: true);

                Console.WriteLine("默认配置已生成（加密）");
            }
            else
            {
                // 验证配置文件
                if (ConfigurationGenerator.ValidateConfigFile(configPath))
                {
                    Console.WriteLine("配置文件验证通过");
                }
                else
                {
                    throw new InvalidOperationException("配置文件无效");
                }
            }
        }
    }
}