using FileConsistencyManager.Logging;
using FileConsistencyManager.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace FileConsistencyManager.Business
{
    internal class ActionService
    {
        private readonly Logger _logger;

        public ActionService(Logger logger)
        {
            _logger = logger;
        }

        #region Delete

        public void DeleteFile(string path, List<ComparisonResult> OrphanFiles)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    _logger.Log($"Orphan file deleted: {path}", LogLevel.Info);
                }
                else
                {
                    _logger.Log($"Orphan file was not found: {path}", LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public void DeleteEntry(ComparisonResult result, string _connectionString, List<ComparisonResult> missingFiles)
        {
            try
            {
                string test = @"DELETE FROM ATTACHMENT WHERE ATTACHID IN ('eDEMOA000001', 'eDEMOA000002');";

                string test2 = GetMissingFilesAsStringListForQuery(missingFiles);

                string test3 = $@"DELETE FROM ATTACHMENT WHERE ATTACHID IN ({test2});";

                /*
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"DELETE FROM ATTACHMENT WHERE ATTACHID IN ('eDEMOA000001', 'eDEMOA000002');";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                */

                //_logger.Log($"Database entry successfully deleted: {result.FileName} ({result.Types})", LogLevel.Info);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        #endregion

        #region Ignore

        public void IgnoreEntry(ComparisonResult result)
        {
            _logger.Log($"Entry ignored: {result.FileName} ({result.Type})", LogLevel.Info);
        }

        #endregion

        #region Archive

        public void ArchiveFile(ComparisonResult sourcePath, string archivePath)
        {
            try
            {
                if (sourcePath.Type != IssueType.Types.OrphanFile)
                    return;

                if (!Directory.Exists(archivePath))
                    Directory.CreateDirectory(archivePath);

                string targetPath = Path.Combine(archivePath, Path.GetFileName(sourcePath.Source));

                File.Move(sourcePath.Source, targetPath);

                _logger.Log($"File archived: {sourcePath.Source} to {targetPath}", LogLevel.Info);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        #endregion

        #region Helper Methods

        public string GetMissingFilesAsStringListForQuery(List<ComparisonResult> missingFiles)
        {
            StringBuilder sb = new StringBuilder();

            // Builds a string in the format: 'Id_1', 'Id_2', 'Id_3' for the SQL Query
            foreach (var file in missingFiles)
            {
                sb.Append($"'{file.FileName}', ");
            }

            // Removes last comma and space
            sb.Replace(", ", "", sb.Length - 2, 2);

            return sb.ToString();
        }

        #endregion
    }
}
