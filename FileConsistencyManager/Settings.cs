using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FileConsistencyManager.Config;
using FileConsistencyManager.Localization;
using FileConsistencyManager.Logging;
using Microsoft.Data.SqlClient;

namespace FileConsistencyManager
{
    public partial class Settings : Form
    {
        private AppConfig _config;
        private ConfigLoader _configLoader;
        private Localize _localization;
        private Logger _logger;
        private string _logFilePath;
        private bool _isMainOpen;

        public Settings(AppConfig confg, Logger logger, ConfigLoader configLoader, Localize localization, bool isMainOpen = false)
        {
            _config = confg;
            _localization = localization;
            _logger = logger;
            _logFilePath = _logger.GetLogFilePath();
            _configLoader = configLoader;
            _isMainOpen = isMainOpen;
            InitializeComponent();
            GetConfig();
        }

        public void GetConfig()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string logFolder = Path.Combine(basePath, "_Archive");
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);

                if (_config == null)
                {
                    tbArchivePath.Text = logFolder;
                    cmbLanguage.SelectedItem = "en";
                    return;
                }

                if (_isMainOpen)
                {
                    MessageBox.Show("Configuration loaded from main application instance. You can adjust settings and save, but changes will only take effect after restarting the application.", "Configuration Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                tbServer.Text = _config.Connection?.Server ?? string.Empty;
                tbDatabase.Text = _config.Connection?.Database ?? string.Empty;
                tbUserId.Text = _config.Connection?.UserId ?? string.Empty;
                tbPassword.Text = _config.Connection?.Password ?? string.Empty;
                if (_config.Path == null || string.IsNullOrWhiteSpace(_config.Path.ArchivePath))
                    tbArchivePath.Text = logFolder;
                else
                    tbArchivePath.Text = _config.Path.ArchivePath;

                // select language in combobox if available
                var langValue = _config.Language?.Current ?? string.Empty;
                if (!string.IsNullOrEmpty(langValue) && cmbLanguage.Items.Contains(langValue))
                    cmbLanguage.SelectedItem = langValue;
                else if (cmbLanguage.Items.Count > 0)
                    cmbLanguage.SelectedItem = "en";
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Show($"Fehler beim Laden der Konfiguration: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);    // check
            }
        }

        public bool isEverythingFilled()
        {
            // ArchivePath may be empty (populated later from DB). Validate only required connection fields and language.
            if (!IsDatabaseCredentialsFilled() ||
                string.IsNullOrWhiteSpace(cmbLanguage.Text))
            {
                MessageBox.Show("Please fill in all required fields: Server, Database, UserId, Password and Language.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool IsDatabaseCredentialsFilled()
        {
            return !(string.IsNullOrWhiteSpace(tbServer.Text)
                || string.IsNullOrWhiteSpace(tbDatabase.Text)
                || string.IsNullOrWhiteSpace(tbUserId.Text)
                || string.IsNullOrWhiteSpace(tbPassword.Text));
        }

        private string BuildConnectionString()
        {
            var sb = new StringBuilder();
            sb.Append("Server=").Append(tbServer.Text)
              .Append(";Database=").Append(tbDatabase.Text)
              .Append(";User Id=").Append(tbUserId.Text)
              .Append(";Password=").Append(tbPassword.Text)
              .Append(";TrustServerCertificate=True;");
            return sb.ToString();
        }

        private async void TestConnection(object? sender, EventArgs e)
        {
            try
            {
                if (!IsDatabaseCredentialsFilled())
                {
                    MessageBox.Show("Please enter Server, Database, UserId and Password before testing the connection.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnSave.Enabled = false;
                btnConnectionTest.Enabled = false;

                string connStr = BuildConnectionString();

                using var conn = new SqlConnection(connStr);
                await conn.OpenAsync();

                MessageBox.Show("Connection successful.", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _logger?.Log("Database connection test succeeded.", LogLevel.Info);

                btnSave.Enabled = true;
                btnConnectionTest.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSave.Enabled = true;
                btnConnectionTest.Enabled = true;
                _logger?.LogException(ex);
                MessageBox.Show($"Connection failed: {ex.Message}", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveConfig()
        {
            var newConfig = new AppConfig
            {
                Connection = new DatabaseConfig
                {
                    Server = tbServer.Text,
                    Database = tbDatabase.Text,
                    UserId = tbUserId.Text,
                    Password = tbPassword.Text
                },
                Path = new PathConfig
                {
                    ArchivePath = tbArchivePath.Text,
                },
                Language = new LanguageConfig
                {
                    Current = cmbLanguage.SelectedItem?.ToString() ?? cmbLanguage.Text ?? string.Empty
                },
            };

            _configLoader.SetNewConfig(newConfig, _logger);
            _config = newConfig;

            // close dialog with OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SaveCurrentSettings(object sender, EventArgs e)
        {
            bool filled = isEverythingFilled();

            if (!filled)
                return;

            SaveConfig();
        }
    }
}
