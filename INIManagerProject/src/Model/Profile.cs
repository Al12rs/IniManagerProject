using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Contains priority and status data for Edits of a specific document.
    /// There can be multiple profiles for each document.
    /// </summary>
    internal class Profile
    {
        #region Fields

        /// <summary>
        /// List of pairs Edit-Status ordered by priority (0 is lowest).
        /// </summary>
        private List<KeyValuePair<Edit, bool>> _priorityList;

        /// <summary>
        /// List of pairs editNames-editStatus ordered by priority.
        /// This is what is read and saved on disk.
        /// </summary>
        private List<KeyValuePair<string, bool>> _editNamesAndStatusByPriority;

        #endregion Fields

        #region Properties

        public string ProfileFolder { get; set; }
        internal string ProfileFilePath { get; set; }
        internal int ProfileID { get; }
        internal string ProfileName { get; set; }
        internal Document Document { get; }
        internal List<KeyValuePair<Edit, bool>> PriorityList { get => _priorityList; }

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Constructor, will populate:
        /// Id, Document, ProfileName.
        /// Generate Profile folder and editsOrder.txt if not present.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="profileName"></param>
        /// <param name="doc"></param>
        public Profile(int profileId, String profileName, Document doc)
        {
            ProfileID = profileId;
            ProfileName = profileName;
            Document = doc;
            _priorityList = new List<KeyValuePair<Edit, bool>>();
            _editNamesAndStatusByPriority = new List<KeyValuePair<string, bool>>();
            var profilesFolder = Document.ProfileManager.ProfilesFolder;
            ProfileFolder = Path.Combine(profilesFolder, ProfileName);
            if (!Directory.Exists(ProfileFolder))
            {
                Directory.CreateDirectory(ProfileFolder);
            }
            ProfileFilePath = Path.Combine(ProfileFolder, "editsOrder.txt");
            if (!File.Exists(ProfileFilePath))
            {
                File.Create(ProfileFilePath).Dispose();
            }
        }

        #endregion Initialization

        #region PublicMethods

        /// <summary>
        /// Loads the content of editsOrder.txt to _editNamesAndStatusByPriority.
        /// Does not populate _priorityList.
        /// </summary>
        internal void ReadNameListFromDisk()
        {
            _editNamesAndStatusByPriority.Clear();
            string rawStr = File.ReadAllText(ProfileFilePath);
            string[] edits = rawStr.Split('\n');
            foreach (var line in edits)
            {
                if (line.Length > 0)
                {
                    if (line[0] == '+')
                    {
                        var entry = new KeyValuePair<string, bool>(line.Substring(1), true);
                        _editNamesAndStatusByPriority.Add(entry);
                    }
                    else if (line[0] == '-')
                    {
                        var entry = new KeyValuePair<string, bool>(line.Substring(1), false);
                        _editNamesAndStatusByPriority.Add(entry);
                    }
                    //do nothing if it doesn't start with +/-
                }
            }
        }

        /// <summary>
        /// Applies _editNamesAndStatusByPriority to the edits in the EditList
        /// all while populating the _priorityList.
        /// Updates  _editNamesAndStatusByPriority removing edits that are missing from EditList and
        /// adding the ones that are present in EditList but missing in _editNamesAndStatusByPriority.
        /// </summary>
        public void ValidateAndUpdatePriorityLists()
        {
            EditListModel editListModel = Document.EditListModel;
            _priorityList.Clear();
            //Add listed Edits to prioritylist
            foreach (var nameStatusPair in _editNamesAndStatusByPriority)
            {
                Edit currentEdit;
                if (editListModel.EditMapByName.TryGetValue(nameStatusPair.Key, out currentEdit))
                {
                    _priorityList.Add(new KeyValuePair<Edit, bool>(currentEdit, nameStatusPair.Value));
                }
            }

            //Add new Edits to bottom of prioritylist
            foreach (Edit edit in editListModel.EditMapByName.Values)
            {
                bool missing = true;
                for (int i = 0; i < _priorityList.Count; i++)
                {
                    if (edit == _priorityList[i].Key)
                    {
                        //This edit is already present
                        missing = false;
                        break;
                    }
                }
                if (missing)
                {
                    _priorityList.Add(new KeyValuePair<Edit, bool>(edit, false));
                }
            }

            //now priorityList is complete and _editNamesAndStatusByPriority can be updated to reflect it.
            UpdateNameListFromEditList();
        }

        /// <summary>
        /// Saves the _editNamesAndStatusByPriority to ProfileName.txt.
        /// </summary>
        internal void Persist()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _editNamesAndStatusByPriority.Count; i++)
            {
                sb.Append(((_editNamesAndStatusByPriority[i].Value) ? "+" : "-") + _editNamesAndStatusByPriority[i].Key + "\n");
            }
            File.WriteAllText(ProfileFilePath, sb.ToString());
        }

        #endregion PublicMethods

        #region PrivateMethods

        /// <summary>
        /// Takes a fully populated _priorityList and generates t
        /// </summary>
        private void UpdateNameListFromEditList()
        {
            _editNamesAndStatusByPriority.Clear();
            for (int i = 0; i < _priorityList.Count; i++)
            {
                _editNamesAndStatusByPriority.Add(new KeyValuePair<string, bool>(_priorityList[i].Key.EditName, _priorityList[i].Value));
            }
        }

        #endregion PrivateMethods
    }
}