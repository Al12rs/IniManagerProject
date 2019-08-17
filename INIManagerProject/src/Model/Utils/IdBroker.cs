using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model.Utils
{
    /// <summary>
    /// Very simple class to generate session unique Ids.
    /// Ids are int and start from 0. Negative Ids can be considered invalid values.
    /// </summary>
    class IdBroker
    {
        private int _nextId;

        public IdBroker()
        {
            _nextId = 0;
        }

        /// <summary> Obtain a new Id unique for this IdBroker instance.</summary>
        public int NextId { get { return _nextId++; } }
    }
}
