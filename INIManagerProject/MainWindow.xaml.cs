using INIManagerProject.Model;
using INIManagerProject.src.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        MainWindowViewModel _mainWindowViewModel;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                _mainWindowViewModel = new MainWindowViewModel();
                documentTabControl.DataContext = _mainWindowViewModel;
                
                // initialize tabItem array

                //New stuff


                // add a tabItem with + in header 
                //TabItem tabAdd = new TabItem();
                //tabAdd.Header = "+";

               // _tabItems.Add(tabAdd);

                // add first tab
                //this.AddTabItem();

                // bind tab control
                //tabControl.DataContext = _tabItems;

                //tabControl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Should not be needed.
        private TabItem AddTabItem()
        {
            /*int count = _tabItems.Count;

            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = string.Format("DocumentName {0}", count);
            tab.Name = string.Format("tab{0}", count);
            tab.HeaderTemplate = tabControl.FindResource("TabHeader") as DataTemplate;

            // add controls to tab item, this case I added just a textbox
            TextBox txt = new TextBox();
            txt.Name = "txt";
            DocumentView dv = new DocumentView();
            tab.Content = dv;

            // insert tab item right before the last (+) tab item
            _tabItems.Insert(count -1, tab);
            return tab;*/
            return null;
        } 

        // Should not be needed.
        private void document_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*TabItem tab = tabControl.SelectedItem as TabItem;

            if (tab != null && tab.Header != null)
            {
                if (tab.Header.Equals("+"))
                {
                    // clear tab control binding
                    tabControl.DataContext = null;

                    // add new tab
                    TabItem newTab = this.AddTabItem();

                    // bind tab control
                    tabControl.DataContext = _tabItems;

                    // select newly added tab item
                    tabControl.SelectedItem = newTab;
                }
                else
                {
                    // your code here...
                }
            }*/
        }

        private void btnCloseTab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var docName = button.Tag;
                Document docToClose = _mainWindowViewModel.DocumentManager.DocumentList.Single(d => d.DocumentName == docName);
                docToClose.Persist();
                ((ObservableCollection<Document>) documentTabControl.ItemsSource).Remove(docToClose);
            }

            //string tabName = (sender as Button).CommandParameter.ToString();

            /*
            var item = tabControl.Items.Cast<TabItem>().Where(i => i.Name.Equals(tabName)).SingleOrDefault();

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
                    TabItem selectedTab = tabControl.SelectedItem as TabItem;

                    // clear tab control binding
                    tabControl.DataContext = null;

                    _tabItems.Remove(tab);

                    // bind tab control
                    tabControl.DataContext = _tabItems;

                    // select previously selected tab. if that is removed then select first tab
                    if (selectedTab == null || selectedTab.Equals(tab))
                    {
                        selectedTab = _tabItems[0];
                    }
                    tabControl.SelectedItem = selectedTab;
                }
            }*/
        }

        private void mnuOpen_Click(object sender, RoutedEventArgs e)
        {
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
