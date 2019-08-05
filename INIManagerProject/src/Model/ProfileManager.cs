using INIManagerProject.util;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Manages the profiles of a specific Document.
    /// Single instance for each Document object.
    /// </summary>
    class ProfileManager
    {
        #region Fields
        /// <summary>
        /// Dictionary of string ProfileNames->Profiles 
        /// </summary>
        private Dictionary<string, Profile> _profileList;
        #endregion

        #region Properties

        public string ProfilesFolder { get; private set; }
        internal Profile CurrentProfile { get; private set; }
        internal Document Document { get; set; }
        internal IdBroker IdBroker { get; private set; }

        #endregion

        #region Initialization
        public ProfileManager(Document document)
        {
            Document = document;
            _profileList = new Dictionary<string, Profile>();
            IdBroker = new IdBroker();

            ProfilesFolder = Path.Combine(Document.DocumentFolderPath, "Profiles");
            if (!Directory.Exists(ProfilesFolder))
            {
                Directory.CreateDirectory(ProfilesFolder);
            }
        }
        #endregion

        #region PublicMethods

        /// <summary>
        /// Load all profiles found in the Profiles folder of the Document
        /// and set the last active as the currentProfile if possible,
        /// otherwise set a random one as currentProfile.
        /// </summary>
        public void LoadProfilesFromDisk()
        {
            string[] subdirs = Directory.GetDirectories(ProfilesFolder)
                             .Select(Path.GetFileName)
                             .ToArray();
            if(subdirs == null || subdirs.Length < 1)
            {
                CreateDefaultProfile();
            }
            else
            {
                foreach (var profileName in subdirs)
                {
                    CreateProfileFromDisk(profileName);
                }
            }
            SetCurrentProfileFromDisk();
        }

        /// <summary>
        /// Only persists the current Profile and the name of the current profile.
        /// </summary>
        public void Persist()
        {
            CurrentProfile.Persist();
            Document.ParsedDocumentSettings["Profiles"]["currentProfile"]
                = CurrentProfile.ProfileName;
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Create a new Profile object and add it to the list given a profileName.
        /// It will either load saved data on disk or create new data if not present.
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns>Generated profile</returns>
        private Profile CreateProfileFromDisk(string profileName)
        {
            int newId = IdBroker.NextId;
            var loadedProfile = new Profile(newId, profileName, Document);
            _profileList.Add(profileName, loadedProfile);
            loadedProfile.ReadNameListFromDisk();
            return loadedProfile;
        }

        /// <summary>
        /// Generates a Default profile.
        /// </summary>
        /// <returns>the generated Profile</returns>
        private Profile CreateDefaultProfile()
        {
            return CreateProfileFromDisk("Default");
        }

        /// <summary>
        /// Will read the DocumentSettings to determine the last active profile.
        /// If no matching profile is found a random one is selected.
        /// </summary>
        private void SetCurrentProfileFromDisk()
        {
            IniData documentSettings = Document.ParsedDocumentSettings;
            var currentProfileName = documentSettings["Profiles"]["currentProfile"];
            Profile foundProfile;
            if (currentProfileName != null && _profileList.TryGetValue(currentProfileName, out foundProfile))
            {
                CurrentProfile = foundProfile;
            }
            else
            {
                //set the current profile to a random one from the list.
                CurrentProfile = _profileList.First().Value;
            }
        }

        #endregion

    }
}
