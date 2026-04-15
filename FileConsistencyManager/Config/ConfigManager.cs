using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Linq.Expressions;
using FileConsistencyManager.Logging;
using FileConsistencyManager.Models;
using FileConsistencyManager.Localization;
using FileConsistencyManager.Security;
using Microsoft.Data.SqlClient;

namespace FileConsistencyManager.Config
{
    public class ConfigManager
    {
        private readonly string _path;
        private Localize _localization;
        private readonly Logger _logger;

        public ConfigManager(string path, Localize localization, Logger logger)
        {
            this._path = path;
            this._localization = localization;
            this._logger = logger;
        }

        public AppConfig Load(bool showDialogs = false)
        {
            AppConfig config = null;
            var fullPath = Path.Combine(AppContext.BaseDirectory, _path);

            try
            {
                if (!File.Exists(fullPath))
                {
                    _logger.Log($"Configuration file not found: {fullPath}. Using defaults.", LogLevel.Warning);
                    if (showDialogs)
                    {
                        string message = string.Format(_localization.GetContent("ConfigNotFoundErrorMessage", _localization.GetCurrentLanguage()), fullPath);
                        MessageBox.Show(
                            message,
                            _localization.GetContent("CustomTextError", _localization.GetCurrentLanguage()),
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    return config;
                }

                var json = File.ReadAllText(fullPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var deserialized = JsonSerializer.Deserialize<AppConfig>(json, options);

                if (deserialized == null)
                {
                    _logger.Log($"Failed to deserialize configuration: {fullPath}. Using defaults.", LogLevel.Warning);
                    if (showDialogs)
                    {
                        string message = string.Format(_localization.GetContent("ConfigLoadDeserializeMessage", _localization.GetCurrentLanguage()), fullPath);
                        MessageBox.Show(
                            message,
                            _localization.GetContent("CustomTextError", _localization.GetCurrentLanguage()),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return config;
                }

                // Fill missing sub-objects with safe defaults to avoid null refs elsewhere
                var defaultConfig = new AppConfig
                {
                    Connection = new DatabaseConfig { Server = string.Empty, Database = string.Empty, UserId = string.Empty, Password = string.Empty },
                    Path = new PathConfig { ArchivePath = string.Empty },
                    Language = new LanguageConfig { Current = "en" }
                };

                if (deserialized.Connection == null)
                    deserialized.Connection = defaultConfig.Connection;
                if (deserialized.Path == null)
                    deserialized.Path = defaultConfig.Path;
                if (deserialized.Language == null)
                    deserialized.Language = defaultConfig.Language;

                // Try to decrypt password stored in configuration. If decryption fails, assume it's plain text and keep it.
                if (!string.IsNullOrEmpty(deserialized.Connection.Password))
                {
                    var decrypted = DataProtector.Unprotect(deserialized.Connection.Password);
                    if (!string.IsNullOrEmpty(decrypted))
                    {
                        deserialized.Connection.Password = decrypted;
                    }
                    // else leave as-is (likely plain text or invalid protected string)
                }

                return deserialized;
            }
            catch (JsonException jex)
            {
                _logger.LogException(jex);
                _logger.Log($"Error parsing configuration file '{_path}'. Using defaults.", LogLevel.Warning);
                if (showDialogs)
                {
                    string message = string.Format(_localization.GetContent("ConfigParseErrorMessage", _localization.GetCurrentLanguage()), fullPath);
                    MessageBox.Show(
                        message,
                        _localization.GetContent("CustomTextError", _localization.GetCurrentLanguage()),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return config;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                _logger.Log($"Unexpected error loading configuration '{_path}'. Using defaults.", LogLevel.Error);
                if (showDialogs)
                {
                    string message = string.Format(_localization.GetContent("ConfigUnexpectedErrorMessage", _localization.GetCurrentLanguage()), fullPath);
                    MessageBox.Show(
                        message,
                        _localization.GetContent("CustomTextError", _localization.GetCurrentLanguage()),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return config;
            }
        }

        #region Get Methods

        public string GetConnectionString(AppConfig config)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Server=").Append($@"{config.Connection.Server}")
                .Append(";Database=").Append(config.Connection.Database)
                .Append(";User Id=").Append(config.Connection.UserId)
                .Append(";Password=").Append(config.Connection.Password)
                .Append(";TrustServerCertificate=True;");
            return sb.ToString();
        }

        public string GetDatabaseName(AppConfig config)
        {
            return config.Connection.Server + "\\" + config.Connection.Database;
        }

        #endregion

        #region Set Methods

        public void SetNewConfig(AppConfig config, Logger logger, bool showDialogs = true)
        {
            // write into the config file with the new config
            try
            {
                // ensure sub-objects
                if (config.Connection == null) config.Connection = new DatabaseConfig();
                if (config.Path == null) config.Path = new PathConfig();
                if (config.Language == null) config.Language = new LanguageConfig { Current = "en" };

                // Encrypt password before writing (do not mutate original config)
                var toWrite = new AppConfig
                {
                    Connection = new DatabaseConfig
                    {
                        Server = config.Connection.Server,
                        Database = config.Connection.Database,
                        UserId = config.Connection.UserId,
                        Password = string.IsNullOrEmpty(config.Connection.Password) ? string.Empty : DataProtector.Protect(config.Connection.Password)
                    },
                    Path = new PathConfig { ArchivePath = config.Path.ArchivePath },
                    Language = new LanguageConfig { Current = config.Language.Current }
                };

                var fullPath = Path.Combine(AppContext.BaseDirectory, _path);
                var dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(toWrite, options);
                File.WriteAllText(fullPath, json);

                if (showDialogs)
                {
                    string message = string.Format(_localization.GetContent("ConfigSuccessfulSaveMessage", _localization.GetCurrentLanguage()), fullPath);
                    MessageBox.Show(
                        message,
                        _localization.GetContent("CustomTextInformation", _localization.GetCurrentLanguage()),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Error writing configuration to file '{_path}': {ex.Message}", LogLevel.Error);
                if (showDialogs)
                {
                    string message = string.Format(_localization.GetContent("ConfigSaveErrorMessage", _localization.GetCurrentLanguage()), _path);
                    MessageBox.Show(
                        message,
                        _localization.GetContent("CustomTextError", _localization.GetCurrentLanguage()),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
        }

        public string SetConnectionString(string server, string database, string userId, string password)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Server=").Append(server)
                .Append(";Database=").Append(database)
                .Append(";User Id=").Append(userId)
                .Append(";Password=").Append(password)
                .Append(";TrustServerCertificate=True;");
            return sb.ToString();
        }

        #endregion

        #region Tests

        public async Task<bool> TestConnection(AppConfig config, string connection)
        {
            try
            {
                using var conn = new SqlConnection(connection);
                await conn.OpenAsync();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return false;
            }
            return true;
        }

        #endregion
    }
}
