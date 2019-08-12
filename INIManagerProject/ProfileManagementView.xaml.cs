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
            InitializeComponent();
            List<Profile> lista = new List<Profile>();
            lista.Add(new Profile { profName = "Default" });
            lista.Add(new Profile { profName = "Performance" });
            lista.Add(new Profile { profName = "Visuals" });
            lvProfiles.ItemsSource = lista;

        }

   
    }

    public class Profile{
        public String profName { get; set; }

        }
}
