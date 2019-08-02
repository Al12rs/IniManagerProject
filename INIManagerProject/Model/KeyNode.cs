using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    class KeyNode
    {
        private Section _fatherSection;
        private String _keyName;
        private List<ValueNode> _alternativeValues;
        internal Section FatherSection { get => _fatherSection; set => _fatherSection = value; }

    }
}
