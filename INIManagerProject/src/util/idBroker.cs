using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.util
{
    class IdBroker
    {
        private int _nextId;

        public IdBroker()
        {
            _nextId = 0;
        }

        public int NextId { get { return _nextId++; } }
    }
}
