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
    /// <summary>
    /// Contains the list of Edits and methods to manage them.
    /// There is only one list for each Document.
    /// Order and status of the edits are stored in Profiles, not in EditList.
    /// </summary>
    class EditListModel
    {
        #region Fileds

        /// <summary>
        /// Dictionary of all edits EditId->Edit
        /// </summary>
        private Dictionary<int, Edit> _editMapById;
        /// <summary>
        /// Dictionary of all edits EditName->Edit
        /// </summary>
        private Dictionary<string, Edit> _editMapByName;

        #endregion

        #region Properties

        internal Document Document { get; }
        internal IdBroker IdBroker { get; }
        internal string EditsFolder { get; private set; }
        internal Dictionary<int, Edit> EditMapById { get => _editMapById; }
        internal Dictionary<string, Edit> EditMapByName { get => _editMapByName; }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor, generates the Edits folder if it does not exist.
        /// To populate the list use LoadEditsFromDisk()
        /// </summary>
        /// <param name="document"></param>
        public EditListModel(Document document)
        {
            Document = document;
            _editMapById = new Dictionary<int, Edit>();
            _editMapByName = new Dictionary<string, Edit>();
            IdBroker = new IdBroker();

            EditsFolder = Path.Combine(Document.DocumentFolderPath, "Edits");
            if (!Directory.Exists(EditsFolder))
            {
                Directory.CreateDirectory(EditsFolder);
            }
        }

        /// <summary>
        /// Will create and add edits found in the edits folder to the lists.
        /// </summary>
        public void LoadEditsFromDisk()
        {
            string[] subdirs = Directory.GetDirectories(EditsFolder)
                             .Select(Path.GetFileName)
                             .ToArray();
            if (subdirs == null || subdirs.Length < 1)
            {
                // TODO: Move Base file to Document isntead of editList.
                CreateBaseFileEdit();
            }
            else
            {
                foreach (var editDirName in subdirs)
                {
                    CreateEditFromDisk(editDirName);
                }
            }
        }

        #endregion

        #region PrivateMethods

        // TODO: Move Base file to Document isntead of editList.
        private Edit CreateBaseFileEdit()
        {
            var baseEditFolder = Path.Combine(EditsFolder, "Base File");
            if (!Directory.Exists(baseEditFolder))
            {
                Directory.CreateDirectory(baseEditFolder);
            }
            File.Copy(Document.ManagedFilePath, Path.Combine(baseEditFolder, "Base File.ini"), overwrite: true);
            return CreateEditFromDisk("Base File");
        }

        /// <summary>
        /// Loads an edit with a given name from the EditsFolder and adds
        /// it to the lists.
        /// Will generate a new one if missing on disk.
        /// </summary>
        /// <param name="editDirName"></param>
        /// <returns>The loaded edit</returns>
        private Edit CreateEditFromDisk(string editDirName)
        {
            int newId = IdBroker.NextId;
            var loadedEdit = new Edit(newId, editDirName, Document);
            _editMapById.Add(newId, loadedEdit);
            _editMapByName.Add(editDirName, loadedEdit);
            loadedEdit.UpdateFromDisk();
            return loadedEdit;
        }

        #endregion

    }
}
