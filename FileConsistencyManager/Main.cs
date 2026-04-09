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
    public partial class Main : Form
    {
        #region Initialization and Setup

        private AppConfig _config;
        private Logger _logger;
        private Localize _localization = new Localize(currentLanguage: "en");
        private List<ComparisonResult> _allResults = new List<ComparisonResult>();

        public Main()
        {
            // load data from json config file
            _config = ConfigLoader.Load();

            // Initializations & Setup for Combobox and GridView
            InitializeComponent();
            SetupGrid();
            SetupButtons();
            SetupComboboxOptions();
            SetupComboboxLanguage();
            ApplyLanguage(_localization.GetCurrentLanguageDictionary());

            // Temporary Testing
            //TestDatabaseConnection();
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
            cmbLanguage.SelectedIndex = 0;
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

                btnDelete.Enabled = false;
                btnArchive.Enabled = false;
                btnIgnore.Enabled = false;

                SetStatus(_localization.GetContent("ProgressBarAnalyseStartMessage", lang));
                pbProgress.Value = 0;

                await Task.Run(() =>
                {
                    ExecuteRunScanWithProgress();
                });

                ApplyFilter();

                int missingFiles = _allResults.Count(r => r.Type == IssueType.Types.MissingFile);
                int orphanFiles = _allResults.Count(r => r.Type == IssueType.Types.OrphanFile);

                lblMissingCount.Text = missingFiles.ToString();
                lblOrphanCount.Text = orphanFiles.ToString();

                btnDelete.Enabled = true;
                btnArchive.Enabled = true;
                btnIgnore.Enabled = true;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Show(ex.Message);
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
            // add a combobox
            if (cmbLanguage.SelectedItem.ToString() == "English")
            {
                _localization.SetCurrentLanguage("en");
                ApplyLanguage(_localization.en);
            }
            else
            {
                _localization.SetCurrentLanguage("de");
                ApplyLanguage(_localization.de);
            }

            SetupComboboxOptions();
            if(dgvResults.DataSource != null) RefreshLanguageDataGridView();
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
            }else if(row.Type == IssueType.Types.Exists)
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

        private void ExecuteRunScanWithProgress()
        {
            string lang = _localization.GetCurrentLanguage();

            // Used thread-safe update for UI controls because this method runs on a background thread
            SetStatus(_localization.GetContent("ProgressBarAnalyseConnectionMessage", lang));

            // Needs connection string from config file, needs to be encrypted -> AES Maybe Login Page and safe last used connection
            string conn = GetConnectionString();

            AttachmentRepository repository = new AttachmentRepository(conn, _logger);
            FileService fileService = new FileService();
            ComparisonService comparisonService = new ComparisonService();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string logFolder = Path.Combine(basePath, "Logs");  //check
            string logFilePath = Path.Combine(logFolder, _config.Logging.LogFilePath!);

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            LogLevel logLevel = Enum.Parse<LogLevel>(_config.Logging.LogLevel!);
            Logger logger = new Logger(_config.Logging.LogFilePath!, logLevel);

            ConsistencyManager manager = new ConsistencyManager(
                repository,
                fileService,
                comparisonService,
                logger);

            UpdateProgress(50, showText: false);

            string databaseName = _config.Connection.Server + "\\" + _config.Connection.Database;
            List<ComparisonResult> results = manager.RunCheck(databaseName);

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
                MessageBox.Show(_localization.GetContent("AfterItemCountMessage", lang));
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
                if (_localization.GetCurrentLanguage() == "de")
                    message = _localization.GetContent("ConfirmMultipleMessageDE", "de");
                else
                    message = _localization.GetContent("ConfirmMultipleMessageEN", "en");
            }
            else
                message = _localization.GetContent("ConfirmMessage", lang);
            message = string.Format(message, actionText, selectedItems.Count);

            //Button ok = new Button() { Text = _localization.GetContent("OKText", lang), DialogResult = DialogResult.OK };
            //Button cancel = new Button() { Text = _localization.GetContent("CancelText", lang), DialogResult = DialogResult.Cancel };

            DialogResult confirm = MessageBox.Show(
                message,
                _localization.GetContent("ConfirmTitle", lang),
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
                );

            if (confirm != DialogResult.OK)
                return;

            Logger logger = new Logger(
                _config.Logging.LogFilePath!,
                Enum.Parse<LogLevel>(_config.Logging.LogLevel!));

            ActionService actionService = new ActionService(logger);

            foreach (ComparisonResult item in selectedItems)
            {
                switch (actionType)
                {
                    case ActionType.Delete:
                        List<ComparisonResult> missingFiles = selectedItems.Where(r => r.Type == IssueType.Types.MissingFile).ToList();
                        List<ComparisonResult> orphanFiles = selectedItems.Where(r => r.Type == IssueType.Types.OrphanFile).ToList();
                        if (orphanFiles.Count > 0) actionService.DeleteFile(item.Source, orphanFiles);
                        if(missingFiles.Count > 0) actionService.DeleteEntry(item, conn, missingFiles);
                        break;
                    case ActionType.Archive:
                        actionService.ArchiveFile(item, $@"{_config.Path.ArchivePath}");
                        break;
                    case ActionType.Ignore:
                        actionService.IgnoreEntry(item);
                        break;
                }
            }

            MessageBox.Show(_localization.GetContent("ActionCompleteMessage", lang), "test", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            } else if(selected == _localization.GetContent("FilterExistsTitle", lang))
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
