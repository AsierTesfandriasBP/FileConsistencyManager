using FileConsistencyManager.Config;
using FileConsistencyManager.Localization;
using FileConsistencyManager.Logging;

namespace FileConsistencyManager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            // Default to English until config is read
            var defaultCulture = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

            ApplicationConfiguration.Initialize();

            // Setup basic logger (used during config loading)
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string logFolder = Path.Combine(basePath, "_Logs");
            if (!Directory.Exists(logFolder)) Directory.CreateDirectory(logFolder);
            string logFile = Path.Combine(logFolder, "_log.txt");
            var logger = new Logger(logFile, LogLevel.Info);

            // First, create a temporary localizer (english) to allow ConfigLoader to display messages
            var tempLocalize = new Localize("en");
            var cfgLoader = new ConfigManager("config.json", tempLocalize, logger);

            // Load config (ConfigLoader will decrypt password if possible)
            var appConfig = cfgLoader.Load();

            // Determine language from config (fallback to 'en')
            var configuredLang = appConfig?.Language?.Current;
            if (string.IsNullOrWhiteSpace(configuredLang)) configuredLang = "en";

            // Set culture based on configured language
            var culture = configuredLang == "de" ? new System.Globalization.CultureInfo("de-DE") : new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Recreate localizer and config loader with correct language for messages
            var localize = new Localize(configuredLang);
            cfgLoader = new ConfigManager("config.json", localize, logger);

            // If config is missing or incomplete, show settings form
            bool needsSettings = false;
            if (appConfig == null)
                needsSettings = true;
            else
            {
                var conn = appConfig.Connection;
                if (conn == null || string.IsNullOrWhiteSpace(conn.Server) || string.IsNullOrWhiteSpace(conn.Database) || string.IsNullOrWhiteSpace(conn.UserId))
                    needsSettings = true;
            }

            if (needsSettings)
            {
                using (var settingsForm = new Settings(appConfig, logger, cfgLoader, localize))
                {
                    var dr = settingsForm.ShowDialog();
                    // After settings saved/closed, reload config
                    appConfig = cfgLoader.Load();
                    // Update language if changed
                    configuredLang = appConfig?.Language?.Current ?? "en";
                    localize.SetCurrentLanguage(configuredLang);
                }
            }

            // Start main with prepared dependencies. ConfigLoader will handle encrypted password storage.
            if(appConfig != null)
                Application.Run(new Main(appConfig, logger, cfgLoader, localize));
        }
    }
}