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
    class INIManagerApplication
    {
        internal string ApplicationAppdataFolder { get; set; }
        internal DocumentManager DocumentManager { get; private set; }
        internal string ApplicationSettingsFilePath { get; private set; }
        internal IniData ParsedApplicationSettings { get; private set; }

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
            DocumentManager = new DocumentManager();
            var parser = new FileIniDataParser();
            if (!File.Exists(ApplicationSettingsFilePath))
            {
                File.Create(ApplicationSettingsFilePath).Dispose();
            }
            //parser.Parser.Configuration.ThrowExceptionsOnError = false;
            ParsedApplicationSettings = parser.ReadFile(ApplicationSettingsFilePath);
            string loadedDocuments = ParsedApplicationSettings["General"]["loadedDocuments"];
            if (loadedDocuments!= null)
            {
                var loadedDocumentsList = loadedDocuments.Split(',');
                foreach(var docName in loadedDocumentsList)
                {
                    //DocumentManager.LoadDocumentFromFolder()
                }
            }


        }

       
    }
}
