using System;

namespace INIManagerProject.Model
{
    public class ValueNode
    {
        #region Fields

        private Edit _fatherEdit;
        private string _sectionName;
        private string _keyName;
        private string _valueData;

        #endregion

        #region Properties

        public string SectionName { get => _sectionName; set => _sectionName = value; }
        public string KeyName { get => _keyName; set => _keyName = value; }
        public string ValueData { get => _valueData; set => _valueData = value; }
        public Edit FatherEdit { get => _fatherEdit; set => _fatherEdit = value; }
        public bool isGlobalSection { get; set; }

        #endregion

        #region Initialization

        public ValueNode(Edit parent, string sectionName, string keyName, string valueData)
        {
            _fatherEdit = parent;
            if(sectionName == null)
            {
                isGlobalSection = true;
            } else
            {
                isGlobalSection = false;
                _sectionName = sectionName;
            }
            _keyName = keyName;
            _valueData = ValueData;
        }

        #endregion
    }
}