using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INIManagerProject.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace INIManagerProject.src.ViewModel
{
    class OpenDocumentViewModel : ViewModelBase
    {
        private readonly DocumentManager _documentManager;
        public List<KeyValuePair<string, string>> SavedDocuments { get; set; }

        public OpenDocumentViewModel()
        {
            _documentManager = ((App)Application.Current).IniApplication.DocumentManager;
            _documentManager.PopulateSavedDocuments();

            //Only show the savedDocuments that are not already opened.
            SavedDocuments = new List<KeyValuePair<string, string>>(_documentManager.SavedDocuments);
            SavedDocuments.RemoveAll(pair => _documentManager.DocumentList.Any(d=> d.DocumentName == pair.Key));            
        }
    }
}
