using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace FMS_Collection.Core.Common
{
    public static class AppSettings
    {
        public static string DefaultDbConnectionString { get; }
        public static string LoggingConnectionString { get; }
        public static string CachingMode { get; }
        public static string AssetDirectory { get; }
        public static string PhysicalPathDirectory { get; }

        public static string CurrencyConversionApiBaseUrl { get; set; }
        public static string CurrencyConversionApiPath { get; set; }
        public static string CurrencyConversionApiAccessKey { get; set; }

        static AppSettings()
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();

            var config = builder.Build();

            DefaultDbConnectionString = config["ConnectionStrings:FMSConnectionString"];
            //LoggingConnectionString = config["ConnectionStrings:LoggingConnectionString"];
            CachingMode = config["ConnectionStrings:CachingMode"];
            AssetDirectory = config["ConnectionStrings:AssetDirectory"];
            PhysicalPathDirectory = config["AppSettings:PhysicalPathDirectory"];
            CurrencyConversionApiBaseUrl = config["ConnectionStrings:CurrencyConversionApiBaseUrl"];
            CurrencyConversionApiPath = config["ConnectionStrings:CurrencyConversionApiPath"];
            CurrencyConversionApiAccessKey = config["ConnectionStrings:CurrencyConversionApiAccessKey"];
        }

    }
}
