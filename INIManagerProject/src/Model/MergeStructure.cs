using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INIManagerProject.src.Model;

namespace INIManagerProject.Model
{
    class MergeStructure
    {
        #region Fields
        private SectionCollection _sectionCollection;
        #endregion

        #region Properties
        internal SectionCollection SectionCollection { get => _sectionCollection; private  set => _sectionCollection = value; }
        internal Document Document { get; private set; }
        #endregion

        #region Initialization
        public MergeStructure(Document doc)
        {
            Document = doc;
        }
        #endregion

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

        #endregion


    }
}
