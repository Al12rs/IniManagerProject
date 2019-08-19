using INIManagerProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.ViewModel
{
    class DocumentViewModel: ViewModelBase
    {

        private Document _document;
        private EditListViewModel _editListViewModel;

        public Document Document { get => _document; private set => _document = value; }
        public EditListViewModel EditListViewModel { get => _editListViewModel; set => _editListViewModel = value; }

        public DocumentViewModel(Document document)
        {
            _document = document;
            _editListViewModel = new EditListViewModel(this);
            // TODO: Add viewModel for EditContentView. Use a single ViewModel with properties that can be set from the other viewModels.
            // Pass the EditContentViewModel to the EditListViewModel so that it can populate it correctly when the selection is changed.
        }
    }
}
