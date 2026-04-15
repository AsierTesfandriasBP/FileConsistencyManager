using System;

namespace FileConsistencyManager.Services
{
    // Interface to allow business logic to report progress and status to the UI layer.
    public interface IUiUpdater
    {
        void SetStatus(string message, string resultsCount = "");
        void UpdateProgress(int value, bool showText = true);
    }
}
