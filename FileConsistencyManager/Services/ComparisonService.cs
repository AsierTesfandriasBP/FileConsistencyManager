using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using FileConsistencyManager.Models;

namespace FileConsistencyManager.Services
{
    internal class ComparisonService
    {
        // Compares the list of attachments from the database with the list of files from the file system.
        public List<ComparisonResult> Compare(List<Attachment> attachments, List<FileEntry> files, string databaseName)
        {
            var results = new List<ComparisonResult>();

            // Helper to normalize paths for reliable comparison.
            static string NormalizePath(string path)
            {
                if (string.IsNullOrWhiteSpace(path)) return string.Empty;
                try
                {
                    var full = Path.GetFullPath(path);
                    return full.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                }
                catch
                {
                    // Fallback to raw trimmed path if GetFullPath fails
                    return path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                }
            }

            // Build HashSet of normalized file full paths for fast lookups (case-insensitive on Windows).
            var normalizedFiles = new HashSet<string>(files
                .Where(f => !string.IsNullOrWhiteSpace(f.FullPath))
                .Select(f => NormalizePath(f.FullPath)), StringComparer.OrdinalIgnoreCase);

            // Normal → file exists in db and file system
            foreach (var attachment in attachments ?? Enumerable.Empty<Attachment>())
            {
                var combined = Path.Combine(attachment.Source, attachment.FileName);
                var normalizedCombined = NormalizePath(combined);
                if (normalizedFiles.Contains(normalizedCombined))
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = attachment.FileName,
                        Source = "Database/File System",
                        Type = IssueType.Types.Exists
                    });
                }
            }


            // db → file is missing
            foreach (var attachment in attachments ?? Enumerable.Empty<Attachment>())
            {
                var combined = Path.Combine(attachment.Source, attachment.FileName);
                var normalizedCombined = NormalizePath(combined);

                if (!normalizedFiles.Contains(normalizedCombined))
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = attachment.FileName,
                        Source = databaseName,
                        Type = IssueType.Types.MissingFile
                    });
                }
            }

            // Build HashSet of normalized attachment paths to check for orphan files
            var normalizedAttachments = new HashSet<string>(attachments
                .Where(a => !string.IsNullOrWhiteSpace(a.Source) || !string.IsNullOrWhiteSpace(a.FileName))
                .Select(a => NormalizePath(Path.Combine(a.Source, a.FileName))), StringComparer.OrdinalIgnoreCase);

            // file → no db-entry
            foreach (var file in files ?? Enumerable.Empty<FileEntry>())
            {
                var normalizedFile = NormalizePath(file.FullPath);

                if (!normalizedAttachments.Contains(normalizedFile))
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = file.FileName,
                        Source = file.FullPath,
                        Type = IssueType.Types.OrphanFile
                    });
                }
            }

            return results;
        }
    }
}
