using IniParser.Model;
using IniParser.Parser;
using System.Collections.Generic;
using System.Linq;

namespace INIManagerProject.Model
{
    public class MergeStructure : ViewModelBase, IRawContentProvider
    {
        #region Fields

        private string _rawContent;
        private Section _globalSection;
        private Dictionary<string, Section> _sectionCollection;
        private IniData _parsedData;
        private IniDataParser _iniParser;

        #endregion Fields

        #region Properties

        public Dictionary<string, Section> SectionCollection { get => _sectionCollection; private set => _sectionCollection = value; }
        public Document Document { get; private set; }

        public string RawContent
        {
            get => _rawContent;
            set
            {
            }
        }

        public Section GlobalSection { get => _globalSection; private set => _globalSection = value; }
        public IniData ParsedData { get => _parsedData; set => _parsedData = value; }

        #endregion Properties

        #region Initialization

        public MergeStructure(Document doc)
        {
            Document = doc;
            SectionCollection = new Dictionary<string, Section>();
            // Initializing global section using boolean.
            GlobalSection = new Section(isGlobal: true);
            _rawContent = "";
            _parsedData = null;
            _iniParser = new IniDataParser();
            _iniParser.Configuration.ThrowExceptionsOnError = false;
        }

        #endregion Initialization

        #region PublicMethods

        public void PopulateStructure()
        {
            // Clearing previous stored data.
            SectionCollection.Clear();
            GlobalSection.Clear();

            // Iterate Edits in descending priority order so the values are added already in order.
            foreach (Edit currentEdit in Document.EditListModel.ModelList.OrderByDescending(e => e.PriorityCache))
            {
                // If edit is inactive skip it.
                if (currentEdit.StatusCache == false)
                {
                    continue;
                }

                foreach (ValueNode currentValue in currentEdit.Values)
                {
                    this.AddValue(currentValue);
                }
            }
        }

        public void CalculateMergeResult()
        {
            var sortedEdits = Document.EditListModel.ModelList.OrderBy(e => e.PriorityCache);
            IniData mergeResult = new IniData();
            foreach (Edit edit in sortedEdits)
            {
                if (edit.StatusCache)
                {
                    if (edit.ParsedData != null)
                    {
                        mergeResult.Merge(edit.ParsedData);
                    }
                }
            }
            ParsedData = mergeResult;
            _rawContent = mergeResult.ToString();
        }

        #endregion PublicMethods

        #region PrivateMethods

        private void AddValue(ValueNode value)
        {
            if (value.isGlobalSection)
            {
                GlobalSection.AddValue(value);
            } else
            {
                if (!SectionCollection.ContainsKey(value.SectionName))
                {
                    SectionCollection.Add(value.SectionName, new Section(value.SectionName));
                }
                SectionCollection[value.SectionName].AddValue(value);
            }
        }
        #endregion
    }
}