using INIManagerProject.Model.Utils;
using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Linq;
using System.ComponentModel;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Class containing the list of open Documents (Managed ini files) and functionality to manage them.
    /// Singleton, there is just one manager for iniApplication, there
    /// can be multiple documents on the other hand.
    /// </summary>
    public class DocumentManager : ViewModelBase
    {

        #region Events


        #endregion

        #region Fields

        private Document _currentDocument;

        #endregion

        #region Properties

        public ObservableCollection<Document> DocumentList { get; private set; }
        public Document CurrentDocument
        {
            get
            {
                return _currentDocument;
            }
            set
            {
                _currentDocument = value;
                OnPropertyChanged("CurrentDocument");
            }
        }
        public IdBroker IdBroker { get; private set; }
        public string DocumentsFolderPath { get; private set; }
        public List<KeyValuePair<string, string>> SavedDocuments { get; private set; }

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Instantiates DocumentList, IdBroker.
        /// Populates DocumentsFolder.
        /// Creates a new folder if DocumentFolder does not exist.
        /// </summary>
        public DocumentManager()
        {
            DocumentList = new ObservableCollection<Document>();
            DocumentList.DefaultIfEmpty();
            IdBroker = new IdBroker();
            var appAppdataFolder = ((App)Application.Current)
                .IniApplication.ApplicationAppdataFolder;
            DocumentsFolderPath = Path.Combine(appAppdataFolder, "Documents");
            if (!Directory.Exists(DocumentsFolderPath))
            {
                Directory.CreateDirectory(DocumentsFolderPath);
            }
            SavedDocuments = new List<KeyValuePair<string, string>>();
            PopulateSavedDocuments();
        }


        #endregion Initialization

        #region PublicMethods

        /// <summary>
        /// Generates a new Document given <paramref name="filePath"/> of an INI file to manage,
        /// and adds it to the list.
        /// </summary>
        /// <param name="filePath">Path to the ini file to manage.</param>
        /// <returns>Generated document.</returns>
        public Document CreateNewDocument(String filePath)
        {
            Document newDocument = new Document(IdBroker.NextId);
            newDocument.CreateNewFromIniFilePath(filePath);
            DocumentList.Add(newDocument);
            return newDocument;
        }

        /// <summary>
        /// Instantiates and loads Documents that were open from last session.
        /// Might not load anything if no documents were open.
        /// </summary>
        public void LoadActiveDocumentsFromDisk()
        {
            string loadedDocuments = ((App)Application.Current)
                .IniApplication.ParsedApplicationSettings["General"]["loadedDocuments"];
            if (loadedDocuments != null)
            {
                var loadedDocumentsList = loadedDocuments.Split(',');
                if (loadedDocumentsList == null || loadedDocumentsList.Length < 1)
                {
                    // Don't load any documents.
                }
                else
                {
                    foreach (var docName in loadedDocumentsList)
                    {
                        CreateAndLoadDocumentFromName(docName);
                    }
                }
            }
            string lastDocumentWithFocus = ((App)Application.Current)
                .IniApplication.ParsedApplicationSettings["General"]["selectedDocument"];
            if (lastDocumentWithFocus != null && DocumentList.Any(d => d.DocumentName == lastDocumentWithFocus))
            {
                CurrentDocument = DocumentList.Single(d => d.DocumentName == lastDocumentWithFocus);
            } else
            {
                if (DocumentList.Count > 0)
                {
                    CurrentDocument = DocumentList[DocumentList.Count - 1];
                }
            }
        }

        /// <summary>
        /// Instantiates and loads a Document from disk given the <paramref name="docName"/>.
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns>Document created or null if failed to load.</returns>
        public Document CreateAndLoadDocumentFromName(string docName)
        {
            Document loadedDocument = new Document(IdBroker.NextId);
            if (loadedDocument.LoadFromDisk(docName))
            {
                DocumentList.Add(loadedDocument);
                return loadedDocument;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Handles persisting of list of open documents as well as calling persist on all of them.
        /// </summary>
        public void Persist()
        {
            StringBuilder openDocuments = new StringBuilder();
            foreach (var doc in DocumentList)
            {
                doc.Persist();
            }
            foreach (var docName in DocumentList.GroupBy(d => d.DocumentName).Select(e => e.Key))
            {
                openDocuments.Append(docName + ",");
            }
            ((App)Application.Current)
                .IniApplication.ParsedApplicationSettings["General"]["loadedDocuments"] = openDocuments.ToString();
            if(CurrentDocument!= null)
            {
                ((App)Application.Current)
                .IniApplication.ParsedApplicationSettings["General"]["selectedDocument"] = CurrentDocument.DocumentName;
            }
            
        }

        /// <summary>
        /// Obtain document from a specific DocumentId.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns>Matching document or null if not found.</returns>
        public Document GetDocumentById(int documentId)
        {
            Document currentDocument = null;
            for (int i = 0; i < DocumentList.Count; i++)
            {
                if (DocumentList[i].DocumentId == documentId)
                {
                    currentDocument = DocumentList[i];
                    break;
                }
            }
            return currentDocument;
        }

        /// <summary>
        /// Populates the SavedDocuments.
        /// Clears the previous contents.
        /// Iterates on the DocumentFolders and reads the managed files.
        /// </summary>
        public void PopulateSavedDocuments()
        {
            SavedDocuments.Clear();
            var docDirs = Directory.GetDirectories(DocumentsFolderPath);
            var parser = new FileIniDataParser();
            IniData iniData;
            foreach (var dir in docDirs)
            {
                var settingsPath = Path.Combine(dir, "DocumentSettings.ini");
                if (File.Exists(settingsPath))
                {
                    iniData = parser.ReadFile(settingsPath);
                    if (iniData != null && iniData["General"]["managedFilePath"] != null && iniData["General"]["managedFilePath"] != "")
                    {
                        SavedDocuments.Add(new KeyValuePair<string, string>(Path.GetFileName(dir), iniData["General"]["managedFilePath"]));
                    }
                }
            }
        }

        public Document CalculateNewCurrentDocument()
        {
            var currentIndex = DocumentList.IndexOf(CurrentDocument);
            var docListCount = DocumentList.Count;
            if (currentIndex > 0)
            {
                // Set the tab to the first one if available.
                CurrentDocument = DocumentList[0];
            } else if (docListCount > 1)
            {
                // We are on the leftMost tab already but there are other tabs so take
                // the first to the right.
                CurrentDocument = DocumentList[1];
            } else
            {
                // The current tab was the last on the list.
                CurrentDocument = null;
            }
            return CurrentDocument;
        }

        #endregion PublicMethods

        #region PrivateMethods


        #endregion
    }
}