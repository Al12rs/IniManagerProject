using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace INIManagerProject.Model
{
    class Document
    {
        private int _documentId;
        private ProfileManager _profileManager;
        private MergeStructure _mergeTree;
        private String _managedFilePath;
        private String _documentName;
        private EditListModel _editListModel;
        private String _documentFolder;

        public Document(int docId, String filePath)
        {
            _documentId = docId;
            ManagedFilePath = filePath;
            //TODO create unique document name
            DocumentName = Path.GetFileName(filePath);
            var appAppdataFolder = ((App)Application.Current).IniApplication.ApplicationAppdataFolder;
            DocumentFolder = Path.Combine(appAppdataFolder, DocumentName);
            if (!Directory.Exists(DocumentFolder))
            {
                Directory.CreateDirectory(DocumentFolder);
            }

        }

        public void InitializeNewDocument()
        {
            //these might need document to be already created so the constructor needs to be concluded
            _profileManager = new ProfileManager(this);
            _profileManager.initializeNewProfile();
            _editListModel = new EditListModel(this);
        }

        public string ManagedFilePath { get => _managedFilePath; set => _managedFilePath = value;  }
        public int DocumentId { get => _documentId; }
        public string DocumentFolder { get => _documentFolder; set => _documentFolder = value; }
        public string DocumentName { get => _documentName; set => _documentName = value; }
        internal ProfileManager ProfileManager { get => _profileManager; }
        internal MergeStructure MergeTree { get => _mergeTree; }
        internal EditListModel EditListModel { get => _editListModel; }
    }
}
