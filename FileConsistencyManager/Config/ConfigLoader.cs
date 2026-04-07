using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FileConsistencyManager.Config
{
    public static class ConfigLoader
    {
        public static AppConfig Load(string path = "config.json")
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppConfig>(json);
        }
    }
}
