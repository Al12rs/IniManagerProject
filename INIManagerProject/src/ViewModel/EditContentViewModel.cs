using INIManagerProject.View;
using INIManagerProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INIManagerProject.ViewModel.Utils;
using System.Windows.Input;
using INIManagerProject.ViewModel;
using System.Windows;

namespace INIManagerProject.ViewModel
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
        public string Header { get => _header; set { SetProperty(ref _header, value, "Header"); } }
        public DocumentViewModel DocumentViewModel { get; set; }
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
        public bool CanSave { get => _canSave; set { SetProperty(ref _canSave, value, "CanSave"); } }


        public EditContentViewModel(DocumentViewModel docViewModel)
        {
            DocumentViewModel = docViewModel;
            CanSave = false;
            _saveContent = new DelegateCommand(OnSaveContentPressed);
        }

        public void Populate(string header, IRawContentProvider rawContentProvider, bool canSave)
        {
            ContentSource = rawContentProvider;
            CanSave = canSave;
            Header = header;
        }

        private void OnSaveContentPressed(object commandParameters)
        {
            if (CanSave)
            {
                if(ContentSource.RawContent != TextContent)
                {
                        ContentSource.RawContent = TextContent;

                    if (ContentSource.RawContent != TextContent)
                    {
                        // Failed to set Raw Content pobably because invalid INI format.
                        MessageBox.Show("Save Failed: Invalid INI format.");
                    }
                }
            }
        }
    }
}
