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
    public class Document
    {
        #region Properties

        public int DocumentId { get; private set; }
        public ManagedFile ManagedFile { get; set; }
        public string DocumentName { get; set; }
        public string DocumentFolderPath { get; set; }
        public string DocumentSettingsFilePath { get; private set; }
        public IniData ParsedDocumentSettings { get; private set; }
        public ProfileManager ProfileManager { get; private set; }
        public MergeStructure MergeTree { get; private set; }
        public EditListModel EditListModel { get; private set; }

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
            ManagedFile = new ManagedFile();
        }

        /// <summary>
        /// Populates an empty document given the path to the file to manage.
        /// Will also generate the folder structure needed for the Document.
        /// </summary>
        /// <param name="filePath"></param>
        public void CreateNewFromIniFilePath(string filePath)
        {
            ManagedFile.ManagedFilePath = filePath;
            ManagedFile.Initialize();
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

            MergeTree = new MergeStructure(this);

            ProfileManager = new ProfileManager(this);
            EditListModel = new EditListModel(this);
            EditListModel.LoadEditsFromDisk();
            ProfileManager.LoadProfilesFromDisk();
            EditListModel.ApplyProfile(ProfileManager.CurrentProfile);
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
            ManagedFile.ManagedFilePath = filePath;
            ManagedFile.Initialize();

            MergeTree = new MergeStructure(this);


            ProfileManager = new ProfileManager(this);
            EditListModel = new EditListModel(this);
            ProfileManager.LoadProfilesFromDisk();
            EditListModel.LoadEditsFromDisk();
            EditListModel.ApplyProfile(ProfileManager.CurrentProfile);
            return true;
        }

        /// <summary>
        /// Handle persisting of managedFilePath as well as telling Profiles and EditLists to persist.
        /// writes DocumentSettings.ini to disk.
        /// </summary>
        public void Persist()
        {
            ParsedDocumentSettings["General"]["managedFilePath"] = ManagedFile.ManagedFilePath;
            ProfileManager.Persist();
            EditListModel.Persist();
            var parser = new FileIniDataParser();
            parser.WriteFile(DocumentSettingsFilePath, ParsedDocumentSettings);
        }

        #endregion Initialization

        #region PublicMethods

        /// <summary>
        /// This is the only function to use to change the current Profile, 
        /// it will make the previos profile Persist and apply the new profile 
        /// to the Edit List.
        /// </summary>
        /// <param name="newCurrentProfile"></param>
        public void SetCurrentProfile(Profile newCurrentProfile)
        {
            ProfileManager.CurrentProfile = newCurrentProfile;
            EditListModel.ApplyProfile(newCurrentProfile);
        }

        #endregion

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