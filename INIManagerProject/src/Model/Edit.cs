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
    class Edit
    {
        private string _rawContent;

        public Edit(int editId, string editName, Document doc)
        {
            EditId = editId;
            EditName = editName;
            Document = doc;
            EditFolder = Path.Combine(Document.EditListModel.EditsFolder, EditName);
            EditSourceFile = Path.Combine(EditFolder, EditName + ".ini");
        }


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

            //TODO: Update mergeTree
        }

        public void PersistEdit()
        {
            File.WriteAllText(EditSourceFile, RawContent);
        }

        public IniData ParsedData { get; private set; }
        public string EditSourceFile { get; private set; }
        public string EditFolder { get; private set; }
        public string RawContent { get => _rawContent; private set => _rawContent = value; }
        public int EditId { get; private set; }
        public string EditName { get; set; }
        internal Document Document { get; private set; }
        internal List<KeyNode> AffectedKeys { get; set; }

    }
}
