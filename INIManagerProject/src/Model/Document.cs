using IniParser;
using IniParser.Model;
using System.IO;
using System.Windows;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Class modelling a managed INI, reference and base point for Profiles and EditsList.
    /// There can be multiple documents open and loaded at one moment, one will probably have the focus though.
    /// </summary>
    internal class Document
    {
        #region Properties

        public int DocumentId { get; private set; }
        public string ManagedFilePath { get; set; }
        public string DocumentName { get; set; }
        public string DocumentFolderPath { get; set; }
        internal string DocumentSettingsFilePath { get; private set; }
        internal IniData ParsedDocumentSettings { get; private set; }
        internal ProfileManager ProfileManager { get; private set; }
        internal MergeStructure MergeTree { get; private set; }
        internal EditListModel EditListModel { get; private set; }

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Generates an emty doc object.
        /// Needs to be populated by either CreateNewFromIniFilePath(fileToManagePath)
        /// or LoadFromDisk(docName).
        /// </summary>
        /// <param name="docId"></param>
        public Document(int docId)
        {
            DocumentId = docId;
        }

        /// <summary>
        /// Populates an empty document given the path to the file to manage.
        /// Will also generate the folder structure needed for the Document.
        /// </summary>
        /// <param name="filePath"></param>
        public void CreateNewFromIniFilePath(string filePath)
        {
            ManagedFilePath = filePath;
            // TODO: create unique document name.
            DocumentName = Path.GetFileNameWithoutExtension(filePath);
            var documentsFolder = ((App)Application.Current).IniApplication.DocumentManager.DocumentsFolderPath;
            DocumentFolderPath = Path.Combine(documentsFolder, DocumentName);
            if (!Directory.Exists(DocumentFolderPath))
            {
                Directory.CreateDirectory(DocumentFolderPath);
            }
            DocumentSettingsFilePath = Path.Combine(DocumentFolderPath, "DocumentSettings.ini");
            if (!File.Exists(DocumentSettingsFilePath))
            {
                File.Create(DocumentSettingsFilePath).Dispose();
            }
            LoadDocumentSettings();

            ProfileManager = new ProfileManager(this);
            EditListModel = new EditListModel(this);
            EditListModel.LoadEditsFromDisk();
            ProfileManager.LoadProfilesFromDisk();
            ProfileManager.CurrentProfile.ValidateAndUpdatePriorityLists();
        }

        /// <summary>
        /// Populates an empty Document object given the name by loading it's data from disk.
        /// Will fail if the Doc folder is missing or if there is no path to
        /// the managedFile saved in the ini file.
        /// </summary>
        /// <param name="documentName"></param>
        /// <returns>true if success false if failed</returns>
        public bool LoadFromDisk(string documentName)
        {
            DocumentName = documentName;
            var documentsFolder = ((App)Application.Current).IniApplication.DocumentManager.DocumentsFolderPath;
            DocumentFolderPath = Path.Combine(documentsFolder, DocumentName);
            if (!Directory.Exists(DocumentFolderPath))
                return false;
            DocumentSettingsFilePath = Path.Combine(DocumentFolderPath, "DocumentSettings.ini");
            if (!File.Exists(DocumentSettingsFilePath))
                return false;
            LoadDocumentSettings();
            string filePath = ParsedDocumentSettings["General"]["managedFilePath"];
            if (filePath == null || filePath == "")
                return false;
            ManagedFilePath = filePath;

            ProfileManager = new ProfileManager(this);
            EditListModel = new EditListModel(this);
            EditListModel.LoadEditsFromDisk();
            ProfileManager.LoadProfilesFromDisk();
            ProfileManager.CurrentProfile.ValidateAndUpdatePriorityLists();
            return true;
        }

        /// <summary>
        /// Handle persisting of managedFilePath as well as telling Profiles and EditLists to persist.
        /// writes DocumentSettings.ini to disk.
        /// </summary>
        public void Persist()
        {
            ParsedDocumentSettings["General"]["managedFilePath"] = ManagedFilePath;
            ProfileManager.Persist();
            EditListModel.Persist();
            var parser = new FileIniDataParser();
            parser.WriteFile(DocumentSettingsFilePath, ParsedDocumentSettings);
        }

        #endregion Initialization

        #region PrivateMethods

        /// <summary>
        /// Populate ParsedDocumentSettings with contents of DocumentSetttings.ini
        /// </summary>
        private void LoadDocumentSettings()
        {
            var parser = new FileIniDataParser();
            ParsedDocumentSettings = parser.ReadFile(DocumentSettingsFilePath);
        }

        #endregion PrivateMethods
    }
}