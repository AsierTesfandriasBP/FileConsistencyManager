using FileConsistencyManager.Business;
using FileConsistencyManager.Config;
using FileConsistencyManager.DataAccess;
using FileConsistencyManager.Logging;
using FileConsistencyManager.Models;
using FileConsistencyManager.Services;
using FileConsistencyManager.Localization;

namespace FileConsistencyManager
{
    public partial class Main : Form
    {
        #region Initialization and Setup

        private AppConfig _config;
        private Logger _logger;
        private Localize _localization = new Localize(currentLanguage: "en");

        public Main()
        {
            // load database, logging, file from json config file
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
            ToolTip tip = new ToolTip();

            var lang = _localization.GetCurrentLanguage();
            tip.SetToolTip(btnDelete, _localization.GetContent("DeleteButtonToolTip", lang));
            tip.SetToolTip(btnArchive, _localization.GetContent("ArchiveButtonToolTip", lang));
            tip.SetToolTip(btnIgnore, _localization.GetContent("IgnoreButtonToolTip", lang));
        }

        private void SetupComboboxOptions()
        {
            string lang = _localization.GetCurrentLanguage();

            // delete all existing items to prevent duplicates when changing language
            cmbFilter.Items.Clear();

            // options
            cmbFilter.Items.Add(_localization.GetContent("FilterAll", lang));
            cmbFilter.Items.Add(_localization.GetContent("FilterMissingFiles", lang));
            cmbFilter.Items.Add(_localization.GetContent("FilterOrphanFiles", lang));

            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.SelectedIndex = 0;
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
            // Default Settings
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResults.AutoGenerateColumns = false;
            dgvResults.MultiSelect = true;
            dgvResults.ReadOnly = true;

            // UX improvements
            dgvResults.EnableHeadersVisualStyles = false;
            dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray;
            dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResults.RowHeadersVisible = false;
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
                var lang = _localization.GetCurrentLanguage();

                btnDelete.Enabled = false;
                btnArchive.Enabled = false;
                btnIgnore.Enabled = false;

                SetStatus(_localization.GetContent("ProgressBarAnalyseStartMessage", lang));
                pbProgress.Value = 0;

                await Task.Run(() =>
                {
                    RunScanWithProgress();
                });

                ApplyFilter();

                var missingFiles = _allResults.Count(r => r.Types == IssueType.Types.MissingFile);
                var orphanFiles = _allResults.Count(r => r.Types == IssueType.Types.OrphanFile);

                lblMissing.Text = string.Format(_localization.GetContent("MissingFilesLabel", lang), missingFiles);
                lblMissing.Text = string.Format(_localization.GetContent("OrphanFilesLabel", lang), orphanFiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearStatus();
            }
        }

        private void SetStatus(string message)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() => lblStatus.Text = message));
            }
            else
            {
                lblStatus.Text = message;
            }
        }

        private void ClearStatus()
        {
            lblStatus.Text = string.Empty;
            pbProgress.Value = 0;
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

        #region DataGridView

        private void dgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = dgvResults.Rows[e.RowIndex].DataBoundItem as ComparisonResult;

            if (row == null) return;

            if (row.Types == IssueType.Types.MissingFile)
            {
                dgvResults.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
            }
            else if (row.Types == IssueType.Types.OrphanFile)
            {
                dgvResults.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
            }
        }

        private void dgvResults_SelectionChanged(object sender, EventArgs e)
        {
            var lang = _localization.GetCurrentLanguage();
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
            else
            {
                ApplyLanguage(_localization.GetCurrentLanguageDictionary());
            }
        }

        #endregion

        #region Combobox

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
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
        }

        #endregion

        #endregion

        #region no name

        private void RunScanWithProgress()
        {
            var lang = _localization.GetCurrentLanguage();

            // Use thread-safe update for UI controls because this method runs on a background thread
            SetStatus(_localization.GetContent("ProgressBarAnalyseConnectionMessage", lang));

            // Needs connection string from config file, needs to be encrypted -> AES? Maybe Login Page and safe last used connection?
            string conn = _config.Database.ConnectionString;
            // Path info comes from CRM Database entry
            string rootPath = _config.FileSystem.RootPath;

            var repository = new AttachmentRepository(_config.Database.ConnectionString);
            var fileService = new FileService();
            var comparisonService = new ComparisonService();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string logFolder = Path.Combine(basePath, "Logs");  //check
            string logFilePath = Path.Combine(logFolder, _config.Logging.LogFilePath);

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            var logLevel = Enum.Parse<LogLevel>(_config.Logging.LogLevel);
            var logger = new Logger(_config.Logging.LogFilePath, logLevel);

            var manager = new ConsistencyManager(
                repository,
                fileService,
                comparisonService,
                logger);

            UpdateProgress(50, showText: false);

            var results = manager.RunCheck(_config.FileSystem.RootPath);

            UpdateProgress(100);

            _allResults = results;
            
            // Show results 
            //MessageBox.Show($"Found problems: {_allResults.Count}");
            SetStatus(string.Format(_localization.GetContent("ProgressBarAnalyseDoneMessage", lang), _allResults.Count));
        }

        private void UpdateProgress(int value, bool showText=true)
        {
            var lang = _localization.GetCurrentLanguage();

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

        private List<ComparisonResult> _allResults = new List<ComparisonResult>();

        private List<ComparisonResult> GetSelectedResults()
        {
            return dgvResults.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(r => r.DataBoundItem as ComparisonResult)
                .Where(r => r != null)
                .ToList()!;
        }

        private void ExecuteAction(ActionType actionType)
        {
            var lang = _localization.GetCurrentLanguage();

            var selectedItems = GetSelectedResults();

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
                message = _localization.GetContent("ConfirmMultipleMessage", lang);
            else
                message = _localization.GetContent("ConfirmMessage", lang);
            message = string.Format(message, selectedItems.Count, actionText);

            var confirm = MessageBox.Show(
                message,
                _localization.GetContent("ConfirmTitle", lang),
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes)
                return;

            var logger = new Logger(
                _config.Logging.LogFilePath,
                Enum.Parse<LogLevel>(_config.Logging.LogLevel));

            var actionService = new ActionService(logger);

            foreach (var item in selectedItems)
            {
                switch (actionType)
                {
                    case ActionType.Delete:
                        actionService.DeleteFile(item.Path);
                        break;

                    case ActionType.Archive:
                        actionService.ArchiveFile(item.Path, @"C:\Archiv"); // add archive path to config file
                        break;

                    case ActionType.Ignore:
                        actionService.IgnoreEntry(item);
                        break;
                }
            }

            MessageBox.Show(_localization.GetContent("ActionCompleteMessage", lang));

            // reload
            btnStart_Click(null, null);
        }

        public void ArchiveFile(string sourcePath, string archiveFolder)
        {
            try
            {
                if (!Directory.Exists(archiveFolder))
                    Directory.CreateDirectory(archiveFolder);

                string fileName = Path.GetFileName(sourcePath);
                string targetPath = Path.Combine(archiveFolder, fileName);

                File.Move(sourcePath, targetPath);

                _logger.Log($"File archived: {sourcePath}", LogLevel.Info);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                ClearStatus();
                throw;
            }
        }

        private void ApplyFilter()
        {
            var lang = _localization.GetCurrentLanguage();

            var selected = cmbFilter.SelectedItem!.ToString();

            var filtered = _allResults;

            if (selected == _localization.GetContent("FilterMissingFiles", lang))
            {
                filtered = _allResults
                    .Where(r => r.Types == IssueType.Types.MissingFile)
                    .ToList();
            }
            else if (selected == _localization.GetContent("FilterOrphanFiles", lang))
            {
                filtered = _allResults
                    .Where(r => r.Types == IssueType.Types.OrphanFile)
                    .ToList();
            }

            dgvResults.DataSource = filtered;
        }

        #endregion

        #region Tests

        private void TestDatabaseConnection()
        {
            // Connection works as intended. Need to setup a secure config file -> AES?, Maybe Login Page and safe last used connection? 
            //string conn = "Server=;Database=;User Id=;Password=;TrustServerCertificate=;";
            string conn = "Server=;Database=;User Id=;Password=;TrustServerCertificate=;";

            var repo = new AttachmentRepository(conn);
            var data = repo.GetAllAttachments();

            // Show some data to verify connection and data retrieval
            MessageBox.Show($"Data Count: {data.Count}");
            MessageBox.Show($"Filename: {data[0].FileName}");
            MessageBox.Show($"Id: {data[0].Id}");
            MessageBox.Show($"Filepath: {data[0].FilePath}");
        }

        #endregion
    }
}
