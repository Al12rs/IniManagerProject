using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIManagerProject.Model.Utils
{
    class Pair<KeyT, ValueT>
    {
        public KeyT Key { get; set; }
        public ValueT Value { get; set; }

        public Pair() { }

        public Pair(KeyT key, ValueT value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
