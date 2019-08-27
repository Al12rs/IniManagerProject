using INIManagerProject.View;
using INIManagerProject.Model;
using INIManagerProject.src.ViewModel;
using INIManagerProject.ViewModel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace INIManagerProject.ViewModel
{
    class EditListViewModel : ViewModelBase
    {
        private EditListModel _editListModel;

        private ICollectionView _viewList;
        private readonly DelegateCommand _selectionChanged;
        private readonly DelegateCommand _addEdit;
        private readonly DelegateCommand _removeEdit;

        public Edit SelectedItem { get; set; }
        public DocumentViewModel DocumentViewModel { get; set;}
        public EditListModel EditListModel { get => _editListModel; set => _editListModel = value; }
        public ICollectionView ViewList { get => _viewList; set => _viewList = value; }
        public ICommand SelectionChanged => _selectionChanged;
        public ICommand AddEdit => _addEdit;
        public ICommand RemoveEdit => _removeEdit;
        public int SelectedIndex { get; set; }

        public EditListViewModel(DocumentViewModel docViewModel)
        {
            DocumentViewModel = docViewModel;
            _removeEdit = new DelegateCommand(OnRemoveEditPressed);
            _addEdit = new DelegateCommand(OnAddEditPressed);
            _selectionChanged = new DelegateCommand(OnSelectionChanged);
            _editListModel = docViewModel.Document.EditListModel;
            _viewList = CollectionViewSource.GetDefaultView(_editListModel.ModelList);
            _viewList.SortDescriptions.Add(new SortDescription("PriorityCache", ListSortDirection.Ascending));
            _viewList.Refresh();
        }

        private void OnSelectionChanged(object commandParameter)
        {
            DocumentViewModel.ShowSelectedEditContents();
        }

        private void OnAddEditPressed(object commandParameter)
        {

        }

        private void OnRemoveEditPressed(object commandParameter)
        {
            Edit edit = commandParameter as Edit;
            if (edit != null)
            {
                EditListModel.RemoveEdit(edit);
            }
        }
    }
}
