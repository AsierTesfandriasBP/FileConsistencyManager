using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Models
{
    internal class FileEntry
    {
        // Represents a file entry with its full path and file name.
        public string FullPath { get; set; }
        public string FileName { get; set; }
    }
}
