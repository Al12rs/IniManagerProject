using INIManagerProject.Model.Utils;
using System;
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

        private ObservableCollection<Edit> _modelList;

        #endregion Fileds

        #region Properties

        public Document Document { get; }
        public IdBroker IdBroker { get; }
        public string EditsFolder { get; private set; }
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
            foreach (var edit in ModelList)
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

        /// <summary>
        /// Create a new empty edit with the passed name.
        /// If the passed name is alredy present it will fail and return null.
        /// </summary>
        /// <param name="editName"></param>
        /// <returns></returns>
        public Edit AddEdit(string editName)
        {
            if (ModelList.Any(e => e.EditName == editName))
            {
                return null;
            }
            Edit result = new Edit(IdBroker.NextId, editName, Document);
            result.SetStatus(false);
            result.UpdateFromDisk();
            var profileList = Document.ProfileManager.CurrentProfile.EditNamesAndStatusByPriority;
            result.PriorityCache = profileList.Count + 1;
            profileList.Add(new Pair<string, bool>(editName, false));
            ModelList.Add(result);
            return result;
        }


        /// <summary>
        /// Removes the edit with the passed name from the list and deletes the relative folder from appdata.
        /// </summary>
        /// <param name="editName"></param>
        /// <returns></returns>
        public bool RemoveEdit(string editName)
        {
            // Obtain Edit from name.
            Edit edit = ModelList.SingleOrDefault(e => e.EditName == editName);
            if (edit == null)
            {
                return false;
            }
            return RemoveEdit(edit);
        }

        /// <summary>
        /// Removes the passed edit from the list and deletes the relative folder from appdata.
        /// </summary>
        /// <param name="edit"></param>
        /// <returns></returns>
        public bool RemoveEdit(Edit edit)
        {
            var profileList = Document.ProfileManager.CurrentProfile.EditNamesAndStatusByPriority;
            profileList.RemoveAll(pair => pair.Key == edit.EditName);
            ModelList.Remove(edit);
            Directory.Delete(edit.EditFolderPath, recursive: true);
            return true;
        }

        /// <summary>
        /// Changes the priority of the passed edit and all other edits that may be pushed up or down.
        /// Also updates the Profile according to the change.
        /// </summary>
        /// <param name="targetEdit"></param>
        /// <param name="newPriority">Will be modified to reflect the actual final priority.</param>
        public void ChangeEditPriority(Edit targetEdit, ref int newPriority)
        {
            int oldPriority = targetEdit.PriorityCache;
            if (!targetEdit.IsRegular || oldPriority == newPriority)
            {
                // Don't change priority of Base File and do nothing if the new
                // prio is same as before.
                return;
            }

            if (newPriority < 1)
            {
                // Can't move edit before BaseFile.
                newPriority = 1;
            }

            foreach(Edit currentEdit in ModelList)
            {
                if (oldPriority > newPriority && newPriority <= currentEdit.PriorityCache && currentEdit.PriorityCache < oldPriority)
                {
                    // Moving an edit to lower priority and increasing priority of all edits
                    // that are between the new and old prios as they get pushed up.
                    currentEdit.PriorityCache += 1;
                } else 
                if (oldPriority < newPriority && oldPriority < currentEdit.PriorityCache && currentEdit.PriorityCache <= newPriority)
                {
                    // Moving an edit to higher priority and decreasing prio of all edits 
                    // that are between the old and new prios as they slide down.
                    currentEdit.PriorityCache -= 1;
                }
            }

            targetEdit.PriorityCache = newPriority;

            // Update the profile according to the change.
            Document.ProfileManager.CurrentProfile.UpdateFromModelList();
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
            ModelList.Add(loadedEdit);
            loadedEdit.UpdateFromDisk();
            return loadedEdit;
        }

        #endregion PrivateMethods
    }
}