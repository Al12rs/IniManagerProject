using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    class Section
    {
        private string SectionName { get; set; }
        private List<KeyNode> _keyNodeList;

        internal List<KeyNode> KeyNodeList { get => _keyNodeList; set => _keyNodeList = value; }

        public Section(string sectionName)
        {
            SectionName = sectionName;
        }
    }
}
