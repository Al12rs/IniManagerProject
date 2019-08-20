﻿using INIManagerProject.Model;
using INIManagerProject.src.ViewModel;
using INIManagerProject.ViewModel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace INIManagerProject.ViewModel
{
    class EditListViewModel : ViewModelBase
    {
        private EditListModel _editListModel;

        private ICollectionView _viewList;

        public EditListViewModel(DocumentViewModel docViewModel)
        {
            _editListModel = docViewModel.Document.EditListModel;
            _viewList = CollectionViewSource.GetDefaultView(_editListModel.ModelList);
            _viewList.SortDescriptions.Add(new SortDescription("PriorityCache", ListSortDirection.Ascending));
            _viewList.Refresh();

        }

        public EditListModel EditListModel { get => _editListModel; set => _editListModel = value; }
        public ICollectionView ViewList { get => _viewList; set => _viewList = value; }
    }
}
