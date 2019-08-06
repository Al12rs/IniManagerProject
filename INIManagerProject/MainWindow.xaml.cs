﻿using Microsoft.Win32;
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

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                // initialize tabItem array
                _tabItems = new List<TabItem>();

                // add a tabItem with + in header 
                TabItem tabAdd = new TabItem();
                tabAdd.Header = "+";

                _tabItems.Add(tabAdd);

                // add first tab
                this.AddTabItem();

                // bind tab control
                tabDynamic.DataContext = _tabItems;

                tabDynamic.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuOpen_Click(object sender, RoutedEventArgs e)
        {

            // Placeholder: hardcoded to open skyrim.ini document.
            ((App)Application.Current).IniApplication.DocumentManager.CreateAndLoadDocumentFromName("Skyrim");


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
