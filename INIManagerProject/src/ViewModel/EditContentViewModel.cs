using INIManagerProject.View;
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
        private string _textContent;

        public EditContentViewModel()
        {
        }

        public Edit Edit { get => _edit; set => _edit = value; }
        // TODO: Possibly make it so that when the edit is changed TextContent is set to Edit.RawContent
        // Also implement iproperty changed.
        public string TextContent { get => _textContent; set => _textContent = value; }
    }
}
