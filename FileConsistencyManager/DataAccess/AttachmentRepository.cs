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

        public AttachmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Attachment> GetAllAttachments()
        {
            var attachments = new List<Attachment>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    //string query = @"SELECT Id, FileName, FilePath FROM Attachments"; 
                    string query = @"SELECT * FROM AiEmbeddings"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                attachments.Add(new Attachment
                                {
                                    Id = reader.GetInt64(0),
                                    FileName = reader.GetString(1),
                                    FilePath = reader.GetString(2)
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
