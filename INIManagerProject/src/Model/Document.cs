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
        public Document(int docId, String filePath)
        {
            DocumentId = docId;
            ManagedFilePath = filePath;
            //TODO create unique document name
            DocumentName = Path.GetFileNameWithoutExtension(filePath);
            var documentsFolder = ((App)Application.Current).IniApplication.DocumentManager.DocumentsFolder;
            DocumentFolder = Path.Combine(documentsFolder, DocumentName);
            if (!Directory.Exists(DocumentFolder))
            {
                Directory.CreateDirectory(DocumentFolder);
            }

        }

        public void InitializeNewDocument()
        {
            //these might need document to be already created so the constructor needs to be concluded
            ProfileManager = new ProfileManager(this);
            EditListModel = new EditListModel(this);
            ProfileManager.initializeNewProfileMananger();
            EditListModel.initializeNewEditListModel();
            ProfileManager.CurrentProfile.ValidateAndUpdateProfileEdits();
        }

        public void LoadFromDisk()
        {
            ProfileManager = new ProfileManager(this);
            EditListModel = new EditListModel(this);
            EditListModel.LoadEditsFromDisk();
            ProfileManager.LoadFromDisk();
        }

        public string ManagedFilePath { get; set; }
        public int DocumentId { get; private set; }
        public string DocumentFolder { get; set; }
        public string DocumentName { get; set; }
        internal ProfileManager ProfileManager { get; private set; }
        internal MergeStructure MergeTree { get; private set; }
        internal EditListModel EditListModel { get; private set; }
    }
}
