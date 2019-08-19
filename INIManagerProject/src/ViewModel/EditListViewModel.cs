using INIManagerProject.Model;
using INIManagerProject.src.ViewModel;
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

        public EditListViewModel(DocumentViewModel docViewModel)
        {
            _editListModel = docViewModel.Document.EditListModel;
        }

        public EditListModel EditListModel { get => _editListModel; set => _editListModel = value; }
    }
}
