using INIManagerProject.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using System.IO;

namespace INIManagerProject.Model
{
    class EditListModel
    {
        //<editId, EditRef>
        private Dictionary<int, Edit> _editMapById;
        private Dictionary<string, Edit> _editMapByName;

        public EditListModel(Document document)
        {
            Document = document;
            _editMapById = new Dictionary<int, Edit>();
            _editMapByName = new Dictionary<string, Edit>();
            IdBroker = new IdBroker();

            EditsFolder = Path.Combine(Document.DocumentFolder, "Edits");
            if (!Directory.Exists(EditsFolder))
            {
                Directory.CreateDirectory(EditsFolder);
            }
        }

        public void initializeNewEditListModel()
        {
            //creating base file (initial undisablable Edit)
            int newId = IdBroker.NextId;
            
            var baseEditFolder = Path.Combine(EditsFolder, "Base File");
            if (!Directory.Exists(baseEditFolder))
            {
                Directory.CreateDirectory(baseEditFolder);
            }
            //take managed file as base file.
            File.Copy(Document.ManagedFilePath, Path.Combine(baseEditFolder, "Base File.ini"), overwrite: true);
            var baseEdit = new Edit(newId, "Base File", Document);
            baseEdit.UpdateFromDisk();
            _editMapById.Add(newId, baseEdit);
            _editMapByName.Add("Base File", baseEdit);

            //not updating profile since Document is doing that for us here.
        }

        public void LoadEditsFromDisk()
        {
            string[] subdirs = Directory.GetDirectories(EditsFolder)
                             .Select(Path.GetFileName)
                             .ToArray();
            foreach(var editDirName in subdirs)
            {
                int newId = IdBroker.NextId;
                var loadedEdit = new Edit(newId, editDirName, Document);
                _editMapById.Add(newId, loadedEdit);
                _editMapByName.Add(editDirName, loadedEdit);
                loadedEdit.UpdateFromDisk();
            }
        }

        internal Document Document { get; }
        internal IdBroker IdBroker { get; }
        internal string EditsFolder {get; private set;}
        internal Dictionary<int, Edit> EditMapById { get => _editMapById;  }
        internal Dictionary<string, Edit> EditMapByName { get => _editMapByName; }
    }
}
