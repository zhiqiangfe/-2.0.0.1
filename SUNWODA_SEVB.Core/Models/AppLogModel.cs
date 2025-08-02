
namespace SUNWODA_SEVB.Core.Models
{
    /// <summary>
    /// 应用程序日志表
    /// </summary>
    public class AppLogModel
    {
      
        public int ID { get; set; }

        public DateTime LogTime { get; set; }

        public string LogLevel { get; set; } = null!;

        public string Logger { get; set; } = null!;

        public string Message { get; set; } = null!;

        public string? Exception { get; set; }

        public AppLogModel() { }
        public AppLogModel(string logLevel, string logger, string message)
        {
            LogTime = DateTime.Now;
            LogLevel = logLevel;
            Logger = logger;
            Message = message;
        }
    }
}
