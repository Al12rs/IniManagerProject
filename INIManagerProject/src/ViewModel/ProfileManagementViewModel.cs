using INIManagerProject.View;
using INIManagerProject.Model;
using INIManagerProject.ViewModel.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace INIManagerProject.ViewModel
{
    class ProfileManagementViewModel : ViewModelBase
    {
        private ProfileManager _profileManager;
        private readonly DelegateCommand _newProfile;
        private readonly DelegateCommand _deleteProfile;

        public ObservableCollection<String> ProfileList { get; private set; }
        public ICommand NewProfile => _newProfile;
        public ICommand DeleteProfile => _deleteProfile;

        public ProfileManagementViewModel(ProfileManager profileManager)
        {
            _profileManager = profileManager;
            ProfileList = new ObservableCollection<String>(_profileManager.ProfileList.Select(p => p.ProfileName));
            _newProfile = new DelegateCommand(OnNewProfile);
            _deleteProfile = new DelegateCommand(OnDeleteProfile);
        }

        private void OnNewProfile(object commandParameter)
        {
            var dialog = new InputDialogue("Insert name of new Profile:");
            while(dialog.ShowDialog() == true)
            {
                string profileName = dialog.Answer;
                if (profileName == "" || _profileManager.ProfileList.Any(e => e.ProfileName == profileName)){

                    dialog = new InputDialogue("Name already in use or invalid, please select a different name:");
                    continue;
                }
                string folderPath;
                try
                {
                    folderPath = Path.Combine(_profileManager.ProfilesFolder, profileName);
                }
                catch (Exception ex)
                {
                    dialog = new InputDialogue("Name already in use or invalid, please select a different name:");
                    continue;
                }


                ProfileList.Add(profileName);
                _profileManager.CreateNewProfile(profileName);
                return;
            }
        }
         
        

        private void OnDeleteProfile(object commandParameter)
        {
            String selectedProfileName = (String)commandParameter;
            _profileManager.DeleteProfile(selectedProfileName);
            ProfileList.Remove(selectedProfileName);
        }

    }

}
