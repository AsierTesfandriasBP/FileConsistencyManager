using FileConsistencyManager.Logging;
using FileConsistencyManager.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
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

        public void DeleteEntry(List<string> missingFiles, string connectionString)
        {
            if (missingFiles == null || missingFiles.Count == 0)
            {
                _logger.Log("No database entries selected for deletion.", LogLevel.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        var parameterNames = new List<string>();

                        for (int i = 0; i < missingFiles.Count; i++)
                        {
                            string parameterName = $"@id{i}";
                            parameterNames.Add(parameterName);

                            command.Parameters.Add(parameterName, SqlDbType.NVarChar, 50).Value = missingFiles[i];
                        }

                        command.CommandText = $@"DELETE FROM ATTACHMENT WHERE ATTACHID IN ({string.Join(", ", parameterNames)});";

                        int affectedRows = command.ExecuteNonQuery();

                        transaction.Commit();

                        _logger.Log(
                            $"Database delete successful. Deleted rows: {affectedRows}",
                            LogLevel.Info);

                        foreach (var item in missingFiles)
                        {
                            _logger.Log(
                                $"Database entry successfully deleted: {item}",
                                LogLevel.Info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        #endregion

        #region Archive

        public void ArchiveFile(List<ComparisonResult> sourcePath, string archivePath)
        {
            try
            {
                if (sourcePath == null || sourcePath.Count == 0)
                    return;

                // Filter to orphan files and existing files
                var filesToArchive = new List<ComparisonResult>();
                foreach (var item in sourcePath)
                {
                    if (item == null) continue;
                    if (item.Type != IssueType.Types.OrphanFile) continue;
                    if (!File.Exists(item.Source))
                    {
                        _logger.Log($"Orphan file was not found for archiving: {item.Source}", LogLevel.Warning);
                        continue;
                    }

                    filesToArchive.Add(item);
                }

                if (filesToArchive.Count == 0)
                    return;

                // Group files by creation date (date only)
                var groups = new Dictionary<DateTime, List<ComparisonResult>>();
                foreach (var item in filesToArchive)
                {
                    var creationDate = File.GetCreationTime(item.Source).Date;
                    if (!groups.ContainsKey(creationDate)) groups[creationDate] = new List<ComparisonResult>();
                    groups[creationDate].Add(item);
                }

                // Ensure base archive path exists
                if (!Directory.Exists(archivePath))
                    Directory.CreateDirectory(archivePath);

                // For each date group create a folder and zip the files
                foreach (var kvp in groups)
                {
                    var date = kvp.Key;
                    var items = kvp.Value;

                    var dateFolder = Path.Combine(archivePath, date.ToString("yyyy-MM-dd"));
                    if (!Directory.Exists(dateFolder)) Directory.CreateDirectory(dateFolder);

                    // Create unique zip filename
                    string zipBase = $"archive_{date:yyyyMMdd}.zip";
                    string zipPath = Path.Combine(dateFolder, zipBase);
                    int suffix = 1;
                    while (File.Exists(zipPath))
                    {
                        zipPath = Path.Combine(dateFolder, Path.GetFileNameWithoutExtension(zipBase) + $"_{suffix}" + Path.GetExtension(zipBase));
                        suffix++;
                    }

                    using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                    {
                        var manifestSb = new StringBuilder();
                        manifestSb.AppendLine($"Archive created: {DateTime.Now:O}");
                        manifestSb.AppendLine($"Group creation date: {date:O}");
                        manifestSb.AppendLine("Files:");

                        var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                        foreach (var item in items)
                        {
                            try
                            {
                                string entryName = Path.GetFileName(item.Source);
                                // avoid duplicate entry names inside zip
                                if (usedNames.Contains(entryName))
                                {
                                    string nameOnly = Path.GetFileNameWithoutExtension(entryName);
                                    string ext = Path.GetExtension(entryName);
                                    int i = 1;
                                    string newName;
                                    do
                                    {
                                        newName = $"{nameOnly}_{i}{ext}";
                                        i++;
                                    } while (usedNames.Contains(newName));
                                    entryName = newName;
                                }

                                zip.CreateEntryFromFile(item.Source, entryName, CompressionLevel.Optimal);
                                usedNames.Add(entryName);

                                manifestSb.AppendLine(item.Source);

                                // delete original after successful add
                                try
                                {
                                    File.Delete(item.Source);
                                    _logger.Log($"File archived and removed: {item.Source} -> {zipPath}", LogLevel.Info);
                                }
                                catch (Exception exDel)
                                {
                                    _logger.Log($"Archived but failed to delete original file: {item.Source}. Error: {exDel.Message}", LogLevel.Warning);
                                }
                            }
                            catch (Exception exFile)
                            {
                                _logger.Log($"Failed to add file to archive: {item.Source}. Error: {exFile.Message}", LogLevel.Error);
                            }
                        }

                        // add manifest file to zip
                        var manifestEntry = zip.CreateEntry("manifest.txt");
                        using (var writer = new StreamWriter(manifestEntry.Open(), Encoding.UTF8))
                        {
                            writer.Write(manifestSb.ToString());
                        }
                    }

                    _logger.Log($"Created archive: {zipPath} (files: {items.Count})", LogLevel.Info);
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
