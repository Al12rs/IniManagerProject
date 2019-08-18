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

        public Document Document { get => _document; private set => _document = value; }

        public DocumentViewModel(Document document)
        {
            _document = document;
        }
    }
}
