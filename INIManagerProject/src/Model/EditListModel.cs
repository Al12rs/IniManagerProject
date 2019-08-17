using INIManagerProject.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Contains the list of Edits and methods to manage them.
    /// There is only one list for each Document.
    /// Order and status of the edits are stored in Profiles, not in EditList.
    /// </summary>
    internal class EditListModel
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

        #endregion Fileds

        #region Properties

        internal Document Document { get; }
        internal IdBroker IdBroker { get; }
        internal string EditsFolder { get; private set; }
        internal Dictionary<int, Edit> EditMapById { get => _editMapById; }
        internal Dictionary<string, Edit> EditMapByName { get => _editMapByName; }
        internal Edit BaseFileEdit { get; private set; }

        #endregion Properties

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
        /// Base File will either be loaded or created anew if missing.
        /// </summary>
        public void LoadEditsFromDisk()
        {
            string[] subdirs = Directory.GetDirectories(EditsFolder)
                             .Select(Path.GetFileName)
                             .ToArray();
            if (subdirs == null || subdirs.Length < 1)
            {
                CreateBaseFileFromManagedFile();
            }
            else
            {
                foreach (var editDirName in subdirs)
                {
                    // Don't load base file into the editLists.
                    if (editDirName != "Base File")
                    {
                        CreateEditFromDisk(editDirName);
                    }
                }
                if (BaseFileEdit == null)
                {
                    LoadBaseFileFromDisk();
                }
            }
        }

        #endregion Initialization

        #region PublicMethods

        /// <summary>
        /// Perists all edits and Base Edit
        /// </summary>
        public void Persist()
        {
            BaseFileEdit.Persist();
            foreach (var edit in _editMapById.Values)
            {
                edit.Persist();
            }
        }

        #endregion PublicMethods

        #region PrivateMethods

        /// <summary>
        /// Generate a new baseFileEdit from the contents of the file to manage.
        /// </summary>
        /// <returns></returns>
        private Edit CreateBaseFileFromManagedFile()
        {
            var baseEditFolder = Path.Combine(EditsFolder, "Base File");
            if (!Directory.Exists(baseEditFolder))
            {
                Directory.CreateDirectory(baseEditFolder);
            }
            File.Copy(Document.ManagedFilePath, Path.Combine(baseEditFolder, "Base File.ini"), overwrite: true);
            int newId = IdBroker.NextId;
            var baseEdit = new Edit(newId, "Base File", Document);
            baseEdit.UpdateFromDisk();
            BaseFileEdit = baseEdit;
            return baseEdit;
        }

        /// <summary>
        /// Loads the BaseFile from the folder, if it does not exist it is generated empty.
        /// </summary>
        /// <returns></returns>
        private Edit LoadBaseFileFromDisk()
        {
            var baseEditFolder = Path.Combine(EditsFolder, "Base File");
            if (!Directory.Exists(baseEditFolder))
            {
                Directory.CreateDirectory(baseEditFolder);
            }
            var BaseFileSourcePath = Path.Combine(baseEditFolder, "Base File.ini");
            if (!File.Exists(BaseFileSourcePath))
            {
                File.Create(BaseFileSourcePath).Dispose();
            }
            int newId = IdBroker.NextId;
            var baseEdit = new Edit(newId, "Base File", Document);
            baseEdit.UpdateFromDisk();
            BaseFileEdit = baseEdit;
            return baseEdit;
        }

        /// <summary>
        /// Loads an edit with a given name from the EditsFolder and adds
        /// it to the lists.
        /// Will generate a new one if missing on disk.
        /// </summary>
        /// <param name="editDirName"></param>
        /// <returns>The loaded edit</returns>
        private Edit CreateEditFromDisk(string editName)
        {
            int newId = IdBroker.NextId;
            var loadedEdit = new Edit(newId, editName, Document);
            _editMapById.Add(newId, loadedEdit);
            _editMapByName.Add(editName, loadedEdit);
            loadedEdit.UpdateFromDisk();
            return loadedEdit;
        }

        #endregion PrivateMethods
    }
}