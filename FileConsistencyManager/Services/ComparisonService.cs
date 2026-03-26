using System;
using System.Collections.Generic;
using System.Text;
using FileConsistencyManager.Models;

namespace FileConsistencyManager.Services
{
    internal class ComparisonService
    {
        // Compares the list of attachments from the database with the list of files from the file system.
        public List<ComparisonResult> Compare(List<Attachment> attachments, List<FileEntry> files)
        {
            var results = new List<ComparisonResult>();

            // db → file is missing
            foreach (var attachment in attachments)
            {
                bool exists = files.Any(f => f.FullPath == attachment.FilePath);

                if (!exists)
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = attachment.FileName,
                        Path = attachment.FilePath,
                        Types = IssueType.Types.MissingFile
                    });
                }
            }

            // file → no db-entry
            foreach (var file in files)
            {
                bool exists = attachments.Any(a => a.FilePath == file.FullPath);

                if (!exists)
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = file.FileName,
                        Path = file.FullPath,
                        Types = IssueType.Types.OrphanFile
                    });
                }
            }

            return results;
        }
    }
}
