using INIManagerProject.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Class containing the list of open Documents (Managed ini files) and functionality to manage them.
    /// </summary>
    class DocumentManager
    {
        #region Properties
        internal List<Document> DocumentList { get; private set; }
        internal IdBroker IdBroker { get; private set; }
        internal string DocumentsFolderPath { get; private set; }
        #endregion

        #region Initialization
        /// <summary>
        /// Instantiates DocumentList, IdBroker.
        /// Populates DocumentsFolder.
        /// Creates a new folder if DocumentFolder does not exist.
        /// </summary>
        public DocumentManager()
        {
            DocumentList = new List<Document>();
            IdBroker = new IdBroker();
            var appAppdataFolder = ((App)Application.Current)
                .IniApplication.ApplicationAppdataFolder;
            DocumentsFolderPath =  Path.Combine(appAppdataFolder, "Documents");
            if (!Directory.Exists(DocumentsFolderPath))
            {
                Directory.CreateDirectory(DocumentsFolderPath);
            }
        }
        #endregion

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
            // TODO: Potentially set one of the documents as the one with focus.
        }

        /// <summary>
        /// Instantiates and loads a Document from disk given the <paramref name="docName"/>.
        /// Singleton, there is just one manager for iniApplication, there 
        /// can be multiple documents on the other hand.
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
        /// Obtain document from a specific DocumentId.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns>Matching document or null if not found.</returns>
        internal Document GetDocumentById(int documentId)
        {
            Document currentDocument = null;
            for (int i = 0; i < DocumentList.Count; i++)
            {
                if(DocumentList[i].DocumentId == documentId)
                {
                    currentDocument = DocumentList[i];
                    break;
                }
            }
            return currentDocument;
        }
        #endregion
    }
}
