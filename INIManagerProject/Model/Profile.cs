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
        private int _profileId;
        private String _profileName;
        private String _profileFolder;
        private Document _document;

        public Profile(int profileId, String profileName, Document doc)
        {
            _profileId = profileId;
            _profileName = profileName;
            _document = doc;
            var appAppdataFolder = Document.ProfileManager.ProfilesFolder;
            ProfileFolder = Path.Combine(appAppdataFolder, ProfileFolder);
            if (!Directory.Exists(ProfileFolder))
            {
                Directory.CreateDirectory(ProfileFolder);
            }
        }

        public string ProfileFolder { get => _profileFolder; set => _profileFolder = value; }
        internal int ProfileID { get => _profileId; }
        internal string ProfileName { get => _profileName; set => _profileName = value; }
        internal Document Document { get => _document; }
    }
}
