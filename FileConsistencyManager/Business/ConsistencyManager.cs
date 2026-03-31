using System;
using System.Text;
using System.Collections.Generic;
using FileConsistencyManager.Models;
using FileConsistencyManager.Services;
using FileConsistencyManager.DataAccess;

namespace FileConsistencyManager.Business
{
    internal class ConsistencyManager
    {
        private readonly AttachmentRepository _repository;
        private readonly FileService _fileService;
        private readonly ComparisonService _comparisonService;

        public ConsistencyManager(
            AttachmentRepository repository,
            FileService fileService,
            ComparisonService comparisonService)
        {
            _repository = repository;
            _fileService = fileService;
            _comparisonService = comparisonService;
        }

        public List<ComparisonResult> RunCheck(string rootPath)
        {
            try
            {
                // Load database entries
                var attachments = _repository.GetAllAttachments();

                // Load files
                var files = _fileService.GetAllFiles(rootPath);

                // Compare and get results
                var results = _comparisonService.Compare(attachments, files);

                return results;
            }
            catch (Exception ex)
            {
                // use logging framework here
                throw new Exception("Fehler beim Konsistenzcheck: " + ex.Message);
            }
        }
    }
}
