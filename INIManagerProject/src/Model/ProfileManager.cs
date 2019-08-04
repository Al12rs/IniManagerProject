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
    class ProfileManager
    {
        //list of all the profiles managed and associated to the current document
        private Dictionary<string, Profile> _profileList;
        //associated document
        private Document _document;

        //path of profiles folder
        private string _profilesFolder;


        public ProfileManager(Document document)
        {
            Document = document;
            _profileList = new Dictionary<string, Profile>();
            IdBroker = new IdBroker();

            //create Profiles folder if it isn't present
            ProfilesFolder = Path.Combine(Document.DocumentFolder, "Profiles");
            if (!Directory.Exists(ProfilesFolder))
            {
                Directory.CreateDirectory(ProfilesFolder);
            }

        }

        public void initializeNewProfileMananger()
        {
            //generate default profile
            var defaultProfile = new Profile(IdBroker.NextId, "default", Document);
            _profileList.Add("default",defaultProfile);

            //enable the default profile
            CurrentProfile = defaultProfile;
        }

        public void LoadFromDisk()
        {
            //needed to get the current profile from last session.
            string[] subdirs = Directory.GetDirectories(ProfilesFolder)
                             .Select(Path.GetFileName)
                             .ToArray();
            //TODO: check if list is empty and create default profile if it is.
            foreach (var profileName in subdirs)
            {
                int newId = IdBroker.NextId;
                var loadedProfile = new Profile(newId, profileName, Document);
                _profileList.Add(profileName, loadedProfile);
                loadedProfile.ReadNameListFromDisk();
            }
            LoadAndSetCurrentProfileFromDisk();
        }

        private void LoadAndSetCurrentProfileFromDisk()
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


        #region Properties

        public string ProfilesFolder { get => _profilesFolder; set => _profilesFolder = value; }
        internal Profile CurrentProfile { get; private set; }
        internal Document Document { get => _document; set => _document = value; }
        internal IdBroker IdBroker { get; private set; }

        #endregion
    }
}
