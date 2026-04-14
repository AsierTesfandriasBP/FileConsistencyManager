using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Config
{
    public class AppConfig
    {
        public DatabaseConfig Connection { get; set; }
        public PathConfig Path { get; set; }
        public LanguageConfig Language { get; set; }
    }

    public class DatabaseConfig
    {
        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? UserId { get; set; }
        // Password is stored encrypted (base64 protected). Use DataProtector to encrypt/decrypt.
        public string? Password { get; set; }
    }
    public class PathConfig
    {
        public string? ArchivePath { get; set; }
    }

    public class LanguageConfig
    {
        public string? Current { get; set; }
    }
}
