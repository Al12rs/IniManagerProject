using INIManagerProject.util;
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
        //currently used profile
        private Profile _currentProfile;
        //list of all the profiles managed and associated to the current document
        private List<Profile> _profileList;
        //associated document
        private Document _document;
        //class capable of generating IDs
        private IdBroker _idBroker;
        //path of profiles folder
        private string _profilesFolder;


        public ProfileManager(Document document)
        {
            Document = document;
            _profileList = new List<Profile>();

            //create Profiles folder if it isn't present
            ProfilesFolder = Path.Combine(Document.DocumentFolder, "Profiles");
            if (!Directory.Exists(ProfilesFolder))
            {
                Directory.CreateDirectory(ProfilesFolder);
            }

        }

        public void initializeNewProfile()
        {
            //generate default profile
            var defaultProfile = new Profile(_idBroker.NextId, "default", Document);
            _profileList.Add(defaultProfile);

            //enable the default profile
            _currentProfile = defaultProfile;
        }

        //properties
        public string ProfilesFolder { get => _profilesFolder; set => _profilesFolder = value; }
        internal Profile CurrentProfile { get => _currentProfile;}
        internal Document Document { get => _document; set => _document = value; }
    }
}
