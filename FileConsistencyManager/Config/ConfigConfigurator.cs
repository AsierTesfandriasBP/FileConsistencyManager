using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FileConsistencyManager.Config
{
    internal class ConfigConfigurator
    {
        public void Set(string path = "config.json", AppConfig config = null)
        {
            var defaultConfig = config ?? new AppConfig
            {
                // Forms for a default configuration
                Connection = new DatabaseConfig
                {
                    Server = "localhost",
                    Database = "user",
                    UserId = "file_consistency_db",
                    Password = "password"
                },
                Path = new PathConfig
                {
                    ArchivePath = "C:\\Source",
                },
                Logging = new LoggingConfig
                {
                    LogFilePath = "_log.txt"
                }
            };
            var json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}
