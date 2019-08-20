using INIManagerProject.View;
using INIManagerProject.Model;
using INIManagerProject.ViewModel.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace INIManagerProject.ViewModel
{  
    class MainWindowViewModel : ViewModelBase
    {
        private DocumentManager documentManager;
        private readonly DelegateCommand _closeDocument;
        public ICommand CloseDocumentCommand => _closeDocument;
        private ObservableViewModelCollection<DocumentViewModel, Document> _documentViewModelList;

        public MainWindowViewModel()
        {
            DocumentManager = ((App)Application.Current).IniApplication.DocumentManager;
            Func<Document, DocumentViewModel> viewModelCreator = model => new DocumentViewModel(model);
            _documentViewModelList = new ObservableViewModelCollection<DocumentViewModel
                , Document>(DocumentManager.DocumentList, viewModelCreator);
            _closeDocument = new DelegateCommand(OnCloseDocument);
            DocumentManager.PropertyChanged += PropertyChangedHandler;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrentDocument":
                    OnPropertyChanged("CurrentDocumentViewModel");
                    break;
            }
        }

        private void OnCloseDocument(object commandParameter){

            var docVM = (commandParameter as DocumentViewModel);
            var docToClose = docVM.Document;
            docToClose.Persist();
            if(DocumentManager.CurrentDocument == docToClose)
            {
                DocumentManager.CalculateNewCurrentDocument();
            }
            DocumentManager.DocumentList.Remove(docToClose);

        }

        public ObservableViewModelCollection<DocumentViewModel, Document> DocumentViewModelList
        {
            get => _documentViewModelList;
            private set => _documentViewModelList = value;
        }
        public DocumentManager DocumentManager { get => documentManager; set => documentManager = value; }
        public DocumentViewModel CurrentDocumentViewModel
        {
            get
            {
                return _documentViewModelList.SingleOrDefault(docVM => docVM.Document == DocumentManager.CurrentDocument);
            }
            set
            {
                if(value != null)
                {
                    DocumentManager.CurrentDocument = value.Document;
                }
            }
        }
    }
}
