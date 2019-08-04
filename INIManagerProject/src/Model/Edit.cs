using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using IniParser.Parser;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Rappresents a gorup of ini lines entries to be added to the mergeTree.
    /// </summary>
    class Edit
    {
        #region Fields
        /// <summary>
        /// contins the raw ini lines and possibly comments of the edit.
        /// </summary>
        private string _rawContent;

        #endregion

        #region Properties

        public IniData ParsedData { get; private set; }
        public string EditSourceFile { get; private set; }
        public string EditFolderPath { get; private set; }
        public string RawContent { get => _rawContent; private set => _rawContent = value; }
        public int EditId { get; private set; }
        public string EditName { get; set; }
        internal Document Document { get; private set; }
        /// <summary>
        /// These are keyNodes from the mergestructure that contain values added by this 
        /// edit, for fast access.
        /// Needs to be considered if worth it.
        /// </summary>
        internal List<KeyNode> AffectedKeys { get; set; }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor, will create the Edit directory using Edit name if not existent.
        /// Does not load data from disk, use UpdateFromDisk() to populate the generated Edit.
        /// </summary>
        /// <param name="editId"></param>
        /// <param name="editName"></param>
        /// <param name="doc"></param>
        public Edit(int editId, string editName, Document doc)
        {
            EditId = editId;
            EditName = editName;
            Document = doc;
            EditFolderPath = Path.Combine(Document.EditListModel.EditsFolder, EditName);
            if (!Directory.Exists(EditFolderPath))
            {
                Directory.CreateDirectory(EditFolderPath);
            }
            EditSourceFile = Path.Combine(EditFolderPath, EditName + ".ini");
        }

        /// <summary>
        /// Loads edit data from disk or generates the edit file if it does not exist.
        /// Parses the data.
        /// </summary>
        /// <returns></returns>
        public bool UpdateFromDisk()
        {
            if (!File.Exists(EditSourceFile))
            {
                File.Create(EditSourceFile).Dispose();
            }

            RawContent = File.ReadAllText(EditSourceFile);

            var parser = new IniDataParser();
            parser.Configuration.ThrowExceptionsOnError = false;
            IniData ParsedData = parser.Parse(RawContent);
            if (ParsedData == null)
            {
                return false;
            }
            return true;
            // TODO: Possibly update directoryTree. Possibly not.
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// Saves rawContent to the editSourceFile.
        /// </summary>
        public void PersistEdit()
        {
            File.WriteAllText(EditSourceFile, RawContent);
        }

        #endregion
    }
}
