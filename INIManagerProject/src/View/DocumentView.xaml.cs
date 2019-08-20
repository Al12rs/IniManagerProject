using INIManagerProject.Model;
using INIManagerProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using INIManagerProject.View;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace INIManagerProject.View
{
    /// <summary>
    /// Logica di interazione per DocumentView.xaml
    /// </summary>
    public partial class DocumentView : UserControl
    {
        private DocumentViewModel _documentViewModel;

        public DocumentView()
        {
            InitializeComponent();
        }



        private void mnuManage_Click(object sender, RoutedEventArgs e)
        {
            var openManageProfileWindow = new ProfileManagementView();
            openManageProfileWindow.Show();
        }

        private void DocumentView_Loaded(object sender, RoutedEventArgs e)
        {
            _documentViewModel = DataContext as DocumentViewModel;
        }

    }


   

}
