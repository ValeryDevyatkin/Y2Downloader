using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Y2Downloader.Common.Interfaces;

namespace Y2Downloader
{
    internal static class ConfigHelper
    {
        public static int GetInt(this IConfigurationRoot root, string sectionName)
        {
            if (int.TryParse(root.GetSection(sectionName).Value, out var val))
            {
                return val;
            }

            throw new ConfigurationException(sectionName);
        }

        public static string GetString(this IConfigurationRoot root, string sectionName) =>
            root.GetSection(sectionName).Value;
    }

    internal class AppSettings : IAppSettings
    {
        private const string AppSettingsFileName = "AppSettings.json";

        public int SourceLocationProcessDelay { get; private set; }

        public void Init()
        {
            ReadSettingsFromConfigFile();
        }

        private void ReadSettingsFromConfigFile()
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile(AppSettingsFileName, false);

            var config = builder.Build();

            SourceLocationProcessDelay = config.GetInt(nameof(SourceLocationProcessDelay));
        }
    }
}