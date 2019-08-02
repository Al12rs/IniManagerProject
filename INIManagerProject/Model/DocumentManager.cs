using INIManagerProject.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    class DocumentManager
    {
        private List<Document> _documentsList;
        private IdBroker _idBroker;

        public DocumentManager()
        {
            _documentsList = new List<Document>();
            _idBroker = new IdBroker();
        }

        internal List<Document> DocumentList { get => _documentsList; }

        internal Document CreateNewDocument(String filePath)
        {
            Document newDocument = new Document(_idBroker.NextId, filePath);
            newDocument.InitializeNewDocument();
            _documentsList.Add(newDocument);
            return newDocument;
        }

        internal Document GetDocumentById(int documentId)
        {
            Document currentDocument = null;
            for (int i = 0; i < _documentsList.Count; i++)
            {
                if(_documentsList[i].DocumentId == documentId)
                {
                    currentDocument = _documentsList[i];
                    break;
                }
            }
            return currentDocument;
        }
    }
}
