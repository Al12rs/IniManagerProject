using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    class Profile
    {
        private List<KeyValuePair<Edit,bool>> _priorityList;
        //what is read from file
        private List<KeyValuePair<string, bool>> _editNamesAndStatusByPriority;

        public Profile(int profileId, String profileName, Document doc)
        {
            ProfileID = profileId;
            ProfileName = profileName;
            Document = doc;
            _priorityList = new List<KeyValuePair<Edit, bool>>();
            _editNamesAndStatusByPriority = new List<KeyValuePair<string, bool>>();
            _editNamesAndStatusByPriority.Add(new KeyValuePair<string, bool>("Base File", true));
            var profilesFolder = Document.ProfileManager.ProfilesFolder;
            ProfileFolder = Path.Combine(profilesFolder, ProfileName);
            if (!Directory.Exists(ProfileFolder))
            {
                Directory.CreateDirectory(ProfileFolder);
            }
            ProfileFilePath = Path.Combine(ProfileFolder, ProfileName + ".txt");
            
        }

        #region Properties

        public string ProfileFolder { get; set; }
        internal string ProfileFilePath { get; set; }
        internal int ProfileID { get; }
        internal string ProfileName { get; set; }
        internal Document Document { get; }
        internal List<KeyValuePair<Edit, bool>> PriorityList { get => _priorityList;}

        #endregion

        public void ValidateAndUpdateProfileEdits()
        {
            EditListModel editListModel = Document.EditListModel;
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

        private void UpdateNameListFromEditList()
        {
            _editNamesAndStatusByPriority.Clear();
            for (int i = 0; i < _priorityList.Count; i++)
            {
                _editNamesAndStatusByPriority.Add(new KeyValuePair<string, bool>(_priorityList[i].Key.EditName, _priorityList[i].Value));
            }
        }

        //saves the _editNamesAndStatusByPriority to ProfileName.txt
        internal void PersistProfile()
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0; i< _editNamesAndStatusByPriority.Count; i++)
            {
                sb.Append( ((_editNamesAndStatusByPriority[i].Value) ? "+" : "-") + _editNamesAndStatusByPriority[i].Key + "\n");
            }
            File.WriteAllText(ProfileFilePath, sb.ToString());
        }

        //loads the content of ProfileName.txt to _editNamesAndStatusByPriority
        internal void ReadNameListFromDisk()
        {
            _editNamesAndStatusByPriority.Clear();
            String rawStr = File.ReadAllText(ProfileFilePath);
            string[] edits = rawStr.Split('\n');
            foreach (var line in edits)
            {
                if (line[0] == '+')
                {
                   var entry = new KeyValuePair<string, bool>(line.Substring(1), true);
                    _editNamesAndStatusByPriority.Add(entry);
                } else if (line[0] == '-')
                {
                    var entry = new KeyValuePair<string, bool>(line.Substring(1), false);
                    _editNamesAndStatusByPriority.Add(entry);
                }
                //do nothing if it doesn't start with +/-
            }
        }
    }
}
