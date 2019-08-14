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
using INIManagerProject.src.ViewModel;

namespace INIManagerProject
{
    /// <summary>
    /// Interaction logic for OpenExistingDocumentWindow.xaml
    /// </summary>
    public partial class OpenExistingDocumentWindow : Window
    {
        private DocumentManager _documentManager;
        public OpenExistingDocumentWindow()
        {
            //OpenDocumentViewModel opvw = new OpenDocumentViewModel(((App)Application.Current).IniApplication.DocumentManager);
            _documentManager = ((App)Application.Current).IniApplication.DocumentManager;
            DataContext = _documentManager;

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
            Document newDoc = _documentManager.CreateAndLoadDocumentFromName(docNameSelected);
            _documentManager.CurrentDocument = newDoc;
            this.Close();
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
