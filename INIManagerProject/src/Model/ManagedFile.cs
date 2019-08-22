using IniParser;
using IniParser.Model;
using IniParser.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    public class ManagedFile : ViewModelBase , IRawContentProvider
    {
        private string _rawContent;

        public IniData ParsedData { get; private set; }
        public string RawContent
        {
            get => _rawContent;
            set
            {
                if (_rawContent != value)
                {
                    _rawContent = value;
                    this.Persist();
                }
            }
        }
        public string ManagedFilePath { get; set; }

        public ManagedFile()
        {
        }

        public void Initialize()
        {
            if (File.Exists(ManagedFilePath))
            {
                RawContent = File.ReadAllText(ManagedFilePath);
                var parser = new IniDataParser();
                ParsedData = parser.Parse(RawContent);
            }
        }

        public void Persist()
        {
            if (RawContent == null)
                return;
            File.WriteAllText(ManagedFilePath, RawContent);
        }
    }
}
