using INIManagerProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace INIManagerProject.src.ViewModel
{  
    class MainWindowViewModel : ViewModelBase
    {
        private DocumentManager documentManager;
        private DelegateCommand _closeDocument;
        public ICommand CloseDocumentCommand => _closeDocument;

        public MainWindowViewModel()
        {
            DocumentManager = ((App)Application.Current).IniApplication.DocumentManager;
            _closeDocument = new DelegateCommand(OnCloseDocument);
        }
        
        private void OnCloseDocument(object commandParameter){
            
            var docToClose = (Document)commandParameter;
            docToClose.Persist();
            DocumentManager.DocumentList.Remove(docToClose);
        }

        public DocumentManager DocumentManager { get => documentManager; set => documentManager = value; }
    }
}
