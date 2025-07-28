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
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }

        public static string GetConnectionString(string name = "DefaultConnection")
        {
            return Configuration.GetConnectionString(name) ?? string.Empty;
        }
    }
}
