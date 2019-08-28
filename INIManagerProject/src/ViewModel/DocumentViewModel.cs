using INIManagerProject.View;
using INIManagerProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using INIManagerProject.ViewModel;
using System.IO;
using System.Windows.Input;
using INIManagerProject.ViewModel.Utils;

namespace INIManagerProject.ViewModel
{
    class DocumentViewModel : ViewModelBase
    {

        private Document _document;
        private EditListViewModel _editListViewModel;
        private EditContentViewModel _editContentViewModel;
        private Profile _currentProfileCache;
        private readonly DelegateCommand _mergeResultSelected;
        private DelegateCommand _managedFileSelectged;
        private DelegateCommand _calculateMergeResultPressed;
        private DelegateCommand _applyMergeResult;


        public Document Document { get => _document; private set => _document = value; }
        public EditContentViewModel EditContentViewModel { get => _editContentViewModel; set => _editContentViewModel = value; }
        public EditListViewModel EditListViewModel { get => _editListViewModel; set => _editListViewModel = value; }
        public int MergeResultSelectionIndex { get; set; }
        public int ManagedFileSelectionIndex { get; set; }
        public ICommand MergeResultSelected => _mergeResultSelected;
        public ICommand ManagedFileSelected => _managedFileSelectged;
        public ICommand CalculateMergeResult => _calculateMergeResultPressed;
        public ICommand ApplyMergeResult => _applyMergeResult;
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
            _mergeResultSelected = new DelegateCommand(OnMergeResultSelected);
            _managedFileSelectged = new DelegateCommand(OnManagedFileSelected);
            _calculateMergeResultPressed = new DelegateCommand(OnCalculateMergeResultPressed);
            _applyMergeResult = new DelegateCommand(OnApplyMergeResultPressed);


            _editListViewModel = new EditListViewModel(this);
            _editContentViewModel = new EditContentViewModel(this);
            _currentProfileCache = Document.ProfileManager.CurrentProfile;
            ShowManagedFileContents();
        }

        public void ShowManagedFileContents()
        {
            _editContentViewModel.Populate("Managed File: " + Document.ManagedFile.FileName, Document.ManagedFile, canSave: true);
        }

        public void ShowMergeResultContents()
        {
            _editContentViewModel.Populate("Merge Result: ", Document.MergeTree, canSave: false);
        }

        public void ShowSelectedEditContents()
        {
            if (EditListViewModel.SelectedItem != null)
            {
                MergeResultSelectionIndex = -1;
                ManagedFileSelectionIndex = -1;
                _editContentViewModel.Populate("Edit: " + EditListViewModel.SelectedItem.EditName, EditListViewModel.SelectedItem, canSave: true);
            }
        }

        private void OnMergeResultSelected(object commandParameter)
        {
            ManagedFileSelectionIndex = -1;
            EditListViewModel.SelectedItem = null;
            // TODO: Uncomment this once RawContent of Mergestructure isn't null.
            ShowMergeResultContents();
        }

        private void OnManagedFileSelected(object commandParameter)
        {
            MergeResultSelectionIndex = -1;
            EditListViewModel.SelectedIndex = -1;
            ShowManagedFileContents();
        }


        private void OnCalculateMergeResultPressed(object commandParameter)
        {
            Document.MergeTree.CalculateMergeResult();
            ShowMergeResultContents();
        }

        private void OnApplyMergeResultPressed(object commandParameter)
        {
            Document.ManagedFile.RawContent = Document.MergeTree.RawContent;
            ShowManagedFileContents();
        }
    }
}
