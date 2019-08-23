using INIManagerProject.src.Model;
using System.Collections.Generic;

namespace INIManagerProject.Model
{
    internal class MergeStructure : IRawContentProvider
    {
        #region Fields

        private string _rawContent;
        private SectionCollection _sectionCollection;

        #endregion Fields

        #region Properties

        internal SectionCollection SectionCollection { get => _sectionCollection; private set => _sectionCollection = value; }
        internal Document Document { get; private set; }
        public string RawContent { get => _rawContent; set { } }

        #endregion Properties

        #region Initialization

        public MergeStructure(Document doc)
        {
            Document = doc;
        }

        #endregion Initialization

        #region PublicMethods

        public void PopulateStructure()
        {
            // Iterate Edits.
            foreach (KeyValuePair<Edit, bool> editStatusPair in Document.ProfileManager.CurrentProfile.PriorityList)
            {
                // If edit is active.
                if (editStatusPair.Value)
                {
                    // TODO: also handle keys without sections.
                    var editSectionEnumerator = editStatusPair.Key.ParsedData.Sections.GetEnumerator();
                    Section currentSection;
                    string currentSectionName;
                    while (editSectionEnumerator.MoveNext())
                    {
                        currentSectionName = editSectionEnumerator.Current.SectionName;

                        if (!SectionCollection.SectionsDictionary.ContainsKey(currentSectionName))
                        {
                            currentSection = new Section(currentSectionName);
                            SectionCollection.SectionsDictionary.Add(currentSectionName, currentSection);
                        }
                        else
                        {
                            currentSection = SectionCollection.SectionsDictionary[currentSectionName];
                        }
                    }
                }
            }
        }

        #endregion PublicMethods
    }
}