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

        public Dictionary<string, string> de = new Dictionary<string, string>()
        {
            // Buttons
            { "Start", "Scan starten" },
            { "Delete", "Löschen [Dauerhaft]" },
            { "Archive", "Archivieren"},
            { "Settings", "Config Einstellungen" },
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
            { "ArchiveActionText", "archivieren" },
            { "ConfirmMultipleMessage", "Möchten Sie wirklich {1} Einträge {0}?"},
            { "ConfirmMessage", "Möchten Sie diesen Eintrag {0}?" },
            { "AfterItemCountMessage", "Bitte wählen Sie mindestens einen Eintrag aus." },
            { "ActionCompleteMessage", "Aktion abgeschlossen." },
            { "ExistsInformationMessage", "Alle 'Exists'-Typen werden ignoriert. " },
            { "ExistsAndMissingInformationMessage", "Alle 'Exists'- und 'MissingFile'-Typen werden ignoriert. " },
            // ProgressBar Labels
            { "ProgressBarAnalyseStartMessage", "Analyse läuft..." },
            { "ProgressBarAnalyseConnectionDatabaseMessage", "Verbindung zur MSSQL-Datenbank wird aufgebaut..." },
            { "ProgressBarAnalyseConnectionFilepathMessage", "Verbindung zum Attachments-Dateipfad wird aufgebaut..." },
            { "ProgressBarAnalyseDoneMessage", "Erfolgreich!" },
            { "ProgressBarAnalyseStatusMessage", "Status: {0}%" },
            // Tooltips
            { "DeleteButtonToolTip", "Löscht die ausgewählten Dateien/Datenbank Einträge" },
            { "ArchiveButtonToolTip", "Verschiebt Dateien ins Archiv" },
            { "SettingsButtonToolTip", "Öffnet eine Forms mit allen Einstellungen" },
            // Labels
            { "MissingFilesLabel", "Einträge in der Datenbank"},
            { "OrphanFilesLabel", "Dateien im Dateipfad"},
            { "ExistsFilesLabel", "Einträge/Dateien in Datenbank und Dateipfad"},
            { "ConnectedToLabel", "Verbunden mit: {0}" },
            { "EntriesFoundLabel", "Gefundene Einträge (insgesamt):" },
            // DataGridView Headers
            { "DGVHeaderFile", "Datei" },
            { "DGVHeaderPath", "Quelle" },
            { "DGVHeaderType", "Typ" },
            // MessageBox Titles
            { "CustomTextInformation", "Information" },
            { "CustomTextWarning", "Warnung" },
            { "CustomTextError", "Fehler" },
            { "CustomTextOK", "OK" },
            { "CustomTextCancel", "Abbrechen" },
            // Config
            { "ConfigNotFoundErrorMessage", "Konfigurationsdatei '{0}' nicht gefunden." },
            { "ConfigLoadErrorMessage", "Laden der Konfigurationsdatei '{0}' fehlgeschlagen. Überprüfen Sie das Dateiformat und den Inhalt." },
            { "ConfigDeserializeErrorMessage", "Fehler beim Deserialisieren der Konfigurationsdatei '{0}'. Bitte überprüfen Sie das Dateiformat und den Inhalt." },
            { "ConfigParseErrorMessage", "Fehler beim Parsen der Konfigurationsdatei '{0}'. Bitte überprüfen Sie das Dateiformat und den Inhalt." },
            { "ConfigUnexpectedErrorMessage", "Unerwarteter Fehler beim Laden der Konfigurationsdatei '{0}'. Bitte überprüfen Sie die Datei und versuchen Sie es erneut." },
            { "ConfigSaveErrorMessage", "Fehler beim Schreiben der Konfigurationsdatei '{0}'. Bitte überprüfen Sie das Dateiformat und den Inhalt." },
            { "ConfigSuccessfulSaveMessage", "Konfigurationsdatei '{0}' erfolgreich gespeichert." },
            // Settings Page
            { "SettingsDatabaseConnectionTitle", "Standard Datenbank Verbindung" },
            { "SettingsServerLabel", "Server:" },
            { "SettingsDatabaseLabel", "Datenbank:" },
            { "SettingsUserIdLabel", "Benutzer/UserId:" },
            { "SettingsPasswordLabel", "Passwort:" },
            { "SettingsPathTitle", "Standard Pfad" },
            { "SettingsArchivePathLabel", "Archiv-Pfad:" },
            { "SettingsLanguageTitle", "Standard Sprache" },
            { "SettingsLanguageLabel", "Sprache:" },
            { "SettingsTestConnectionButton", "Verbindung testen" },
            { "SettingsSaveButton", "Einstellungen speichern"  },
            // Settings Page Messages
            { "SettingsSavedAfterLoadFromMain", "Starten Sie die Anwendung neu, um sicherzustellen, dass die Einstellungen übernommen werden." },
            { "ValidationAllFieldsErrorMessage", "Bitte füllen Sie alle erforderlichen Felder aus: Server, Datenbank, Benutzer/UserId, Passwort, Archivpfad und Sprache." },
            { "ValidationDatabaseFieldsErrorMessage", "Bitte füllen Sie alle erforderlichen Felder aus: Server, Datenbank, Benutzer/UserId, Passwort." },
            { "SettingsTestConnectionSuccessMessage", "Verbindung erfolgreich!" },
            { "SettingsTestConnectionFailedMessage", "Verbindung fehlgeschlagen. Bitte überprüfen Sie Ihre Einstellungen und versuchen Sie es erneut." },
        };

        public Dictionary<string, string> en = new Dictionary<string, string>()
        {
            // Buttons
            { "Start", "Start Scan" },
            { "Delete", "Delete [Permanent]" },
            { "Archive", "Archive" },
            { "Settings", "Config Settings" },
            // Filter Options
            { "Culture", "English" },
            { "FilterAll", "All" },
            { "FilterMissingFiles", "Missing Files"},
            { "FilterOrphanFiles", "Orphan Files" },
            { "FilterExistsTitle", "Existing Entries" },
            // Filter Labels
            { "FilterOptionsTitle", "FilterTest:" },
            { "FilterLanguageTitle", "Language:" },
            // MessageBox Messages & Words to insert into Messages
            { "DeleteActionText", "deleted" },
            { "ArchiveActionText", "archived" },
            { "ConfirmMultipleMessage", "Are you sure you want {1} entries to be {0}?"},
            { "ConfirmMessage", "Are you sure you want this entry to be {0}?" },
            { "AfterItemCountMessage", "Please pick atleast one Entry." },
            { "ActionCompleteMessage", "Action complete." },
            { "ExistsInformationMessage", "All 'Exists'-Types will be ignored. " },
            { "ExistsAndMissingInformationMessage", "All 'Exists'- and 'MissingFile'-Types will be ignored. " },
            // ProgressBar Labels
            { "ProgressBarAnalyseStartMessage", "Analysis starting..." },
            { "ProgressBarAnalyseConnectionDatabaseMessage", "Trying to connect to MSSQL-Database..." },
            { "ProgressBarAnalyseConnectionFilepathMessage", "Trying to connect to Attachments Filepath..." },
            { "ProgressBarAnalyseDoneMessage", "Successfull!" },
            { "ProgressBarAnalyseStatusMessage", "Status: {0}%" },
            // Tooltips
            { "DeleteButtonToolTip", "Deletes all selected Files/Database Entries" },
            { "ArchiveButtonToolTip", "Moves the Files to the archive" },
            { "SettingsButtonToolTip", "Opens a form with all settings" },
            // Labels
            { "MissingFilesLabel", "Entries in Database"},
            { "OrphanFilesLabel", "Files in Filepath"},
            { "ExistsFilesLabel", "Entries/Files in Database and Filepath"},
            { "ConnectedToLabel", "Connected to: {0}" },
            { "EntriesFoundLabel", "Found Entries (in Total):" },
            // DataGridView Headers
            { "DGVHeaderFile", "File" },
            { "DGVHeaderPath", "Source" },
            { "DGVHeaderType", "Type" },
            // MessageBox Titles
            { "CustomTextInformation", "Information" },
            { "CustomTextWarning", "Warning" },
            { "CustomTextError", "Error" },
            { "CustomTextOK", "OK" },
            { "CustomTextCancel", "Cancel" },
            // Config
            { "ConfigNotFoundErrorMessage", "Configuration file '{0}' not found." },
            { "ConfigLoadErrorMessage", "Failed to load configuration file '{0}'. Please check the file format and content." },
            { "ConfigDeserializeErrorMessage", "Failed to deserialize configuration file '{0}'. Please check the file format and content." },
            { "ConfigParseErrorMessage", "Failed to parse configuration file '{0}'. Please check the file format and content." },
            { "ConfigUnexpectedErrorMessage", "Unexpected error loading configuration file '{0}'. Please check the file and try again." },
            { "ConfigSaveErrorMessage", "Error writing configuration to file '{0}'. Please check the file format and content." },
            { "ConfigSuccessfulSaveMessage", "Configuration file '{0}' saved successfully." },
            // Settings Page
            { "SettingsDatabaseConnectionTitle", "Default Database Connection" },
            { "SettingsServerLabel", "Server:" },
            { "SettingsDatabaseLabel", "Database:" },
            { "SettingsUserIdLabel", "User/UserId:" },
            { "SettingsPasswordLabel", "Password:" },
            { "SettingsPathTitle", "Default Path" },
            { "SettingsArchivePathLabel", "Archive Path:" },
            { "SettingsLanguageTitle", "Default Language" },
            { "SettingsLanguageLabel", "Language:" },
            { "SettingsTestConnectionButton", "Test Connection" },
            { "SettingsSaveButton", "Save Settings" },
            // Settings Page Messages
            { "SettingsSavedAfterLoadFromMain", "Please restart the application, to make sure that the settings apply." },
            { "ValidationAllFieldsErrorMessage", "Please fill in all required fields: Server, Database, UserId, Password, Archivepath and Language." },
            { "ValidationDatabaseFieldsErrorMessage", "Please fill in all required fields: Server, Database, UserId, Password." },
            { "SettingsTestConnectionSuccessMessage", "Connection successful!" },
            { "SettingsTestConnectionFailedMessage", "Connection failed. Please check your settings and try again." },
        };
    }
}
