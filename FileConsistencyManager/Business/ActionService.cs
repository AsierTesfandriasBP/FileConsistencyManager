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

        public void DeleteFile(List<ComparisonResult> OrphanFiles)
        {
            try
            {
                foreach (var item in OrphanFiles)
                {
                    if (File.Exists(item.Source))
                    {
                        File.Delete(item.Source);
                        _logger.Log($"Orphan file deleted: {item.Source}", LogLevel.Info);
                    }
                    else
                    {
                        _logger.Log($"Orphan file was not found: {item.Source}", LogLevel.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public void DeleteEntry(List<string> missingFiles, string _connectionString)
        {
            try
            {
                //string test = @"DELETE FROM ATTACHMENT WHERE ATTACHID IN ('eDEMOA000001', 'eDEMOA000002');";
                string idList = GetMissingFilesAsStringListForQuery(missingFiles);
                string query = $@"DELETE FROM ATTACHMENT WHERE ATTACHID IN ({idList});";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                foreach (var item in missingFiles)
                {
                    _logger.Log($"Database entry successfully deleted: {item}", LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        #endregion

        #region Ignore

        public void IgnoreEntry(List<ComparisonResult> result)
        {
            foreach (var item in result)
            {
                _logger.Log($"Entry ignored: {item.FileName} ({item.Type})", LogLevel.Info);
            }
        }

        #endregion

        #region Archive

        public void ArchiveFile(List<ComparisonResult> sourcePath, string archivePath)
        {
            try
            {
                foreach(var item in sourcePath)
                {
                    if (item.Type != IssueType.Types.OrphanFile)
                        return;

                    if (!Directory.Exists(archivePath))
                        Directory.CreateDirectory(archivePath);

                    string targetPath = Path.Combine(archivePath, Path.GetFileName(item.Source));

                    File.Move(item.Source, targetPath);

                    _logger.Log($"File archived: {item.Source} to {targetPath}", LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        #endregion

        #region Helper Methods

        public string GetMissingFilesAsStringListForQuery(List<string> missingFiles)
        {
            StringBuilder sb = new StringBuilder();

            // Builds a string in the format: 'Id_1', 'Id_2', 'Id_3' for the SQL Query
            foreach (var file in missingFiles)
            {
                sb.Append($"'{file}', ");
            }

            // Removes last comma and space
            sb.Replace(", ", "", sb.Length - 2, 2);

            return sb.ToString();
        }

        #endregion
    }
}
