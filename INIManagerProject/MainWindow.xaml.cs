using INIManagerProject.Model;
using Microsoft.Win32;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TabItem> _tabItems;
        private TabItem _tabAdd;
        private DocumentManager _documentManager;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                _documentManager = ((App)Application.Current).IniApplication.DocumentManager;
                // initialize tabItem array
                _tabItems = new List<TabItem>();

                // add a tabItem with + in header 
                //TabItem tabAdd = new TabItem();
                //tabAdd.Header = "+";

               // _tabItems.Add(tabAdd);

                // add first tab
                //this.AddTabItem();

                // bind tab control
                document.DataContext = _documentManager;

                document.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       private TabItem AddTabItem()
        {
            int count = _tabItems.Count;

            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = string.Format("DocumentName {0}", count);
            tab.Name = string.Format("tab{0}", count);
            tab.HeaderTemplate = document.FindResource("TabHeader") as DataTemplate;

            // add controls to tab item, this case I added just a textbox
            TextBox txt = new TextBox();
            txt.Name = "txt";
            DocumentView dv = new DocumentView();
            tab.Content = dv;

            // insert tab item right before the last (+) tab item
            _tabItems.Insert(count, tab);
            return tab;
        } 

        private void document_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tab = document.SelectedItem as TabItem;

            if (tab != null && tab.Header != null)
            {
                if (tab.Header.Equals("+"))
                {
                    // clear tab control binding
                    document.DataContext = null;

                    // add new tab
                    TabItem newTab = this.AddTabItem();

                    // bind tab control
                    document.DataContext = _tabItems;

                    // select newly added tab item
                    document.SelectedItem = newTab;
                }
                else
                {
                    // your code here...
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tabName = (sender as Button).CommandParameter.ToString();

            var item = document.Items.Cast<TabItem>().Where(i => i.Name.Equals(tabName)).SingleOrDefault();

            TabItem tab = item as TabItem;

            if (tab != null)
            {
                if (_tabItems.Count < 3)
                {
                    MessageBox.Show("Cannot remove last tab.");
                }
                else if (MessageBox.Show(string.Format("Are you sure you want to remove the tab '{0}'?", tab.Header.ToString()),
                    "Remove Tab", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // get selected tab
                    TabItem selectedTab = document.SelectedItem as TabItem;

                    // clear tab control binding
                    document.DataContext = null;

                    _tabItems.Remove(tab);

                    // bind tab control
                    document.DataContext = _tabItems;

                    // select previously selected tab. if that is removed then select first tab
                    if (selectedTab == null || selectedTab.Equals(tab))
                    {
                        selectedTab = _tabItems[0];
                    }
                    document.SelectedItem = selectedTab;
                }
            }
        }

        private void mnuOpen_Click(object sender, RoutedEventArgs e)
        {

            // Placeholder: hardcoded to open skyrim.ini document.
            ((App)Application.Current).IniApplication.DocumentManager.CreateAndLoadDocumentFromName("Skyrim");
            //generate new window
            var openDocumentWindow = new OpenExistingDocumentWindow();
            openDocumentWindow.Show();

        }

        private void mnuNew_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "INI files(*.ini)|*.ini";
            if (openFileDialog.ShowDialog() == true)
            {
                ((App)Application.Current).IniApplication.DocumentManager.CreateNewDocument(openFileDialog.FileName);
            }

        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            //Persist all settings.
            ((App)Application.Current).IniApplication.Persist();
        }
    }
}
