using INIManagerProject.View;
using INIManagerProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INIManagerProject.ViewModel.Utils;
using System.Windows.Input;

namespace INIManagerProject.src.ViewModel
{
    class EditContentViewModel : ViewModelBase
    {
        private Edit _edit;
        private string _textContent;
        private string _contentSource;
        private string _header;
        private DelegateCommand _saveContent;
        private bool _canSave;

        public string TextContent { get => _textContent; set => _textContent = value; }
        public string Header { get => _header; set => _header = value; }
        public Edit Edit { get => _edit; set => _edit = value; }
        public ICommand SaveContent => _saveContent;
        public string ContentSource { get => _contentSource; set => _contentSource = value; }
        public bool CanSave { get => _canSave; set => _canSave = value; }

        public event EventHandler SaveContentPressed;

        public EditContentViewModel()
        {
            CanSave = false;
            _saveContent = new DelegateCommand(OnSaveContentPressed);
        }

        private void OnSaveContentPressed(object commandParameters)
        {
            SaveContentPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
