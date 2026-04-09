using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Models
{
    internal class IssueType
    {
        // Enum to represent the types of issues found during the comparison.
        public enum Types
        {
            Exists,        // no issue, file and db-entry match
            MissingFile,   // only db-entry exists, no file
            OrphanFile     // only File exists, no db-entry
        }
    }
}
