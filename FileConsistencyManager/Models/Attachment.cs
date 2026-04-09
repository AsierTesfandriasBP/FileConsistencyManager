using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Models
{
    internal class Attachment
    {
        // Represents an attachment entry from the database.
        public string Id { get; set; }
        public string FileName { get; set; }
        public string Source { get; set; }
    }
}
