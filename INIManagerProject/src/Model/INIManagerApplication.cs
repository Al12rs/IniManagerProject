using IniParser.Model;
using IniParser;
using IniParser.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Point of reference for the application.
    /// Instatiates DocumentManager.
    /// Obtainable from anywhere with:
    /// ((App)Application.Current).IniApplication
    /// </summary>
    class INIManagerApplication
    {
        #region Properties

        internal string ApplicationAppdataFolder { get; set; }
        internal string ApplicationSettingsFilePath { get; private set; }
        internal IniData ParsedApplicationSettings { get; private set; }
        internal DocumentManager DocumentManager { get; private set; }

        #endregion Properties

        #region Initialization

        public INIManagerApplication()
        {
            var appdataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ApplicationAppdataFolder = Path.Combine(appdataLocal, "INIManager");
            if (!Directory.Exists(ApplicationAppdataFolder))
            {
                Directory.CreateDirectory(ApplicationAppdataFolder);
            }
            ApplicationSettingsFilePath = Path.Combine(ApplicationAppdataFolder, "INIManagerSettings.ini");
        }

        public void start()
        {
            LoadApplicationSettings();
            DocumentManager = new DocumentManager();     
            DocumentManager.LoadActiveDocumentsFromDisk();
            // TODO: set the document currently in focus in the view.
        }

        #endregion Initialization

        #region PrivateMethods

        /// <summary>
        /// Reads INIManagerSettings.ini and populates ParsedApplicationSettings
        /// </summary>
        private void LoadApplicationSettings()
        {
            var parser = new FileIniDataParser();
            if (!File.Exists(ApplicationSettingsFilePath))
            {
                File.Create(ApplicationSettingsFilePath).Dispose();
            }
            ParsedApplicationSettings = parser.ReadFile(ApplicationSettingsFilePath);
        }

        #endregion PrivateMethods

    }
}
