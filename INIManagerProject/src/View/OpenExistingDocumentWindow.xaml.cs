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
            
            /*List<Document> docList = new List<Document>();
            docList.Add(new Document() { docName = "documento 1" , docPath="Path/of/Managed/file1.INI"});
            docList.Add(new Document() { docName = "documento 2", docPath = "Path/of/Managed/file2.INI" });*/
            //lvDoc.ItemsSource = opvw();


        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            //load selected document
            //((App)Application.Current).IniApplication.DocumentManager.CreateAndLoadDocumentFromName("Skyrim");
            var selectedItem = (KeyValuePair<string, string>)lvDoc.SelectedItem;
            String docNameSelected = selectedItem.Key;
            Document newDoc = ((App)Application.Current).IniApplication.DocumentManager.CreateAndLoadDocumentFromName(docNameSelected);
            ((App)Application.Current).IniApplication.DocumentManager.CurrentDocument = newDoc;
            this.Close();
        }


        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GridViewColumnHeader_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= 140)
            {
                e.Handled = true;
                ((GridViewColumnHeader)sender).Column.Width = 140;
            }

        }
    }
}
