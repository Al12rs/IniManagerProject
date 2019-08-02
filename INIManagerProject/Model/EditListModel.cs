using INIManagerProject.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model
{
    class EditListModel
    {
        private List<Edit> _fisicalEditList;
        private Document _document;
        private IdBroker _idBroker;


        public EditListModel(Document document)
        {
            _document = document;
            _fisicalEditList = new List<Edit>();
            _idBroker = new IdBroker();
        }

        public void initializeNewEditListModel()
        {
            //creating base file (initial undisablable Edit)
            var baseEdit = new Edit(_idBroker.NextId, "Base File");
            _fisicalEditList.Add(baseEdit);
        }

        internal Document Document { get => _document; }
    }
}
