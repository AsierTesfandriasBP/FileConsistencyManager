using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Localization
{
    public class Localize
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
            // Buttons
            { "Start", "Scan starten" },
            { "Delete", "Löschen [Dauerhaft]" },
            { "Ignore", "Ignorieren" },
            { "Archive", "Archivieren"},
            // Filter Options
            { "Culture", "Deutsch" },
            { "FilterAll", "Alle" },
            { "FilterMissingFiles", "Fehlende Dateien"},
            { "FilterOrphanFiles", "Verwaiste Dateien" },
            { "FilterExistsTitle", "Vorhandene Dateien" },
            // Filter Labels
            { "FilterOptionsTitle", "Filter:" },
            { "FilterLanguageTitle", "Sprache:" },
            // MessageBox Messages & Words to insert into Messages
            { "DeleteActionText", "löschen" },
            { "IgnoreActionText", "ignorieren" },
            { "ArchiveActionText", "archivieren" },
            { "ConfirmMultipleMessage", "Möchten Sie wirklich {1} Einträge {0}?"},
            { "ConfirmMessage", "Möchten Sie diesen Eintrag {0}?" },
            { "AfterItemCountMessage", "Bitte wählen Sie mindestens einen Eintrag aus." },
            { "ActionCompleteMessage", "Aktion abgeschlossen." },
            // ProgressBar Labels
            { "ProgressBarAnalyseStartMessage", "Analyse läuft..." },
            { "ProgressBarAnalyseConnectionMessage", "Verbindung zur MSSQL-Datenbank wird aufgebaut..." },
            { "ProgressBarAnalyseDoneMessage", "Einträge gefunden!" },
            { "ProgressBarAnalyseStatusMessage", "Status: {0}%" },
            // Tooltips
            { "DeleteButtonToolTip", "Löscht die ausgewählten Dateien/Datenbank Einträge" },
            { "ArchiveButtonToolTip", "Verschiebt Dateien ins Archiv" },
            { "IgnoreButtonToolTip", "Markiert Einträge als ignoriert" },
            // Labels
            { "MissingFilesLabel", "Dateien in der Datenbank"},
            { "OrphanFilesLabel", "Dateien im Dateipfad"},
            // DataGridView Headers
            { "DGVHeaderFile", "Datei" },
            { "DGVHeaderPath", "Quelle" },
            { "DGVHeaderType", "Typ" },
            // MessageBox Titles
            { "CustomTextInformation", "Information" },
            { "CustomTextWarning", "Warnung" },
            { "CustomTextError", "Fehler" },
            // Config
            { "ConfigNotFoundErrorMessage", "Konfigurationsdatei '{0}' nicht gefunden." },
            { "ConfigLoadErrorMessage", "Laden der Konfigurationsdatei '{0}' fehlgeschlagen. Überprüfen Sie das Dateiformat und den Inhalt." },
            { "ConfigDeserializeErrorMessage", "Fehler beim Deserialisieren der Konfigurationsdatei '{0}'. Bitte überprüfen Sie das Dateiformat und den Inhalt." },
            { "ConfigParseErrorMessage", "Fehler beim Parsen der Konfigurationsdatei '{0}'. Bitte überprüfen Sie das Dateiformat und den Inhalt." },
            { "ConfigUnexpectedErrorMessage", "Unerwarteter Fehler beim Laden der Konfigurationsdatei '{0}'. Bitte überprüfen Sie die Datei und versuchen Sie es erneut." },
            { "ConfigSaveErrorMessage", "Fehler beim Schreiben der Konfigurationsdatei '{0}'. Bitte überprüfen Sie das Dateiformat und den Inhalt." },
            { "ConfigSuccessfulSaveMessage", "Konfigurationsdatei '{0}' erfolgreich gespeichert." },
        };

        public Dictionary<string, string> en = new Dictionary<string, string>()
        {
            // Buttons
            { "Start", "Start Scan" },
            { "Delete", "Delete [Permanent]" },
            { "Ignore", "Ignore" },
            { "Archive", "Archive" },
            // Filter Options
            { "Culture", "English" },
            { "FilterAll", "All" },
            { "FilterMissingFiles", "Missing Files"},
            { "FilterOrphanFiles", "Orphan Files" },
            { "FilterExistsTitle", "Existing Entries" },
            // Filter Labels
            { "FilterOptionsTitle", "Filter:" },
            { "FilterLanguageTitle", "Language:" },
            // MessageBox Messages & Words to insert into Messages
            { "DeleteActionText", "deleted" },
            { "IgnoreActionText", "ignored" },
            { "ArchiveActionText", "archived" },
            { "ConfirmMultipleMessage", "Are you sure you want {0} entries to be {1}?"},
            { "ConfirmMessage", "Are you sure you want to {0} this entry?" },
            { "AfterItemCountMessage", "Please pick atleast one Entry." },
            { "ActionCompleteMessage", "Action complete." },
            // ProgressBar Labels
            { "ProgressBarAnalyseStartMessage", "Analysis starting..." },
            { "ProgressBarAnalyseConnectionMessage", "Trying to connect to MSSQL-Database..." },
            { "ProgressBarAnalyseDoneMessage", "entries found!" },
            { "ProgressBarAnalyseStatusMessage", "Status: {0}%" },
            // Tooltips
            { "DeleteButtonToolTip", "Deletes all selected Files/Database Entries" },
            { "ArchiveButtonToolTip", "Moves the Files to the archive" },
            { "IgnoreButtonToolTip", "Marks all selected entries as ignored" },
            // Labels
            { "MissingFilesLabel", "Files in Database"},
            { "OrphanFilesLabel", "Files in Filepath"},
            // DataGridView Headers
            { "DGVHeaderFile", "File" },
            { "DGVHeaderPath", "Source" },
            { "DGVHeaderType", "Type" },
            // MessageBox Titles
            { "CustomTextInformation", "Information" },
            { "CustomTextWarning", "Warning" },
            { "CustomTextError", "Error" },
            // Config
            { "ConfigNotFoundErrorMessage", "Configuration file '{0}' not found." },
            { "ConfigLoadErrorMessage", "Failed to load configuration file '{0}'. Please check the file format and content." },
            { "ConfigDeserializeErrorMessage", "Failed to deserialize configuration file '{0}'. Please check the file format and content." },
            { "ConfigParseErrorMessage", "Failed to parse configuration file '{0}'. Please check the file format and content." },
            { "ConfigUnexpectedErrorMessage", "Unexpected error loading configuration file '{0}'. Please check the file and try again." },
            { "ConfigSaveErrorMessage", "Error writing configuration to file '{0}'. Please check the file format and content." },
            { "ConfigSuccessfulSaveMessage", "Configuration file '{0}' saved successfully." },
        };
    }
}
