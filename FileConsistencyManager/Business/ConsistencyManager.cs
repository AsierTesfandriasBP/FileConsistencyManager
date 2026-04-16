using System;
using System.Text;
using System.Collections.Generic;
using FileConsistencyManager.Models;
using FileConsistencyManager.Services;
using FileConsistencyManager.DataAccess;
using FileConsistencyManager.Logging;
using FileConsistencyManager.Localization;
using System.Runtime.CompilerServices;
using FileConsistencyManager.Config;

namespace FileConsistencyManager.Business
{
    internal class ConsistencyManager
    {
        private readonly AttachmentRepository _repository;
        private readonly FileService _fileService;
        private readonly ComparisonService _comparisonService;
        private readonly Logger _logger;
        private readonly IUiUpdater _uiUpdater;

        public ConsistencyManager( 
            AttachmentRepository repository,
            FileService fileService,
            ComparisonService comparisonService,
            Logger logger,
            IUiUpdater uiUpdater = null)
        {
            _repository = repository;
            _fileService = fileService;
            _comparisonService = comparisonService;
            _logger = logger;
            _uiUpdater = uiUpdater;
        }

        public List<ComparisonResult> RunCheck(string databaseName, Localize lang, ConfigManager configManager, AppConfig config, bool logText=true)
        {
            try
            {
                if (logText)
                {
                    _logger.Log("", LogLevel.Info);
                    _logger.LogSeparator("Starting consistency check");
                }

                // Load database entries
                _uiUpdater?.SetStatus(lang.GetContent("ProgressBarAnalyseConnectionDatabaseMessage", lang.GetCurrentLanguage()));
                _uiUpdater?.UpdateProgress(20, showText: false);

                string connectionStr = configManager.GetConnectionString(config);
                Task<bool> testedConnection = configManager.TestConnection(config, connectionStr);

                var attachments = _repository.GetAllAttachments();
                if (logText) _logger.Log($"Loaded database entries: {attachments.Count}", LogLevel.Info);
                _uiUpdater?.UpdateProgress(40, showText: false);

                _uiUpdater?.SetStatus(lang.GetContent("ProgressBarAnalyseConnectionFilepathMessage", lang.GetCurrentLanguage()));
                _uiUpdater?.UpdateProgress(60, showText: false);
                
                // Load files
                var files = _fileService.GetAllFiles(_repository.GetRootPath());
                if (logText) _logger.Log($"Found files: {files.Count}", LogLevel.Info);
                _uiUpdater?.UpdateProgress(80, showText: false);

                // Compare and get results
                var results = _comparisonService.Compare(attachments, files, databaseName);
                if (logText) _logger.Log($"Found entries: {results.Count}", LogLevel.Info);
                _uiUpdater?.UpdateProgress(100, showText: false);

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
