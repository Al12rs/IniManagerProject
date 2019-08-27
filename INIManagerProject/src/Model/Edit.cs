using IniParser.Model;
using IniParser.Parser;
using System;
using System.Collections.Generic;
using System.IO;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Rappresents a gorup of ini lines entries to be added to the mergeTree.
    /// </summary>
    public class Edit : ViewModelBase, IRawContentProvider
    {
        #region Fields

        /// <summary>
        /// contins the raw ini lines and possibly comments of the edit.
        /// </summary>
        private string _rawContent;
        private int _priorityCache;
        private bool _statusCache;

        #endregion Fields

        #region Properties

        public IniData ParsedData { get; private set; }
        public string EditSourceFile { get; private set; }
        public string EditFolderPath { get; private set; }
        public bool IsRegular { get { return EditName != "Base File"; } }
        public string RawContent
        {
            get => _rawContent;
            set
            {
                if (_rawContent != value)
                {
                    string previousValue = _rawContent;
                    _rawContent = value;
                    if (!this.UpdateFromRawContent())
                    {
                        _rawContent = previousValue;
                    }
                    else
                    {
                        this.Persist();
                    }
                }
            }
        }
        public int EditId { get; private set; }
        public string EditName { get; set; }
        /// <summary>
        /// Contains the status of the edit, is directly mapped to the checkboxes of the list.
        /// It will update the Profile to reflect the change.
        /// Don't use this from within codebut instead use SetStatus to avoid changing the Profile unnecessarely.
        /// </summary>
        public bool StatusCache
        {
            get => _statusCache;
            set
            {
                if(_statusCache != value)
                {
                    // Update the profile when the status changes.
                    Document.ProfileManager.CurrentProfile.EditNamesAndStatusByPriority[PriorityCache - 1].Value = value;
                    _statusCache = value;
                    OnPropertyChanged("StatusCache");
                }
            }
        }
        public int PriorityCache
        {
            get => _priorityCache;
            set
            {
                _priorityCache = value;
                OnPropertyChanged("PriorityCache");
            }
        }
        public Document Document { get; private set; }

        /// <summary>
        /// These are keyNodes from the mergestructure that contain values added by this
        /// edit, for fast access.
        /// Needs to be considered if worth it.
        /// </summary>
        public List<KeyNode> AffectedKeys { get; set; }

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Constructor, will create the Edit directory using Edit name if not existent.
        /// Does not load data from disk, use UpdateFromDisk() to populate the generated Edit.
        /// </summary>
        /// <param name="editId"></param>
        /// <param name="editName"></param>
        /// <param name="doc"></param>
        public Edit(int editId, string editName, Document doc)
        {
            EditId = editId;
            EditName = editName;
            Document = doc;
            _rawContent = "";
            EditFolderPath = Path.Combine(Document.EditListModel.EditsFolder, EditName);
            if (!Directory.Exists(EditFolderPath))
            {
                Directory.CreateDirectory(EditFolderPath);
            }
            EditSourceFile = Path.Combine(EditFolderPath, EditName + ".ini");
        }

        /// <summary>
        /// Loads edit data from disk or generates the edit file if it does not exist.
        /// Parses the data.
        /// </summary>
        /// <returns></returns>
        public bool UpdateFromDisk()
        {
            if (!File.Exists(EditSourceFile))
            {
                File.Create(EditSourceFile).Dispose();
            }

            _rawContent = File.ReadAllText(EditSourceFile);
            return UpdateFromRawContent();
            // TODO: Possibly update directoryTree. Possibly not.
        }

        private bool UpdateFromRawContent()
        {
            var parser = new IniDataParser();
            parser.Configuration.ThrowExceptionsOnError = false;
            IniData ParsedData = parser.Parse(_rawContent);
            if (ParsedData == null)
            {
                return false;
            }
            return true;
        }

        #endregion Initialization

        #region PublicMethods

        /// <summary>
        /// Saves rawContent to the editSourceFile.
        /// </summary>
        public void Persist()
        {
            if (!File.Exists(EditSourceFile))
            {
                File.Create(EditSourceFile).Dispose();
            }
            File.WriteAllText(EditSourceFile, _rawContent);
        }

        /// <summary>
        /// Allows to circumnvent the updating of the profile that instead happens when using the normal proprety.
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(bool status)
        {
            if (_statusCache != status)
            {
                _statusCache = status;
                OnPropertyChanged("StatusCache");
            }
        }

        #endregion PublicMethods
    }
}