using INIManagerProject.Model;
using INIManagerProject.View;
using INIManagerProject.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace INIManagerProject.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _mainWindowViewModel;
        private readonly FieldInfo _menuDropAlignmentField;
        MainWindowViewModel MainWindowViewModel => _mainWindowViewModel;

        public MainWindow()
        {
            try
            {
                _menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
                System.Diagnostics.Debug.Assert(_menuDropAlignmentField != null);
                EnsureStandardPopupAlignment();
                SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;

                _mainWindowViewModel = new MainWindowViewModel();
                DataContext = _mainWindowViewModel;
                InitializeComponent();
                this.Title = "INI Manager " + getRunningVersion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            EnsureStandardPopupAlignment();
        }

        private void EnsureStandardPopupAlignment()
        {
            if (SystemParameters.MenuDropAlignment && _menuDropAlignmentField != null)
            {
                _menuDropAlignmentField.SetValue(null, false);
            }
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
                var dialog = new InputDialogue("Insert name of new Edit:");
                while (dialog.ShowDialog() == true)
                {
                    string docName = dialog.Answer;
                    if (docName == "" || _mainWindowViewModel.DocumentManager.DocumentList.Any(x => x.DocumentName == docName))
                    {
                        dialog = new InputDialogue("Name already in use or invalid, please select a different name: 1");
                        continue;
                    }
                    string folderPath;
                    try
                    {
                        folderPath = Path.Combine(_mainWindowViewModel.DocumentManager.DocumentsFolderPath, docName);

                    }
                    catch (Exception ex)
                    {
                        dialog = new InputDialogue("Name already in use or invalid, please select a different name: 2");
                        continue;
                    }

                    ((App)Application.Current).IniApplication.DocumentManager.CurrentDocument = ((App)Application.Current).IniApplication.DocumentManager.CreateNewDocument(openFileDialog.FileName, docName);
                    break;

                }
                
            }

        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            //Persist all settings.
            ((App)Application.Current).IniApplication.Persist();
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private Version getRunningVersion()
        {
            try
            {
                return ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            catch (Exception)
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }
    }
}
