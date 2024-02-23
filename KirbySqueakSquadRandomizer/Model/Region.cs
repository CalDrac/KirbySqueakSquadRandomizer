using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbySqueakSquadRandomizer.Model
{
    class Region
    {
        public string fromId { get; set; }
        public string toId { get; set; }
        public List<string> requiredItems { get; set; }

    }
}
