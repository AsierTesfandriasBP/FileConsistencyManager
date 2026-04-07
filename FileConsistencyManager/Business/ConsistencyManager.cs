using System;
using System.Text;
using System.Collections.Generic;
using FileConsistencyManager.Models;
using FileConsistencyManager.Services;
using FileConsistencyManager.DataAccess;
using FileConsistencyManager.Logging;

namespace FileConsistencyManager.Business
{
    internal class ConsistencyManager
    {
        private readonly AttachmentRepository _repository;
        private readonly FileService _fileService;
        private readonly ComparisonService _comparisonService;
        private readonly Logger _logger;

        public ConsistencyManager(
            AttachmentRepository repository,
            FileService fileService,
            ComparisonService comparisonService,
            Logger logger)
        {
            _repository = repository;
            _fileService = fileService;
            _comparisonService = comparisonService;
            _logger = logger;
        }

        public List<ComparisonResult> RunCheck()
        {
            try
            {
                _logger.Log("", LogLevel.Info);
                _logger.LogSeparator("Starte Konsistenzprüfung...");

                // Load database entries
                var attachments = _repository.GetAllAttachments();
                _logger.Log($"Geladene DB-Einträge: {attachments.Count}", LogLevel.Info);

                // Load files
                var files = _fileService.GetAllFiles(_repository.GetRootPath());
                _logger.Log($"Gefundene Dateien: {files.Count}", LogLevel.Info);

                // Compare and get results
                var results = _comparisonService.Compare(attachments, files);
                _logger.Log($"Gefundene Inkonsistenzen: {results.Count}", LogLevel.Info);

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }
    }
}
