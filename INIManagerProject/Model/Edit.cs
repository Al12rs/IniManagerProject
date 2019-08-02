using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    class Edit
    {
        private int _editId;
        private String _editName;
        private String _rawContent;
        /// <summary>
        /// keys of the merge structure, only populated if edit is active.
        /// </summary>
        private List<KeyNode> affectedKeys;

        public Edit(int editId, string editName)
        {
            _editId = editId;
            _editName = editName;
        }
    }
}
