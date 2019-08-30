using INIManagerProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INIManagerProject.Model;
using System.Collections.ObjectModel;
using System.Windows;
using INIManagerProject.ViewModel.Utils;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;

namespace INIManagerProject.ViewModel
{
    class OpenDocumentViewModel : ViewModelBase
    {
        private readonly DocumentManager _documentManager;
        private readonly DelegateCommand _cancel;
        private readonly DelegateCommand _openDocument;
        private readonly DelegateCommand _newDocument;

        public int SelectedIndex { get; set; }
        public List<KeyValuePair<string, string>> SavedDocuments { get; set; }
        public ICommand CancelCommand => _cancel;
        public ICommand OpenDocumentCommand => _openDocument;
        public ICommand NewDocumentCommand => _newDocument;
        public OpenDocumentViewModel()
        {
            _documentManager = ((App)Application.Current).IniApplication.DocumentManager;
            _documentManager.PopulateSavedDocuments();

            //Only show the savedDocuments that are not already opened.
            SavedDocuments = new List<KeyValuePair<string, string>>(_documentManager.SavedDocuments);
            SavedDocuments.RemoveAll(pair => _documentManager.DocumentList.Any(d=> d.DocumentName == pair.Key));

            //Initialize commands
            _cancel = new DelegateCommand(OnCancel);
            _openDocument = new DelegateCommand(OnOpenDocument);
            _newDocument = new DelegateCommand(OnCreateNewDocument);
        }

        /// <summary>
        /// Closes the selection window.
        /// </summary>
        /// <param name="commandParameter">The commandParameter is the OpenExistingDocumentWindow</param>
        private void OnCancel(object commandParameter)
        {
            if(commandParameter != null)
            {
                ((Window)commandParameter).Close();
            }
        }

        /// <summary>
        /// Opens the selected existing document and closes the selection window.
        /// </summary>
        /// <param name="commandParameter">The commandParameter is the OpenExistingDocumentWindow</param>
        private void OnOpenDocument(object commandParameter)
        {
            var selectedItem = SavedDocuments[SelectedIndex];
            String docNameSelected = selectedItem.Key;
            Document newDoc = _documentManager.CreateAndLoadDocumentFromName(docNameSelected);
            _documentManager.CurrentDocument = newDoc;
            if (commandParameter != null)
            {
                ((Window)commandParameter).Close();
            }
        }

        /// <summary>
        /// Create and open a new document based on the selection choice made by the user.
        /// </summary>
        /// <param name="commandParameter">The commandParameter is the OpenExistingDocumentWindow</param>
        private void OnCreateNewDocument(object commandParameter)
        {
            if (commandParameter != null)
            {
                ((Window)commandParameter).Close();
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "INI files(*.ini)|*.ini";
            if (openFileDialog.ShowDialog() == true)
            {
                var dialog = new InputDialogue("Insert name of new Edit:");
                while (dialog.ShowDialog() == true)
                {
                    string docName = dialog.Answer;
                    if (docName == "" || _documentManager.DocumentList.Any(x => x.DocumentName == docName))
                    {
                        dialog = new InputDialogue("Name already in use or invalid, please select a different name:");
                        continue;
                    }
                    string folderPath;
                    try
                    {
                        folderPath = Path.Combine(_documentManager.DocumentsFolderPath, docName);

                    }
                    catch (Exception ex)
                    {
                        dialog = new InputDialogue("Name already in use or invalid, please select a different name:");
                        continue;
                    }

                    _documentManager.CurrentDocument = _documentManager.CreateNewDocument(openFileDialog.FileName, docName);
                    break;
                }
            }
        }
    }
}
