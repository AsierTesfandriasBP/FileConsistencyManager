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
        private ConfigManager _configManager;
        private Localize _localization;
        private string _lang;
        private Logger _logger;
        private string _logFilePath;
        private bool _isMainOpen;

        public Settings(AppConfig confg, Logger logger, ConfigManager configManager, Localize localization, bool isMainOpen = false)
        {
            _config = confg;
            _localization = localization;
            _lang = localization.GetCurrentLanguage();
            _logger = logger;
            _logFilePath = _logger.GetLogFilePath();
            _configManager = configManager;
            _isMainOpen = isMainOpen;
            InitializeComponent();
            SetupUI();
            GetConfig();    
        }

        public void SetupUI()
        {
            lblConnectionTitle.Text = _localization.GetContent("SettingsDatabaseConnectionTitle", _lang);
            lblServer.Text = _localization.GetContent("SettingsServerLabel", _lang);
            lblDatabase.Text = _localization.GetContent("SettingsDatabaseLabel", _lang);
            lblUserId.Text = _localization.GetContent("SettingsUserIdLabel", _lang);
            lblPassword.Text = _localization.GetContent("SettingsPasswordLabel", _lang);
            lblPathTitle.Text = _localization.GetContent("SettingsPathTitle", _lang);
            lblArchivePath.Text = _localization.GetContent("SettingsArchivePathLabel", _lang);
            lblLanguageTitle.Text = _localization.GetContent("SettingsLanguageTitle", _lang);
            lblLanguage.Text = _localization.GetContent("SettingsLanguageLabel", _lang);
            btnConnectionTest.Text = _localization.GetContent("SettingsTestConnectionButton", _lang);
            btnSave.Text = _localization.GetContent("SettingsSaveButton", _lang);
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
                MessageBox.Show(
                    _localization.GetContent("ConfigLoadErrorMessage", _lang), 
                    _localization.GetContent("CustomTextError", _lang), 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool isEverythingFilled()
        {
            if (!IsDatabaseCredentialsFilled() ||
                !IsGeneralSettingsFilled())
            {
                MessageBox.Show(
                    _localization.GetContent("ValidationAllFieldsErrorMessage", _lang), 
                    _localization.GetContent("CustomTextError", _lang), 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public bool IsGeneralSettingsFilled()
        {
            return !(string.IsNullOrWhiteSpace(tbArchivePath.Text) ||
                string.IsNullOrWhiteSpace(cmbLanguage.Text));
        }

        private async void TestConnection(object? sender, EventArgs e)
        {
            try
            {
                if (!IsDatabaseCredentialsFilled())
                {
                    MessageBox.Show(
                        _localization.GetContent("ValidationDatabaseFieldsErrorMessage", _lang), 
                        _localization.GetContent("CustomTextError", _lang), 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnSave.Enabled = false;
                btnConnectionTest.Enabled = false;

                string connectionStr = _configManager.SetConnectionString(tbServer.Text, tbDatabase.Text, tbUserId.Text, tbPassword.Text);
                Task<bool> testedConnection = _configManager.TestConnection(_config, connectionStr);
                await testedConnection;

                if (testedConnection.Result)
                {
                    MessageBox.Show(
                        _localization.GetContent("SettingsTestConnectionSuccessMessage", _lang),
                        _localization.GetContent("CustomTextInformation", _lang),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _logger?.Log("Database connection test succeeded.", LogLevel.Info);
                }
                else
                {
                    MessageBox.Show(
                        _localization.GetContent("SettingsTestConnectionFailedMessage", _lang),
                        _localization.GetContent("CustomTextError", _lang),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _logger?.Log("Database connection test failed.", LogLevel.Error);
                }
                

                btnSave.Enabled = true;
                btnConnectionTest.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSave.Enabled = true;
                btnConnectionTest.Enabled = true;
                _logger?.LogException(ex);

                MessageBox.Show(
                    _localization.GetContent("SettingsTestConnectionFailedMessage", _lang), 
                    _localization.GetContent("CustomTextError", _lang), 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            _configManager.SetNewConfig(newConfig, _logger);
            _config = newConfig;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SaveCurrentSettings(object sender, EventArgs e)
        {
            bool filled = isEverythingFilled();

            if (!filled)
                return;

            SaveConfig();
            if(_isMainOpen)
                MessageBox.Show(
                    _localization.GetContent("SettingsSavedAfterLoadFromMain", _lang), 
                    _localization.GetContent("CustomTextInformation", _lang), 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
