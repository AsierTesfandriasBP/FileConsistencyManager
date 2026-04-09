using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;

namespace FileConsistencyManager.Config
{
    public static class ConfigLoader
    {
        public static AppConfig Load(string path = "config.json")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Configuration file not found: {path}");

            var json = File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var config = JsonSerializer.Deserialize<AppConfig>(json, options);

            if (config == null)
                throw new InvalidOperationException("Failed to deserialize configuration.");

            return config;
        }
    }
}
