using FileConsistencyManager.Business;
using FileConsistencyManager.Config;
using FileConsistencyManager.DataAccess;
using FileConsistencyManager.Logging;
using FileConsistencyManager.Models;
using FileConsistencyManager.Services;
using FileConsistencyManager.Localization;
using System.Text;

namespace FileConsistencyManager
{
    internal partial class Main : Form, IUiUpdater
    {
        #region Initialization and Setup

        private AppConfig _config;
        private ConfigManager _configManager;
        private Logger _logger;
        private Localize _localization;
        private List<ComparisonResult> _allResults = new List<ComparisonResult>();
        private string lang;

        public Main(AppConfig appConfig, Logger logger, ConfigManager configManager, Localize localization)
        {
            // Set localization early so SetupUI uses correct language
            _localization = localization;
            lang = _localization.GetCurrentLanguage();

            // Ensure logger exists
            _logger = logger;

            // ConfigLoader (may show messages using localization)
            _configManager = configManager;

            // Load config (ConfigLoader will handle decryption)
            _config = appConfig;

            // Initializations & Setup for Combobox and GridView
            InitializeComponent();
            SetupUI();

            lblConnectionLabel.Text = string.Format(_localization.GetContent("ConnectedToLabel", lang), _configManager.GetDatabaseName(_config));
        }

        private void OpenSettingsForm()
        {
            Settings settingsForm = new Settings(_config, _logger, _configManager, _localization, isMainOpen: true);
            settingsForm.ShowDialog();
        }

        private void SetupUI()
        {
            SetupGrid();
            SetupButtons();
            SetupComboboxOptions();
            SetupComboboxLanguage();
            ApplyLanguage(_localization.GetCurrentLanguage());
        }

        private void SetupButtons()
        {
            // Default Settings
            btnDelete.Enabled = false;
            btnArchive.Enabled = false;

            // UX improvements
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnArchive.FlatStyle = FlatStyle.Flat;
            btnSettings.FlatStyle = FlatStyle.Flat;

            // Tooltips
            tip.SetToolTip(btnDelete, _localization.GetContent("DeleteButtonToolTip", lang));
            tip.SetToolTip(btnArchive, _localization.GetContent("ArchiveButtonToolTip", lang));
        }

        private void SetupComboboxOptions()
        {
            int currentIndex = cmbFilter.SelectedIndex != -1 ? cmbFilter.SelectedIndex : 0;

            // delete all existing items to prevent duplicates when changing language
            cmbFilter.Items.Clear();

            // options
            cmbFilter.Items.Add(_localization.GetContent("FilterAll", lang));
            cmbFilter.Items.Add(_localization.GetContent("FilterMissingFiles", lang));
            cmbFilter.Items.Add(_localization.GetContent("FilterOrphanFiles", lang));
            cmbFilter.Items.Add(_localization.GetContent("FilterExistsTitle", lang));

            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.SelectedIndex = currentIndex;
        }

        private void SetupComboboxLanguage()
        {
            cmbLanguage.Items.Add(_localization.GetContent("Culture", "en"));
            cmbLanguage.Items.Add(_localization.GetContent("Culture", "de"));

            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.SelectedIndex = _config.Language.Current == "de" ? 1 : 0;
        }

        private void SetupGrid()
        {
            // Default Settings
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResults.AutoGenerateColumns = false;
            dgvResults.MultiSelect = true;
            dgvResults.ReadOnly = true;
            dgvResults.Columns.Clear();

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = _localization.GetContent("DGVHeaderFile", lang),
                DataPropertyName = "FileName",
            });

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = _localization.GetContent("DGVHeaderPath", lang),
                DataPropertyName = "Source",
            });

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = _localization.GetContent("DGVHeaderType", lang),
                DataPropertyName = "Type",
            });

            // UX improvements
            dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray;
            dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResults.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvResults.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
        }

        #endregion

        #region Event Methods

        #region Buttons

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                UIEnabled(false);

                ((IUiUpdater)this).SetStatus(_localization.GetContent("ProgressBarAnalyseStartMessage", lang));
                pbProgress.Value = 0;

                bool logText = sender != null ? true : false;

                await Task.Run(() =>
                {
                    ExecuteRunScanWithProgress(logText);
                });

                ApplyFilter();

                int missingFiles = _allResults.Count(r => r.Type == IssueType.Types.MissingFile);
                int orphanFiles = _allResults.Count(r => r.Type == IssueType.Types.OrphanFile);

                lblMissingCount.Text = missingFiles.ToString();
                lblOrphanCount.Text = orphanFiles.ToString();

                UIEnabled(true);
            }
            catch (Exception ex)
            {
                UIEnabled(true);
                ButtonEnabled(false);

                _logger.LogException(ex);
                MessageBox.Show(
                    ex.Message,
                    _localization.GetContent("CustomTextError", _localization.GetCurrentLanguage()),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ClearStatus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionType.Delete);
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionType.Archive);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            OpenSettingsForm();
        }

        #endregion

        #region Combobox

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLanguage.SelectedItem.ToString() == "English")
            {
                var culture = new System.Globalization.CultureInfo("en-US");
                System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
                System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;

                _localization.SetCurrentLanguage("en");
                lang = _localization.GetCurrentLanguage();
                ApplyLanguage(forcedLang: lang);
            }
            else
            {
                var culture = new System.Globalization.CultureInfo("de-DE");
                System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
                System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;

                _localization.SetCurrentLanguage("de");
                lang = _localization.GetCurrentLanguage();
                ApplyLanguage(forcedLang: lang);
            }

            SetupComboboxOptions();
            if (dgvResults.DataSource != null) RefreshLanguageDataGridView();
        }

        #endregion

        #region DataGridView

        private List<ComparisonResult> GetSelectedResults()
        {
            return dgvResults.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(r => r.DataBoundItem as ComparisonResult)
                .Where(r => r != null)
                .ToList()!;
        }

        private void dgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            ComparisonResult row = dgvResults.Rows[e.RowIndex].DataBoundItem as ComparisonResult;

            if (row == null) return;

            if (row.Type == IssueType.Types.MissingFile)
            {
                dgvResults.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
            }
            else if (row.Type == IssueType.Types.OrphanFile)
            {
                dgvResults.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
            }
            else if (row.Type == IssueType.Types.Exists)
            {
                dgvResults.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }

        private void dgvResults_SelectionChanged(object sender, EventArgs e)
        {
            SelectionChangedEvent();
        }

        private void SelectionChangedEvent()
        {
            int selectedCount = dgvResults.SelectedRows.Count;
            bool hasSelection = dgvResults.SelectedRows.Count > 0;

            ButtonEnabled(hasSelection);

            if (hasSelection)
            {
                btnDelete.Text = _localization.GetContent("Delete", lang) + " (" + selectedCount + ") ";
                btnArchive.Text = _localization.GetContent("Archive", lang) + " (" + selectedCount + ") ";
                return;
            }
        }

        private void RefreshLanguageDataGridView()
        {
            SetupGrid();
            dgvResults.DataSource = _allResults;
            ApplyFilter();
        }

        #endregion

        #endregion

        #region Status Methods

        void IUiUpdater.SetStatus(string message, string resultsCount = "")
        {
            // ToolStripStatusLabel is not a Control and therefore does not expose Invoke/InvokeRequired.
            // Use the Form (this) which is a Control to marshal updates to the UI thread.
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = message;
                    if (!string.IsNullOrEmpty(resultsCount)) lblEntriesFoundCount.Text = resultsCount;
                }));
            }
            else
            {
                lblStatus.Text = message;
                if (!string.IsNullOrEmpty(resultsCount)) lblEntriesFoundCount.Text = resultsCount;
            }
        }

        private void ClearStatus()
        {
            lblStatus.Text = string.Empty;
            pbProgress.Value = 0;
        }

        void IUiUpdater.UpdateProgress(int value, bool showText = true)
        {
            if (pbProgress.InvokeRequired)
            {
                pbProgress.Invoke(new Action(() =>
                {
                    pbProgress.Value = value;
                    if (showText) ((IUiUpdater)this).SetStatus(string.Format(_localization.GetContent("ProgressBarAnalyseStatusMessage", lang), value));
                }));
            }
            else
            {
                pbProgress.Value = value;
                if (showText) ((IUiUpdater)this).SetStatus(string.Format(_localization.GetContent("ProgressBarAnalyseStatusMessage", lang), value));
            }
        }

        #endregion

        #region Execute Methods

        private void ExecuteRunScanWithProgress(bool logText)
        {
            // Used thread-safe update for UI controls because this method runs on a background thread
            ((IUiUpdater)this).SetStatus(_localization.GetContent("ProgressBarAnalyseConnectionMessage", lang));

            string conn = _configManager.GetConnectionString(_config);

            AttachmentRepository repository = new AttachmentRepository(conn, _logger);
            FileService fileService = new FileService();
            ComparisonService comparisonService = new ComparisonService();

            ConsistencyManager manager = new ConsistencyManager(
                repository,
                fileService,
                comparisonService,
                _logger,
                uiUpdater: this);

            ((IUiUpdater)this).UpdateProgress(10, showText: false);

            string databaseName = _configManager.GetDatabaseName(_config);
            List<ComparisonResult> results = manager.RunCheck(databaseName, _localization, _configManager, _config, logText);

            _allResults = results;

            ((IUiUpdater)this).SetStatus(_localization.GetContent("ProgressBarAnalyseDoneMessage", lang), resultsCount: _allResults.Count.ToString());
        }

        private void ExecuteAction(ActionType actionType)
        {
            string conn = _configManager.GetConnectionString(_config);

            List<ComparisonResult> selectedItems = GetSelectedResults();

            // if no Row was selected
            if (selectedItems.Count == 0)
            {
                MessageBox.Show(
                    _localization.GetContent("AfterItemCountMessage", lang),
                    _localization.GetContent("CustomTextInformation", lang),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            string actionText;
            switch (actionType)
            {
                case ActionType.Delete:
                    actionText = _localization.GetContent("DeleteActionText", lang);
                    break;
                case ActionType.Archive:
                    actionText = _localization.GetContent("ArchiveActionText", lang);
                    break;
                default:
                    actionText = "test";
                    break;
            }

            string message;
            if (selectedItems.Count > 1)
            {
                message = _localization.GetContent("ConfirmMultipleMessage", lang);
            }
            else
                message = _localization.GetContent("ConfirmMessage", lang);
            message = string.Format(message, actionText, selectedItems.Count);

            DialogResult confirm = MessageBox.Show(
                message,
                _localization.GetContent("CustomTextWarning", lang),
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.OK)
                return;

            ActionService actionService = new ActionService(_logger);

            switch (actionType)
            {
                case ActionType.Delete:
                    List<ComparisonResult> missingFiles = selectedItems.Where(r => r.Type == IssueType.Types.MissingFile).ToList();
                    List<ComparisonResult> orphanFiles = selectedItems.Where(r => r.Type == IssueType.Types.OrphanFile).ToList();
                    if (orphanFiles.Count > 0) actionService.DeleteFile(orphanFiles);
                    if (missingFiles.Count > 0)
                    {
                        // Select all Ids from the selected missing files to delete the entries in the database
                        AttachmentRepository allAttachments = new AttachmentRepository(conn, _logger);

                        List<string> missingFilesAttachmentIdList = allAttachments.GetAllAttachments()
                            .Where(a => missingFiles.Any(m => m.FileName == a.FileName))
                            .Select(a => a.Id)
                            .ToList();

                        actionService.DeleteEntry(missingFilesAttachmentIdList, conn);
                    }
                    break;
                case ActionType.Archive:
                    actionService.ArchiveFile(selectedItems, $@"{_config.Path.ArchivePath}");
                    break;
            }

            MessageBox.Show(
                _localization.GetContent("ActionCompleteMessage", lang),
                _localization.GetContent("CustomTextInformation", lang),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // reload the DataGrid
            btnStart_Click(null, null);
        }

        #endregion

        #region Apply Methods

        private void ApplyFilter()
        {
            string selected = cmbFilter.SelectedItem!.ToString()!;

            List<ComparisonResult> filtered = _allResults;

            if (selected == _localization.GetContent("FilterMissingFiles", lang))
            {
                filtered = _allResults
                    .Where(r => r.Type == IssueType.Types.MissingFile)
                    .ToList();
            }
            else if (selected == _localization.GetContent("FilterOrphanFiles", lang))
            {
                filtered = _allResults
                    .Where(r => r.Type == IssueType.Types.OrphanFile)
                    .ToList();
            }
            else if (selected == _localization.GetContent("FilterExistsTitle", lang))
            {
                filtered = _allResults
                    .Where(r => r.Type == IssueType.Types.Exists)
                    .ToList();
            }

            dgvResults.DataSource = filtered;
            dgvResults.Refresh();
        }

        private void ApplyLanguage(string forcedLang)
        {
            // Buttons
            btnStart.Text = _localization.GetContent("Start", forcedLang);
            btnDelete.Text = _localization.GetContent("Delete", forcedLang);
            btnArchive.Text = _localization.GetContent("Archive", forcedLang);
            btnSettings.Text = _localization.GetContent("Settings", forcedLang);

            // Labels
            lblCmbFilterTitle.Text = _localization.GetContent("FilterOptionsTitle", forcedLang);
            lblCmbLanguageTitle.Text = _localization.GetContent("FilterLanguageTitle", forcedLang);
            lblMissing.Text = _localization.GetContent("MissingFilesLabel", forcedLang);
            lblOrphan.Text = _localization.GetContent("OrphanFilesLabel", forcedLang);
            lblConnectionLabel.Text = string.Format(_localization.GetContent("ConnectedToLabel", forcedLang), _configManager.GetDatabaseName(_config));
            lblEntriesFound.Text = _localization.GetContent("EntriesFoundLabel", forcedLang);
            if (!string.IsNullOrEmpty(lblStatus.Text)) lblStatus.Text = _localization.GetContent("ProgressBarAnalyseDoneMessage", forcedLang);

            // Tooltip
            tip.SetToolTip(btnDelete, _localization.GetContent("DeleteButtonToolTip", forcedLang));
            tip.SetToolTip(btnArchive, _localization.GetContent("ArchiveButtonToolTip", forcedLang));

            // DataGridView
            ApplyFilter();
            SelectionChangedEvent();
        }

        private void UIEnabled(bool enabled)
        {
            btnStart.Enabled = enabled;
            ButtonEnabled(enabled);
            btnSettings.Enabled = enabled;
            cmbFilter.Enabled = enabled;
            cmbLanguage.Enabled = enabled;
        }

        private void ButtonEnabled(bool enabled)
        {
            btnDelete.Enabled = enabled;
            btnArchive.Enabled = enabled;
        }

        #endregion

        #region Tests

        private void TestDatabaseConnection()
        {
            // Connection works as intended. Need to setup a secure config file -> AES, Maybe Login Page and safe last used connection 
            //string conn = "Server=;Database=;User Id=;Password=;TrustServerCertificate=;";
            StringBuilder sb = new StringBuilder();
            sb.Append("Server=").Append(_config.Connection.Server)
                .Append(";Database=").Append(_config.Connection.Database)
                .Append(";User Id=").Append(_config.Connection.UserId)
                .Append(";Password=").Append(_config.Connection.Password)
                .Append(";TrustServerCertificate=True;");

            string conn = sb.ToString();

            AttachmentRepository repo = new AttachmentRepository(conn, _logger);
            List<Attachment> data = repo.GetAllAttachments();

            // Show some data to verify connection and data retrieval
            MessageBox.Show($"Data Count: {data.Count}");
            MessageBox.Show($"Filename: {data[0].FileName}");
            MessageBox.Show($"Id: {data[0].Id}");
            MessageBox.Show($"Filepath: {data[0].Source}");
        }

        #endregion
    }
}
