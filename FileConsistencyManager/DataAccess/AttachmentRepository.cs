using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using FileConsistencyManager.Models;
using FileConsistencyManager.Logging;
using FileConsistencyManager.Localization;


namespace FileConsistencyManager.DataAccess
{
    internal class AttachmentRepository
    {
        private readonly string _connectionString;
        private string _rootPath;
        private readonly Logger _logger;

        public AttachmentRepository(string connectionString, Logger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public string GetRootPath()
        {
            return _rootPath;
        }

        public List<Attachment> GetAllAttachments()
        {
            var attachments = new List<Attachment>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"SELECT A.ATTACHID AS Id, A.FILENAME AS Filename, B.ATTACHMENTPATH AS Filepath FROM ATTACHMENT A CROSS JOIN BRANCHOPTIONS B"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if(_rootPath != reader.GetString(2) || _rootPath == string.Empty)
                                    _rootPath = reader.GetString(2);

                                attachments.Add(new Attachment
                                {
                                    Id = reader.GetString(0),
                                    FileName = reader.GetString(1),
                                    Source = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }

            return attachments;
        }
    }
}
