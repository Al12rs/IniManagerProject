using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using INIManagerProject.Model;

namespace INIManagerProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private INIManagerApplication _iniApplication;

        internal INIManagerApplication IniApplication { get => _iniApplication; set => _iniApplication = value; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _iniApplication = new INIManagerApplication();
            _iniApplication.start();
        }
    }
}
