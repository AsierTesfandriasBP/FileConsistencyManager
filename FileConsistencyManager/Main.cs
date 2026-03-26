using FileConsistencyManager.DataAccess;

namespace FileConsistencyManager
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            TestDatabase();
        }

        public void TestDatabase()
        {
            // Connection works as intended. Need to setup a secure config file -> AES?, Maybe Login Page and safe last used connection? 
            string conn = "Server=;Database=;User Id=;Password=;TrustServerCertificate=;";


            var repo = new AttachmentRepository(conn);
            var data = repo.GetAllAttachments();

            MessageBox.Show($"Gefundene Einträge: {data.Count}");
        }
    }
}
