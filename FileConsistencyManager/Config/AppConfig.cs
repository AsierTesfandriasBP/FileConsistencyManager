using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Config
{
    public class AppConfig
    {
        public DatabaseConfig Database { get; set; }
        public FileSystemConfig FileSystem { get; set; }
        public LoggingConfig Logging { get; set; }
    }

    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
    }

    public class FileSystemConfig
    {
        public string RootPath { get; set; }
    }

    public class LoggingConfig
    {
        public string LogFilePath { get; set; }
        public string LogLevel { get; set; }
    }
}
