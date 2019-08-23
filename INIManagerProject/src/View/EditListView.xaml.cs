using INIManagerProject.View;
using INIManagerProject.ViewModel;
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

namespace INIManagerProject.View
{
    /// <summary>
    /// Logica di interazione per EditListView.xaml
    /// </summary>
    public partial class EditListView : UserControl
    {

        private EditListViewModel _editListViewModel;

        public EditListView()
        {
            InitializeComponent();
        }


        private void EditListView_Loaded(object sender, EventArgs e)
        {
            _editListViewModel = (this.DataContext as EditListViewModel); ;
        }

        private void GridViewColumnHeader_SizeChanged(object sender, SizeChangedEventArgs e)
        {
             if (e.NewSize.Width <= 40) {
            e.Handled = true;
            ((GridViewColumnHeader) sender).Column.Width = 40;
        }
        }

        private void GridViewColumnHeader_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= 100)
            {
                e.Handled = true;
                ((GridViewColumnHeader)sender).Column.Width = 100;
            }
        }
    }
}
