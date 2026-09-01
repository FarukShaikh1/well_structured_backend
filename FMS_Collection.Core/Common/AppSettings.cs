using Microsoft.Extensions.Configuration;

namespace FMS_Collection.Core.Common
{
    public static class AppSettings
    {
        public static string DefaultDbConnectionString { get; }
        public static string AesMasterSecret { get; }
        public static string AzureStorageConnectionString { get; }
        public static string AzureStorageContainerName { get; }

        public static int AllowedFailedLoginCount { get; }
        public static string CachingMode { get; }
        public static string AssetDirectory { get; }

        public static string CurrencyConversionApiBaseUrl { get; set; }
        public static string CurrencyConversionApiPath { get; set; }
        public static string CurrencyConversionApiAccessKey { get; set; }

        public static string SmtpHost { get; set; }
        public static int SmtpPort { get; set; }
        public static string SenderEmail { get; set; }
        public static string EmailPassword { get; set; }
        public static string AzureVision_Endpoint { get; set; }
        public static string AzureVision_Key { get; set; }
        public static string SiteLiveUrl { get; set; }
        public static string OwnerEmail { get; set; }

        static AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

#if DEBUG
            builder.AddJsonFile(
                "appsettings.Development.json",
                optional: true,
                reloadOnChange: true);
#endif

            builder.AddEnvironmentVariables();

            var config = builder.Build();
            DefaultDbConnectionString = config["ConnectionStrings:FMSConnectionString"] ?? "";
            AesMasterSecret = config["AppSettings:AesMasterSecret"] ?? "";
            AllowedFailedLoginCount = Convert.ToInt32(config["AppSettings:AllowedFailedLoginCount"]);
            SiteLiveUrl = config["AppSettings:SiteLiveUrl"] ?? "";
            OwnerEmail = config["AppSettings:OwnerEmail"] ?? "";
            CachingMode = config["AppSettings:CachingMode"] ?? "";
            AssetDirectory = config["AppSettings:AssetDirectory"] ?? "";
            CurrencyConversionApiBaseUrl = config["AppSettings:CurrencyConversionApiBaseUrl"] ?? "";
            CurrencyConversionApiPath = config["AppSettings:CurrencyConversionApiPath"] ?? "";
            CurrencyConversionApiAccessKey = config["AppSettings:CurrencyConversionApiAccessKey"] ?? "";
            SmtpHost = config["MailConfig:SmtpHost"] ?? "";
            SmtpPort = Convert.ToInt32(config["MailConfig:SmtpPort"]);
            SenderEmail = Environment.GetEnvironmentVariable("MailConfigSenderEmail") ?? "";
            EmailPassword = Environment.GetEnvironmentVariable("MailConfigEmailPassword") ?? "";
#if DEBUG
            SenderEmail = "farukshaikh908@gmail.com";
            EmailPassword = "wqdc guum pjgp yckh";
#endif
            AzureVision_Endpoint = Environment.GetEnvironmentVariable("AzureVisionEndpoint") ?? "";
            AzureVision_Key = Environment.GetEnvironmentVariable("AzureVisionKey") ?? "";
            AzureStorageConnectionString = Environment.GetEnvironmentVariable("AzureStorageConnectionString") ?? "";
            AzureStorageContainerName = Environment.GetEnvironmentVariable("AzureStorageContainerName") ?? "";

            //EmailPassword = config["MailConfig:EmailPassword"] ?? "";
        }

    }
}
