using System;

namespace INIManagerProject.Model
{
    internal class ValueNode
    {
        private Edit _fatherEdit;
        private String _keyName;
        private String _valueData;
        private uint _editId;

        public string KeyName { get => _keyName; set => _keyName = value; }
        public string ValueData { get => _valueData; set => _valueData = value; }
        public uint EditId { get => _editId; set => _editId = value; }
        internal Edit FatherEdit { get => _fatherEdit; set => _fatherEdit = value; }
    }
}