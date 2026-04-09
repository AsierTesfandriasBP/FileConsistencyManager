using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Models
{
    internal class ComparisonResult
    {
        // Represents the result of comparing a file entry with the database attachments.
        public string FileName { get; set; }
        public string Source { get; set; }
        public IssueType.Types Type { get; set; }
    }
}
