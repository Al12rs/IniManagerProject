using System;
using System.Collections.Generic;

namespace INIManagerProject.Model
{
    public class KeyNode
    {
        private Section _fatherSection;
        private String _keyName;
        private List<ValueNode> _alternativeValues;
        public Section FatherSection { get => _fatherSection; set => _fatherSection = value; }
    }
}