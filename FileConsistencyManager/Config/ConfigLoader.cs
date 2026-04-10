using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Linq.Expressions;
using FileConsistencyManager.Logging;
using FileConsistencyManager.Models;
using FileConsistencyManager.Localization;

namespace FileConsistencyManager.Config
{
    public class ConfigLoader
    {
        private readonly string _path;
        private Localize _localization;

        public ConfigLoader(string path, Localize localization)
        {
            this._path = path;
            this._localization = localization;
        }

        public AppConfig Load(Logger logger)
        {
            AppConfig config = CreateDefaultConfig();

            try
            {
                if (!File.Exists(_path))
                {
                    logger.Log($"Configuration file not found: {_path}. Using defaults.", LogLevel.Warning);
                    MessageBox.Show($"Configuration file '{_path}' not found. Please create the file with the necessary settings. Using defaults.", "Configuration Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return config;
                }

                var json = File.ReadAllText(_path);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var deserialized = JsonSerializer.Deserialize<AppConfig>(json, options);

                if (deserialized == null)
                {
                    logger.Log($"Failed to deserialize configuration: {_path}. Using defaults.", LogLevel.Warning);
                    // Insert Method to open configuration Forms
                    logger.Log("Configuration loading failed.", LogLevel.Error);
                    CustomMessageBox.Show($"Failed to deserialize configuration file '{_path}'. Please check the file format. Using defaults",
                        "Error",
                        CustomMessageBoxTypes.CustomMessageBoxButtons.OK,
                        CustomMessageBoxTypes.CustomMessageBoxIcon.Error,
                        _localization);
                    //MessageBox.Show($"Failed to deserialize configuration file '{_path}'. Please check the file format. Using defaults.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return config;
                }

                // Fill missing sub-objects with safe defaults to avoid null refs elsewhere
                if (deserialized.Connection == null)
                    deserialized.Connection = config.Connection;
                if (deserialized.Path == null)
                    deserialized.Path = config.Path;

                return deserialized;
            }
            catch (JsonException jex)
            {
                logger.LogException(jex);
                logger.Log($"Error parsing configuration file '{_path}'. Using defaults.", LogLevel.Warning);
                CustomMessageBox.Show($"Error parsing configuration file '{_path}'. Please check the file format. Using defaults.",
                        "Error",
                        CustomMessageBoxTypes.CustomMessageBoxButtons.OK,
                        CustomMessageBoxTypes.CustomMessageBoxIcon.Error,
                        _localization);
                MessageBox.Show($"Error parsing configuration file '{_path}'. Please check the file format. Using defaults.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);  //check
                return config;
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                logger.Log($"Unexpected error loading configuration '{_path}'. Using defaults.", LogLevel.Error);
                CustomMessageBox.Show($"Unexpected error loading configuration '{_path}'. Please check the file and try again. Using defaults.",
                        "Error",
                        CustomMessageBoxTypes.CustomMessageBoxButtons.OK,
                        CustomMessageBoxTypes.CustomMessageBoxIcon.Error,
                        _localization);
                //MessageBox.Show($"Unexpected error loading configuration '{_path}'. Please check the file and try again. Using defaults.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);  //check
                return config;
            }
        }

        private static AppConfig CreateDefaultConfig()
        {
            return new AppConfig
            {
                Connection = new DatabaseConfig
                {
                    Server = string.Empty,
                    Database = string.Empty,
                    UserId = string.Empty,
                    Password = string.Empty
                },
                Path = new PathConfig
                {
                    ArchivePath = string.Empty
                }
            };
        }

        public void SetNewConfig(AppConfig config, Logger logger)
        {
            // write into the config file with the new config
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(_path, json);
                MessageBox.Show($"Configuration saved successfully to '{_path}'.", "Configuration Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Log($"Error writing configuration to file '{_path}': {ex.Message}", LogLevel.Error);
                MessageBox.Show($"Error writing configuration to file '{_path}'. Please check the file permissions and try again.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
        }
    }
}
