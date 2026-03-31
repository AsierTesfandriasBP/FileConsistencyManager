using FileConsistencyManager.Business;
using FileConsistencyManager.DataAccess;
using FileConsistencyManager.Models;
using FileConsistencyManager.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FileConsistencyManager
{
    public partial class Main : Form
    {
        #region Initialization and Setup

        public Main()
        {
            // Initializations & Setup for Combobox and GridView
            InitializeComponent();
            SetupCombobox();
            SetupGrid();

            // Temporary Testing
            //TestDatabaseConnection();
        }

        private void SetupCombobox()
        {
            cmbFilter.Items.Add("Alle");
            cmbFilter.Items.Add("Fehlende Dateien");
            cmbFilter.Items.Add("Verwaiste Dateien");

            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.SelectedIndex = 0;
        }

        private void SetupGrid()
        {
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResults.AutoGenerateColumns = false;
            dgvResults.MultiSelect = false;
            dgvResults.ReadOnly = true;
        }

        #endregion

        #region Event Methods

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                // Needs connection string from config file, needs to be encrypted -> AES? Maybe Login Page and safe last used connection?
                string conn = "Server=;Database=;User Id=;Password=;TrustServerCertificate=;";
                // Path info comes from CRM Database entry
                string rootPath = @"C:\Users\Test\Desktop";

                var repository = new AttachmentRepository(conn);
                var fileService = new FileService();
                var comparisonService = new ComparisonService();

                var manager = new ConsistencyManager(
                    repository,
                    fileService,
                    comparisonService);

                _allResults = manager.RunCheck(rootPath);

                ApplyFilter();

                // Show results 
                //MessageBox.Show($"Found problems: {_allResults.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        #endregion

        #region no name

        private List<ComparisonResult> _allResults = new List<ComparisonResult>();

        private void ApplyFilter()
        {
            var selected = cmbFilter.SelectedItem.ToString();

            var filtered = _allResults;

            if (selected == "Fehlende Dateien")
            {
                filtered = _allResults
                    .Where(r => r.Types == IssueType.Types.MissingFile)
                    .ToList();
            }
            else if (selected == "Verwaiste Dateien")
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
