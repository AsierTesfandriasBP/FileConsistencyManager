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
            MissingFile,   // missing file
            OrphanFile     // no database entry
        }
    }
}
