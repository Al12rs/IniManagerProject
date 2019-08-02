using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    class INIManagerApplication
    {
        private DocumentManager _documentManager;
        private String _applicationAppdataFolder;

        public string ApplicationAppdataFolder { get => _applicationAppdataFolder; set => _applicationAppdataFolder = value; }
        internal DocumentManager DocumentManager { get => _documentManager; }

        public INIManagerApplication()
        {
            var appdataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ApplicationAppdataFolder = Path.Combine(appdataLocal, "INIManager");
            if (!Directory.Exists(ApplicationAppdataFolder))
            {
                Directory.CreateDirectory(ApplicationAppdataFolder);
            }
        }

        public void start()
        {
            _documentManager = new DocumentManager();
        }

       
    }
}
