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
        private IRawContentProvider _contentSource;
        private string _header;
        private DelegateCommand _saveContent;
        private bool _canSave;

        public string TextContent
        {
            get => _textContent;
            set
            {
                SetProperty(ref _textContent, value, "TextContent");
            }
        }
        public string Header { get => _header; set => _header = value; }
        public ICommand SaveContent => _saveContent;
        public IRawContentProvider ContentSource
        {
            get => _contentSource;
            set
            {
                _contentSource = value;
                TextContent = _contentSource.RawContent;
            }
        }
        public bool CanSave { get => _canSave; set => _canSave = value; }


        public EditContentViewModel()
        {
            CanSave = false;
            _saveContent = new DelegateCommand(OnSaveContentPressed);
        }

        private void OnSaveContentPressed(object commandParameters)
        {
            if (CanSave)
            {
                if(ContentSource.RawContent != TextContent)
                {
                    ContentSource.RawContent = TextContent;
                }
            }
        }
    }
}
