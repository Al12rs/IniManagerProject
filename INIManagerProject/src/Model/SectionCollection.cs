using INIManagerProject.Model;
using System.Collections.Generic;

namespace INIManagerProject.Model
{
    /// <summary>
    /// Class modelling the collection of Sections in an ini file.
    /// It also contains a special section for keys without a section.
    /// </summary>
    public class SectionCollection
    {
        #region Fields

        /// <summary>
        /// Contains the keys that are not iside any section.
        /// </summary>
        private Section _globalSection;

        /// <summary>
        /// Section name- Section object
        /// </summary>
        private Dictionary<string, Section> _sectionsDictionary;

        #endregion Fields

        #region Properties

        public Dictionary<string, Section> SectionsDictionary { get => _sectionsDictionary; set => _sectionsDictionary = value; }
        public Section GlobalSection { get => _globalSection; set => _globalSection = value; }

        #endregion Properties

        #region Initialization

        public SectionCollection()
        {
            _sectionsDictionary = new Dictionary<string, Section>();
        }

        #endregion Initialization
    }
}