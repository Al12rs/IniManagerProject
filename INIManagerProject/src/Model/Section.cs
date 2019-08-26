using System.Collections.Generic;

namespace INIManagerProject.Model
{
    public class Section
    {
        private string SectionName { get; set; }
        private List<KeyNode> _keyNodeList;

        public List<KeyNode> KeyNodeList { get => _keyNodeList; set => _keyNodeList = value; }

        public Section(string sectionName)
        {
            SectionName = sectionName;
        }
    }
}