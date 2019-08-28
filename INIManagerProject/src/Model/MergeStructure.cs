using INIManagerProject.Model;
using System.Linq;
using IniParser.Model;
using System.Collections.Generic;

namespace INIManagerProject.Model
{
    public class MergeStructure : ViewModelBase, IRawContentProvider
    {
        #region Fields

        private string _rawContent;
        private SectionCollection _sectionCollection;

        #endregion Fields

        #region Properties

        public SectionCollection SectionCollection { get => _sectionCollection; private set => _sectionCollection = value; }
        public Document Document { get; private set; }
        public string RawContent { get => _rawContent;
            set
            {
            }
        }
        public IniData ParsedData { get; set; }


        #endregion Properties

        #region Initialization

        public MergeStructure(Document doc)
        {
            Document = doc;
            _rawContent = "";
        }

        #endregion Initialization

        #region PublicMethods

        public void PopulateStructure()
        {
            // Iterate Edits.
            //foreach (KeyValuePair<Edit, bool> editStatusPair in Document.ProfileManager.CurrentProfile.PriorityList)
            //{
            //    // If edit is active.
            //    if (editStatusPair.Value)
            //    {
            //        // TODO: also handle keys without sections.
            //        var editSectionEnumerator = editStatusPair.Key.ParsedData.Sections.GetEnumerator();
            //        Section currentSection;
            //        string currentSectionName;
            //        while (editSectionEnumerator.MoveNext())
            //        {
            //            currentSectionName = editSectionEnumerator.Current.SectionName;

            //            if (!SectionCollection.SectionsDictionary.ContainsKey(currentSectionName))
            //            {
            //                currentSection = new Section(currentSectionName);
            //                SectionCollection.SectionsDictionary.Add(currentSectionName, currentSection);
            //            }
            //            else
            //            {
            //                currentSection = SectionCollection.SectionsDictionary[currentSectionName];
            //            }
            //        }
            //    }
            //}
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
    }
}