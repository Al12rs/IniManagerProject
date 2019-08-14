using INIManagerProject.src.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace INIManagerProject
{
    /// <summary>
    /// Logica di interazione per ProfileManagementView.xaml
    /// </summary>
    public partial class ProfileManagementView : Window
    {
        public ProfileManagementView()
        {
            DataContext = new ProfileManagementViewModel(((App)Application.Current).IniApplication.DocumentManager.CurrentDocument.ProfileManager);
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class Profile{
        public String profName { get; set; }

        }
}
