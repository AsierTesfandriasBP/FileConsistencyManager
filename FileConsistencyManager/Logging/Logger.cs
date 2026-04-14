using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Logging
{
    public class Logger
    {
        private readonly string _logFilePath;
        private readonly LogLevel _currentLogLevel;

        public Logger(string logFilePath, LogLevel logLevel)
        {
            _logFilePath = logFilePath;
            _currentLogLevel = logLevel;
        }

        public string GetLogFilePath()
        {
            return _logFilePath;
        }

        public void Log(string message, LogLevel level)
        {
            if (level < _currentLogLevel)
                return;

            string logEntry = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} [{level}] {message}";

            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // Logging darf niemals die App crashen!
            }
        }

        public void LogSeparator(string title = "")
        {
            string line = new string('=', 2);

            if (!string.IsNullOrWhiteSpace(title))
            {
                Log($"{line} {title} {line}", LogLevel.Info);
            }
            else
            {
                Log(line, LogLevel.Info);
            }
        }

        public void LogException(Exception ex)
        {
            Log($"Exception: {ex.Message} | StackTrace: {ex.StackTrace}", LogLevel.Error);
        }
    }
}
