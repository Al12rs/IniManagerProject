using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    class Section
    {
        private List<KeyNode> _keyNodeList;

        internal List<KeyNode> KeyNodeList { get => _keyNodeList; set => _keyNodeList = value; }
    }
}
