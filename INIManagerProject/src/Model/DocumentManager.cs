using INIManagerProject.util;
using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Linq;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Class containing the list of open Documents (Managed ini files) and functionality to manage them.
    /// Singleton, there is just one manager for iniApplication, there
    /// can be multiple documents on the other hand.
    /// </summary>
    internal class DocumentManager
    {
        #region Properties

        public ObservableCollection<Document> DocumentList { get; private set; }
        public Document CurrentDocument { get; set; }
        internal IdBroker IdBroker { get; private set; }
        internal string DocumentsFolderPath { get; private set; }
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
        internal Document CreateNewDocument(String filePath)
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
        internal void LoadActiveDocumentsFromDisk()
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
        internal Document CreateAndLoadDocumentFromName(string docName)
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
        internal void Persist()
        {
            StringBuilder openDocuments = new StringBuilder();
            foreach (var doc in DocumentList)
            {
                openDocuments.Append(doc.DocumentName + ",");
                doc.Persist();
            }
            ((App)Application.Current)
                .IniApplication.ParsedApplicationSettings["General"]["loadedDocuments"] = openDocuments.ToString();
            // TODO: write parsed application settings to file.
        }

        /// <summary>
        /// Obtain document from a specific DocumentId.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns>Matching document or null if not found.</returns>
        internal Document GetDocumentById(int documentId)
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
        internal void PopulateSavedDocuments()
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

        #endregion PublicMethods

        #region PrivateMethods



        #endregion
    }
}