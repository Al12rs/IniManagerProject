using System.Collections.Generic;

namespace INIManagerProject.Model
{
    public class Section
    {
        private string SectionName { get; set; }
        private Dictionary<string,KeyNode> _keycollection;

        public Dictionary<string, KeyNode> KeyCollection { get => _keycollection; set => _keycollection = value; }
        public bool IsGlobal { get; set; }

        public Section(string sectionName)
        {
            SectionName = sectionName;
            IsGlobal = false;
        }

        public Section(bool isGlobal)
        {
            IsGlobal = isGlobal;
        }

        public void Clear()
        {
            KeyCollection.Clear();
        }

        public void AddValue(ValueNode value)
        {
            if (!KeyCollection.ContainsKey(value.KeyName))
            {
                KeyCollection.Add(value.KeyName, new KeyNode(this, value.KeyName));
            }
            KeyCollection[value.KeyName].AddValue(value, sort: false);
        }
    }
}