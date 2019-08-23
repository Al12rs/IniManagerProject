using INIManagerProject.View;
using INIManagerProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using INIManagerProject.src.ViewModel;
using System.IO;

namespace INIManagerProject.ViewModel
{
    class DocumentViewModel: ViewModelBase
    {

        private Document _document;
        private EditListViewModel _editListViewModel;
        private EditContentViewModel _editContentViewModel;
        private Profile _currentProfileCache;

        public Document Document { get => _document; private set => _document = value; }
        public EditContentViewModel EditContentViewModel { get => _editContentViewModel; set => _editContentViewModel = value; }
        public EditListViewModel EditListViewModel { get => _editListViewModel; set => _editListViewModel = value; }
        public Profile CurrentProfileCache
        {
            // Does not currently listen to chages from ProfileManager.
            get => _currentProfileCache;
            set
            {
                Document.SetCurrentProfile(value);
                _currentProfileCache = value;
                CollectionViewSource.GetDefaultView(Document.EditListModel.ModelList).Refresh();
            }
        }

        public DocumentViewModel(Document document)
        {
            _document = document;
            _editListViewModel = new EditListViewModel(this);
            _editContentViewModel = new EditContentViewModel();
            _currentProfileCache = Document.ProfileManager.CurrentProfile;
            ShowManagedFileContents();
        }

        public void ShowManagedFileContents()
        {
            _editContentViewModel.Populate("Managed File: " + Document.ManagedFile.FileName, Document.ManagedFile, canSave: true);
        }
    }
}
