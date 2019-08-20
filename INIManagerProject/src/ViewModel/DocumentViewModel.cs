using INIManagerProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace INIManagerProject.ViewModel
{
    class DocumentViewModel: ViewModelBase
    {

        private Document _document;
        private EditListViewModel _editListViewModel;
        private EditContentView _editContentViewModel;
        private Profile _currentProfileCache;

        public Document Document { get => _document; private set => _document = value; }
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
            _editContentViewModel = new EditContentView();
            _currentProfileCache = Document.ProfileManager.CurrentProfile;
            
            // TODO: Use a single ViewModel with properties that can be set from the other viewModels.
            // Pass the EditContentViewModel to the EditListViewModel so that it can populate it correctly when the selection is changed.
        }
    }
}
