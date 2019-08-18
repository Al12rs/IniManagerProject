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

namespace INIManagerProject.ViewModel
{
    class ProfileManagementViewModel : ViewModelBase
    {
        private ProfileManager _profileManager;
        public ObservableCollection<String> ProfileList { get; private set; }
        private readonly DelegateCommand _newProfile;
        private readonly DelegateCommand _deleteProfile;
        public ICommand NewProfile => _newProfile;
        public ICommand DeleteProfile => _deleteProfile;

        public ProfileManagementViewModel(ProfileManager profileManager)
        {
            _profileManager = profileManager;
            ProfileList = new ObservableCollection<String>(_profileManager.ProfileList.Keys);
            _newProfile = new DelegateCommand(OnNewProfile);
            _deleteProfile = new DelegateCommand(OnDeleteProfile);
        }

        private void OnNewProfile(object commandParameter)
        {
            int count = ProfileList.Count;
            ProfileList.Add("Profile" + count);
            _profileManager.CreateNewProfile("Profile" + count);
        }

        private void OnDeleteProfile(object commandParameter)
        {
            String selectedProfileName = (String)commandParameter;
            _profileManager.DeleteProfile(selectedProfileName);
            ProfileList.Remove(selectedProfileName);
        }

    }

}
