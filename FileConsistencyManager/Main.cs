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
    internal partial class Main : Form
    {
        #region Initialization and Setup

        private AppConfig _config;
        private ConfigLoader _configLoader;
        private Logger _logger;
        private Localize _localization;
        private List<ComparisonResult> _allResults = new List<ComparisonResult>();

        public Main(AppConfig appConfig, Logger logger, ConfigLoader configLoader, Localize localization)
        {
            // Set localization early so SetupUI uses correct language
            _localization = localization ?? new Localize("en");

            // Ensure logger exists
            SetupLogger();
            _logger = logger ?? _logger;

            // ConfigLoader (may show messages using localization)
            _configLoader = configLoader ?? new ConfigLoader("config.json", _localization);

            // Load config (ConfigLoader will handle decryption)
            _config = appConfig ?? _configLoader.Load(_logger);

            // Initializations & Setup for Combobox and GridView
            InitializeComponent();
            SetupUI();

            lblConnectionLabel.Text = "Connected to: " + (_config.Connection?.Server ?? string.Empty) + "\\" + (_config.Connection?.Database ?? string.Empty);
        }

        private void OpenSettingsForm()
        {
            Settings settingsForm = new Settings(_config, _logger, _configLoader, _localization, isMainOpen: true);
            settingsForm.ShowDialog();
        }

        private string GetConnectionString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Server=").Append($@"{_config.Connection.Server}")
                .Append(";Database=").Append(_config.Connection.Database)
                .Append(";User Id=").Append(_config.Connection.UserId)
                .Append(";Password=").Append(_config.Connection.Password)
                .Append(";TrustServerCertificate=True;");
            return sb.ToString();
        }

        private void SetupLogger()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string logFolder = Path.Combine(basePath, "_Logs");
            string logFilePath = Path.Combine(logFolder, "_log.txt");
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            _logger = new Logger(logFilePath, LogLevel.Info);
        }

        private void SetupUI()
        {
            SetupGrid();
            SetupButtons();
            SetupComboboxOptions();
            SetupComboboxLanguage();
            ApplyLanguage(_localization.GetCurrentLanguageDictionary());
        }

        private void SetupButtons()
        {
            // Default Settings
            btnDelete.Enabled = false;
            btnArchive.Enabled = false;
            btnIgnore.Enabled = false;

            // UX improvements
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnArchive.FlatStyle = FlatStyle.Flat;
            btnIgnore.FlatStyle = FlatStyle.Flat;

            // Tooltips
            string lang = _localization.GetCurrentLanguage();
            tip.SetToolTip(btnDelete, _localization.GetContent("DeleteButtonToolTip", lang));
            tip.SetToolTip(btnArchive, _localization.GetContent("ArchiveButtonToolTip", lang));
            tip.SetToolTip(btnIgnore, _localization.GetContent("IgnoreButtonToolTip", lang));
        }

        private void SetupComboboxOptions()
        {
            string lang = _localization.GetCurrentLanguage();

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
            string lang = _localization.GetCurrentLanguage();

            cmbLanguage.Items.Add(_localization.GetContent("Culture", "en"));
            cmbLanguage.Items.Add(_localization.GetContent("Culture", "de"));

            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.SelectedIndex = _config.Language.Current == "de" ? 1 : 0;
        }

        private void SetupGrid()
        {
            string lang = _localization.GetCurrentLanguage();

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
                string lang = _localization.GetCurrentLanguage();

                UIEnabled(false);

                SetStatus(_localization.GetContent("ProgressBarAnalyseStartMessage", lang));
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

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionType.Ignore);
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionType.Archive);
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
                ApplyLanguage(_localization.en);
            }
            else
            {
                var culture = new System.Globalization.CultureInfo("de-DE");
                System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
                System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;

                _localization.SetCurrentLanguage("de");
                ApplyLanguage(_localization.de);
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
            string lang = _localization.GetCurrentLanguage();
            int selectedCount = dgvResults.SelectedRows.Count;
            bool hasSelection = dgvResults.SelectedRows.Count > 0;

            btnDelete.Enabled = hasSelection;
            btnArchive.Enabled = hasSelection;
            btnIgnore.Enabled = hasSelection;

            if (hasSelection)
            {
                btnDelete.Text = _localization.GetContent("Delete", lang) + " (" + selectedCount + ") ";
                btnArchive.Text = _localization.GetContent("Archive", lang) + " (" + selectedCount + ") ";
                btnIgnore.Text = _localization.GetContent("Ignore", lang) + " (" + selectedCount + ") ";
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

        private void SetStatus(string message, string resultsCount = "")
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() => lblStatus.Text = message));
                if (resultsCount != string.Empty) lblStatusCount.Invoke(new Action(() => lblStatusCount.Text = resultsCount));
            }
            else
            {
                lblStatus.Text = message;
                if (resultsCount != string.Empty) lblStatusCount.Text = resultsCount;
            }
        }

        private void ClearStatus()
        {
            lblStatus.Text = string.Empty;
            pbProgress.Value = 0;
        }

        private void UpdateProgress(int value, bool showText = true)
        {
            string lang = _localization.GetCurrentLanguage();

            if (pbProgress.InvokeRequired)
            {
                pbProgress.Invoke(new Action(() =>
                {
                    pbProgress.Value = value;
                    if (showText) SetStatus(string.Format(_localization.GetContent("ProgressBarAnalyseStatusMessage", lang), value));
                }));
            }
            else
            {
                pbProgress.Value = value;
                if (showText) SetStatus(string.Format(_localization.GetContent("ProgressBarAnalyseStatusMessage", lang), value));
            }
        }

        #endregion

        #region Execute Methods

        private void ExecuteRunScanWithProgress(bool logText)
        {
            string lang = _localization.GetCurrentLanguage();

            // Used thread-safe update for UI controls because this method runs on a background thread
            SetStatus(_localization.GetContent("ProgressBarAnalyseConnectionMessage", lang));

            // Needs connection string from config file, needs to be encrypted -> AES Maybe Login Page and safe last used connection
            string conn = GetConnectionString();

            AttachmentRepository repository = new AttachmentRepository(conn, _logger);
            FileService fileService = new FileService();
            ComparisonService comparisonService = new ComparisonService();

            ConsistencyManager manager = new ConsistencyManager(
                repository,
                fileService,
                comparisonService,
                _logger);

            UpdateProgress(50, showText: false);

            string databaseName = _config.Connection.Server + "\\" + _config.Connection.Database;
            List<ComparisonResult> results = manager.RunCheck(databaseName, _localization, logText);

            UpdateProgress(100);

            _allResults = results;

            // Show results 
            SetStatus(_localization.GetContent("ProgressBarAnalyseDoneMessage", lang), resultsCount: _allResults.Count.ToString());
        }

        private void ExecuteAction(ActionType actionType)
        {
            string lang = _localization.GetCurrentLanguage();
            string conn = GetConnectionString();

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
                case ActionType.Ignore:
                    actionText = _localization.GetContent("IgnoreActionText", lang);
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
                case ActionType.Ignore:
                    actionService.IgnoreEntry(selectedItems);
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
            string lang = _localization.GetCurrentLanguage();

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

        private void ApplyLanguage(Dictionary<string, string> lang)
        {
            // Buttons
            btnStart.Text = lang["Start"];
            btnDelete.Text = lang["Delete"];
            btnIgnore.Text = lang["Ignore"];
            btnArchive.Text = lang["Archive"];

            // Labels
            lblCmbFilterTitle.Text = lang["FilterOptionsTitle"];
            lblCmbLanguageTitle.Text = lang["FilterLanguageTitle"];
            lblMissing.Text = lang["MissingFilesLabel"];
            lblOrphan.Text = lang["OrphanFilesLabel"];
            if (lblStatus.Text != string.Empty) lblStatus.Text = lang["ProgressBarAnalyseDoneMessage"];

            // Tooltip
            tip.SetToolTip(btnDelete, lang["DeleteButtonToolTip"]);
            tip.SetToolTip(btnArchive, lang["ArchiveButtonToolTip"]);
            tip.SetToolTip(btnIgnore, lang["IgnoreButtonToolTip"]);

            // DataGridView
            ApplyFilter();
            SelectionChangedEvent();
        }

        private void UIEnabled(bool enabled)
        {
            btnStart.Enabled = enabled;
            btnDelete.Enabled = enabled;
            btnArchive.Enabled = enabled;
            btnIgnore.Enabled = enabled;
            cmbFilter.Enabled = enabled;
            cmbLanguage.Enabled = enabled;
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

        private void btnSettings_Click(object sender, EventArgs e)
        {
            OpenSettingsForm();
        }
    }
}
