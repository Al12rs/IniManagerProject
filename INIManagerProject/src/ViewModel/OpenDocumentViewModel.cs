using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INIManagerProject.Model;
using System.Collections.ObjectModel;

namespace INIManagerProject.src.ViewModel
{
    class OpenDocumentViewModel : ViewModelBase
    {
        private DocumentManager _documentManager;
        public List<KeyValuePair<string, string>> SavedDocuments { get; set; }

        public OpenDocumentViewModel(DocumentManager documentManager)
        {
            _documentManager = documentManager;
            _documentManager.PopulateSavedDocuments();
            SavedDocuments = _documentManager.SavedDocuments;
            //SavedDocuments = new List<KeyValuePair<string, string>>();
            //SavedDocuments.Add(new KeyValuePair<string, string>("Alex", "boss"));
        }


    }
}
