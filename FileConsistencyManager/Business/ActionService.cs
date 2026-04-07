using System;
using System.Text;
using System.Collections.Generic;
using FileConsistencyManager.Models;
using FileConsistencyManager.Logging;

namespace FileConsistencyManager.Business
{
    internal class ActionService
    {
        private readonly Logger _logger;

        public ActionService(Logger logger)
        {
            _logger = logger;
        }

        public void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    _logger.Log($"File deleted: {path}", LogLevel.Info);
                }
                else
                {
                    _logger.Log($"File was not found: {path}", LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public void IgnoreEntry(ComparisonResult result)
        {
            _logger.Log($"Entry ignored: {result.FileName} ({result.Types})", LogLevel.Info);
        }

        public void ArchiveFile(string path, string archivePath)
        {
            // Archivierungslogik hier implementieren
            _logger.Log($"File archived: {archivePath}", LogLevel.Info);
        }
    }
}
