using INIManagerProject.Model;
using INIManagerProject.ViewModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.ViewModel
{
    class EditListViewModel : ViewModelBase
    {
        private EditListModel _editListModel;
        private ObservableViewModelCollection<DocumentViewModel, Document> _documentViewModelList;

        public EditListViewModel(DocumentViewModel docViewModel)
        {
            _editListModel = docViewModel.Document.EditListModel;
        }

        internal EditListModel EditListModel { get => _editListModel; set => _editListModel = value; }
    }
}
