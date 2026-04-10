using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Config
{
    public class AppConfig
    {
        public DatabaseConfig Connection { get; set; }
        public PathConfig Path { get; set; }
    }

    public class DatabaseConfig
    {
        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
    }
    public class PathConfig
    {
        public string? ArchivePath { get; set; }
    }
}
