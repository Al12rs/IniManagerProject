using INIManagerProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace INIManagerProject.src.ViewModel
{

    

    class MainWindowViewModel
    {
        private DocumentManager documentManager;

        public MainWindowViewModel()
        {
            DocumentManager = ((App)Application.Current).IniApplication.DocumentManager;
            
        }

        public DocumentManager DocumentManager { get => documentManager; set => documentManager = value; }
    }
}
