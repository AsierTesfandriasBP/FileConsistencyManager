using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using FileConsistencyManager.Models;
using System.Data;

namespace FileConsistencyManager.DataAccess
{
    internal class AttachmentRepository
    {
        private readonly string _connectionString;
        private string _rootPath;

        public AttachmentRepository(string connectionString)
        {
            _connectionString = connectionString;
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

                    //Test Query
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
                                    //Id = reader.GetInt64(0),
                                    Id = reader.GetString(0),
                                    FileName = reader.GetString(1),
                                    FilePath = reader.GetString(2) + "\\" + reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // use logging framework here
                throw new Exception("Fehler beim Laden der Attachments: " + ex.Message);
            }

            return attachments;
        }
    }
}
