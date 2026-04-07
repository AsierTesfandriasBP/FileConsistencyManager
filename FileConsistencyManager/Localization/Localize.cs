using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Localization
{
    internal class Localize
    {
        private string currentLanguage;

        public Localize(string currentLanguage)
        {
            this.currentLanguage = currentLanguage;
        }

        public string GetCurrentLanguage()
        {
            return currentLanguage;
        }

        public void SetCurrentLanguage(string language)
        {
            currentLanguage = language;
        }

        public string GetContent(string key, string language)
        {
            switch (language)
            {
                case "en":
                    return en.ContainsKey(key) ? en[key] : key;
                case "de":
                    return de.ContainsKey(key) ? de[key] : key;
                default:
                    // Fallback: Return the key if no translation is found
                    return key;
            }
        }

        public Dictionary<string, string> GetCurrentLanguageDictionary()
        {
            switch (currentLanguage)
            {
                case "en":
                    return en;
                case "de":
                    return de;
                default:
                    // Fallback: Return an empty dictionary if no language is set
                    return new Dictionary<string, string>();
            }
        }

        public Dictionary<string, string> de = new Dictionary<string, string>()
        {
            { "Start", "Scan starten" },
            { "Delete", "Löschen [Dauerhaft]" },
            { "Ignore", "Ignorieren" },
            { "Archive", "Archivieren"},
            { "Culture", "Deutsch" },
            { "DeleteActionText", "delete" },
            { "IgnoreActionText", "ignore" },
            { "ArchiveActionText", "archive" },
            { "FilterAll", "Alle" },
            { "FilterMissingFiles", "Fehlende Dateien"},
            { "FilterOrphanFiles", "Verwaiste Dateien" },
            { "FilterOptionsTitle", "Filter:" },
            { "FilterLanguageTitle", "Sprache:" },
            { "ConfirmTitle", "Bestätigung" },
            { "AfterItemCountMessage", "Bitte wählen Sie mindestens einen Eintrag aus." },
            { "ActionCompleteMessage", "Aktion abgeschlossen." },
            { "ProgressBarAnalyseStartMessage", "Analyse läuft..." },
            { "ProgressBarAnalyseConnectionMessage", "Verbindung zur MSSQL-Datenbank wird aufgebaut..." },
            { "ProgressBarAnalyseDoneMessage", "{0} Inkonsistenzen gefunden!" },
            { "ProgressBarAnalyseStatusMessage", "Status: {0}%" },
            { "DeleteButtonToolTip", "Löscht die ausgewählten Dateien/Datenbank Einträge" },
            { "ArchiveButtonToolTip", "Verschiebt Dateien ins Archiv" },
            { "IgnoreButtonToolTip", "Markiert Einträge als ignoriert" },
            { "MissingFilesLabel", "Fehlende Dateien:"},
            { "OrphanFilesLabel", "Verwaiste Dateien:"},
        };

        public Dictionary<string, string> en = new Dictionary<string, string>()
        {
            { "Start", "Start Scan" },
            { "Delete", "Delete [Permanent]" },
            { "Ignore", "Ignore" },
            { "Archive", "Archive" },
            { "Culture", "English" },
            { "DeleteActionText", "delete" },
            { "IgnoreActionText", "ignore" },
            { "ArchiveActionText", "archive" },
            { "FilterAll", "All" },
            { "FilterMissingFiles", "Missing Files"},
            { "FilterOrphanFiles", "Orphan Files" },
            { "FilterOptionsTitle", "Filter:" },
            { "FilterLanguageTitle", "Language:" },
            { "ConfirmTitle", "Confirmation" },
            { "AfterItemCountMessage", "Please pick atleast one Entry." },
            { "ActionCompleteMessage", "Action complete." },
            { "ProgressBarAnalyseStartMessage", "Analysis starting..." },
            { "ProgressBarAnalyseConnectionMessage", "Trying to connect to MSSQL-Database..." },
            { "ProgressBarAnalyseDoneMessage", "{0} Inconsistencies found!" },
            { "ProgressBarAnalyseStatusMessage", "Status: {0}%" },
            { "DeleteButtonToolTip", "Deletes all selected Files/Database Entries" },
            { "ArchiveButtonToolTip", "Moves the Files to the archive" },
            { "IgnoreButtonToolTip", "Marks all selected entries as ignored" },
            { "MissingFilesLabel", "Missing Files:"},
            { "OrphanFilesLabel", "Orphan Files:"},
        };
    }
}
