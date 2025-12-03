using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.ComponentModel;
using System.Net.Mail;

namespace FMS_Collection.Core.Common
{
  public static class AppSettings
  {
    public static string DefaultDbConnectionString { get; }
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

    static AppSettings()
    {
      var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables();

      var config = builder.Build();

      DefaultDbConnectionString = config["ConnectionStrings:FMSConnectionString"] ?? "";
      AzureStorageConnectionString = config["AzureStorage:ConnectionString"] ?? "";
      AzureStorageContainerName = config["AzureStorage:ContainerName"] ?? "";

      AllowedFailedLoginCount = Convert.ToInt32(config["AppSettings:AllowedFailedLoginCount"]);
      CachingMode = config["AppSettings:CachingMode"] ?? "";
      AssetDirectory = config["AppSettings:AssetDirectory"] ?? "";
      CurrencyConversionApiBaseUrl = config["AppSettings:CurrencyConversionApiBaseUrl"] ?? "";
      CurrencyConversionApiPath = config["AppSettings:CurrencyConversionApiPath"] ?? "";
      CurrencyConversionApiAccessKey = config["AppSettings:CurrencyConversionApiAccessKey"] ?? "";
      SmtpHost = config["MailConfig:SmtpHost"] ?? "";
      SmtpPort = Convert.ToInt32(config["MailConfig:SmtpPort"]);
      SenderEmail = config["MailConfig:SenderEmail"] ?? "";
      EmailPassword = Environment.GetEnvironmentVariable("MailConfig:EmailPassword");

      //EmailPassword = config["MailConfig:EmailPassword"] ?? "";
    }

  }
}
