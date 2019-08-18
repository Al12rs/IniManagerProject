using INIManagerProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.src.ViewModel
{
    class EditContentViewModel : ViewModelBase
    {
        private Edit _edit;

        public EditContentViewModel(Edit edit)
        {
            _edit = edit;
        }

        internal Edit Edit { get => _edit; set => _edit = value; }
    }
}
