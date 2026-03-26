using System;
using System.Collections.Generic;
using System.Text;
using FileConsistencyManager.Models;

namespace FileConsistencyManager.Services
{
    internal class FileService
    {
        // Retrieves all files from the specified root path and its subdirectories.
        public List<FileEntry> GetAllFiles(string rootPath)
        {
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);

            return files.Select(f => new FileEntry
            {
                FullPath = f,
                FileName = Path.GetFileName(f)
            }).ToList();
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}
