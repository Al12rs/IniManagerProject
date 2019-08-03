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
    class DocumentManager
    {
        public DocumentManager()
        {
            DocumentList = new List<Document>();
            IdBroker = new IdBroker();
            var appAppdataFolder = ((App)Application.Current).IniApplication.ApplicationAppdataFolder;
            DocumentsFolder =  Path.Combine(appAppdataFolder, "Documents");
            if (!Directory.Exists(DocumentsFolder))
            {
                Directory.CreateDirectory(DocumentsFolder);
            }
        }

        internal List<Document> DocumentList { get; private set; }
        internal IdBroker IdBroker { get; private set; }
        internal string DocumentsFolder { get; private set; }

        internal Document CreateNewDocument(String filePath)
        {
            Document newDocument = new Document(IdBroker.NextId, filePath);
            newDocument.InitializeNewDocument();
            DocumentList.Add(newDocument);
            return newDocument;
        }

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

        internal Document LoadDocumentFromFolderName(string folderName)
        {
            Document loadedDocument = new Document(IdBroker.NextId, Path.Combine(DocumentsFolder,folderName));
            loadedDocument.LoadFromDisk();
            return loadedDocument;
        }
    }
}
