using INIManagerProject.View;
using INIManagerProject.Model;
using INIManagerProject.ViewModel;
using INIManagerProject.ViewModel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.IO;
using GongSolutions.Wpf.DragDrop;

namespace INIManagerProject.ViewModel
{
    class EditListViewModel : ViewModelBase, IDropTarget
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
            var dialog = new InputDialogue("Insert name of new Edit:");
            while (dialog.ShowDialog() == true)
            {
                string folderName = dialog.Answer;
                if(folderName == "" || EditListModel.ModelList.Any(e=> e.EditName == folderName))
                {
                    dialog = new InputDialogue("Name already in use or invalid, please select a different name:");
                    continue;
                }
                string folderPath;
                try
                {
                    folderPath = Path.Combine(EditListModel.EditsFolder, folderName);

                }
                catch (Exception ex)
                {
                    dialog = new InputDialogue("Name already in use or invalid, please select a different name:");
                    continue;
                }
                if (EditListModel.AddEdit(folderName) != null)
                {
                    break;
                }
            }
            
        }

        private void OnRemoveEditPressed(object commandParameter)
        {
            if (commandParameter is Edit edit)
            {
                EditListModel.RemoveEdit(edit);
            }
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            Edit sourceItem = dropInfo.Data as Edit;
            Edit targetItem = dropInfo.TargetItem as Edit;
            RelativeInsertPosition relativePosition = dropInfo.InsertPosition;
            // Avoid cases where there are no targets and no sources, as well
            // as cases where the drop isn't before or after.
            // Also avoid drops before Base File.
            if(sourceItem != null && targetItem != null
                && relativePosition != RelativeInsertPosition.TargetItemCenter
                && sourceItem.IsRegular
                && !(!targetItem.IsRegular && relativePosition != RelativeInsertPosition.AfterTargetItem))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = System.Windows.DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            Edit sourceItem = dropInfo.Data as Edit;
            Edit targetItem = dropInfo.TargetItem as Edit;
            RelativeInsertPosition relativePosition = dropInfo.InsertPosition;
            if (sourceItem != null && targetItem != null
                && relativePosition != RelativeInsertPosition.TargetItemCenter
                && sourceItem.IsRegular
                && !(!targetItem.IsRegular && relativePosition != RelativeInsertPosition.AfterTargetItem))
            {
                int oldPriority = sourceItem.PriorityCache;
                int newPriority = targetItem.PriorityCache;
                if(relativePosition.HasFlag(RelativeInsertPosition.AfterTargetItem))
                {
                    newPriority += 1;
                }

                if(oldPriority < newPriority)
                {
                    newPriority -= 1;
                }

                EditListModel.ChangeEditPriority(sourceItem, ref newPriority);
                _viewList.Refresh();

            }
        }
    }
}
