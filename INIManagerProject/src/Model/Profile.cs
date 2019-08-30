using INIManagerProject.Model.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Contains priority and status data for Edits of a specific document.
    /// There can be multiple profiles for each document.
    /// </summary>
    public class Profile
    {
        #region Fields

        /// <summary>
        /// List of pairs editNames-editStatus ordered by priority.
        /// This is what is read and saved on disk.
        /// </summary>
        private List<Pair<string, bool>> _editNamesAndStatusByPriority;

        #endregion Fields

        #region Properties

        public string ProfileFolder { get; set; }
        public string ProfileFilePath { get; set; }
        public int ProfileID { get; }
        public string ProfileName { get; set; }
        public Document Document { get; }
        internal List<Pair<string, bool>> EditNamesAndStatusByPriority { get => _editNamesAndStatusByPriority; set => _editNamesAndStatusByPriority = value; }


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
            _editNamesAndStatusByPriority = new List<Pair<string, bool>>();
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
        /// </summary>
        public void ReadNameListFromDisk()
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
                        var entry = new Pair<string, bool>(line.Substring(1), true);
                        _editNamesAndStatusByPriority.Add(entry);
                    }
                    else if (line[0] == '-')
                    {
                        var entry = new Pair<string, bool>(line.Substring(1), false);
                        _editNamesAndStatusByPriority.Add(entry);
                    }
                    // Do nothing if it doesn't start with +/-.
                }
            }
        }

        /// <summary>
        /// Saves the _editNamesAndStatusByPriority to ProfileName.txt.
        /// </summary>
        public void Persist()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _editNamesAndStatusByPriority.Count; i++)
            {
                sb.Append(((_editNamesAndStatusByPriority[i].Value) ? "+" : "-") + _editNamesAndStatusByPriority[i].Key + "\n");
            }
            File.WriteAllText(ProfileFilePath, sb.ToString());
        }

        /// <summary>
        /// Removes from _editNamesAndStatusByPriority all edit names that 
        /// are missing from the current ModelList
        /// </summary>
        public void ClearDanglingEditNames()
        {
            var modelList = Document.EditListModel.ModelList;
            _editNamesAndStatusByPriority.RemoveAll(pair => ! modelList.Any(edit => edit.EditName == pair.Key));
        }

        public void UpdateFromModelList()
        {
            var modelList = Document.EditListModel.ModelList;
            var sortedEditList = modelList.OrderBy(e => e.PriorityCache);
            _editNamesAndStatusByPriority.Clear();

            foreach(Edit edit in sortedEditList)
            {
                if(edit.EditName == "Base File")
                {
                    continue;
                }
                _editNamesAndStatusByPriority.Add(new Pair<string, bool>(edit.EditName, edit.StatusCache));
            }
            Persist();
        }

        #endregion PublicMethods

        #region PrivateMethods

        #endregion PrivateMethods
    }
}