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
        public string KeyName { get => _keyName; set => _keyName = value; }
        public List<ValueNode> AlternativeValues { get => _alternativeValues; set => _alternativeValues = value; }
        public ValueNode WinningValue { get => _alternativeValues.Count > 0? _alternativeValues[0] : null; }

        public KeyNode(Section fatherSection, string keyName)
        {
            FatherSection = fatherSection;
            KeyName = keyName;
            _alternativeValues = new List<ValueNode>();
        }

        public void AddValue(ValueNode value, bool sort)
        {
            AlternativeValues.Add(value);
            if (sort)
            {
                // Sort the alternatives based on the priority of their origin Edits.
                // Note: The winning edit is the first of the list.
                _alternativeValues.Sort((val1, val2) => val1.FatherEdit.PriorityCache.CompareTo(val2.FatherEdit.PriorityCache));
            }
        }
    }
}