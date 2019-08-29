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
using INIManagerProject.Model;
using INIManagerProject.View;
using INIManagerProject.ViewModel;

namespace INIManagerProject.View
{
    /// <summary>
    /// Interaction logic for OpenExistingDocumentWindow.xaml
    /// </summary>
    public partial class OpenExistingDocumentWindow : Window
    {
        private OpenDocumentViewModel _openDocumentViewModel;

        public OpenExistingDocumentWindow()
        {
            OpenDocumentViewModel _openDocumentViewModel = new OpenDocumentViewModel();
            DataContext = _openDocumentViewModel;

            InitializeComponent();
        }

    }
}
