using INIManagerProject.Model.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Contains the list of Edits and methods to manage them.
    /// There is only one list for each Document.
    /// Order and status of the edits are stored in Profiles, not in EditList.
    /// </summary>
    public class EditListModel
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

        private ObservableCollection<Edit> _modelList;

        #endregion Fileds

        #region Properties

        public Document Document { get; }
        public IdBroker IdBroker { get; }
        public string EditsFolder { get; private set; }
        public Dictionary<int, Edit> EditMapById { get => _editMapById; }
        public Dictionary<string, Edit> EditMapByName { get => _editMapByName; }
        public ObservableCollection<Edit> ModelList { get => _modelList; set => _modelList = value; }
        public Edit BaseFileEdit { get; private set; }

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
            _modelList = new ObservableCollection<Edit>();
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

        /// <summary>
        /// Applies the passed profile to the list of Edits,
        /// populating their priority and status caches.
        /// Assumes that the passed profile is the current profile as
        /// Document should be the one responsible for profile change.
        /// </summary>
        /// <param name="newProfile"></param>
        public void ApplyProfile(Profile newProfile)
        {
            newProfile.ClearDanglingEditNames();
            foreach (Edit edit in ModelList)
            {
                if (edit == BaseFileEdit)
                {
                    // Don't set the baseEdit priority or status as those are fixed.
                    continue;
                }
                // The profile list is ordered by priority.
                int index = newProfile.EditNamesAndStatusByPriority.FindIndex(pair => pair.Key == edit.EditName);
                if (index < 0)
                {
                    // Edit from ModelList is missing in the profile so we add
                    // it at the bottom disabled.
                    index = newProfile.EditNamesAndStatusByPriority.Count;
                    newProfile.EditNamesAndStatusByPriority.Add(new Pair<string, bool>(edit.EditName, false));
                }
                // We use the index + 1 because the 0 position is occupied by the BaseEdit.
                edit.PriorityCache = index + 1;
                edit.SetStatus(newProfile.EditNamesAndStatusByPriority[index].Value);
            }
            // The sorting is applied with CollectionViewSource class in viewModel using PriorityCache.
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
            File.Copy(Document.ManagedFile.ManagedFilePath, Path.Combine(baseEditFolder, "Base File.ini"), overwrite: true);
            int newId = IdBroker.NextId;
            var baseEdit = new Edit(newId, "Base File", Document);
            baseEdit.UpdateFromDisk();
            baseEdit.PriorityCache = 0;
            baseEdit.SetStatus(true);
            BaseFileEdit = baseEdit;
            ModelList.Add(baseEdit);
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
            baseEdit.PriorityCache = 0;
            baseEdit.SetStatus(true);
            BaseFileEdit = baseEdit;
            ModelList.Add(baseEdit);
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
            ModelList.Add(loadedEdit);
            loadedEdit.UpdateFromDisk();
            return loadedEdit;
        }

        #endregion PrivateMethods
    }
}