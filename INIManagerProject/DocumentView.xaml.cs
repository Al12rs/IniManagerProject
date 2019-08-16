using INIManagerProject.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace INIManagerProject
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
            // HACK: Pretty sure this does not work correctly but heh.
            //_documentViewModel = new DocumentViewModel((Document)DataContext);
            //DataContext = _documentViewModel;
        }

        private void mnuManage_Click(object sender,RoutedEventArgs e )
        {
            var openManageProfileWindow = new ProfileManagementView();
            openManageProfileWindow.Show();


        }

    }


   

}
