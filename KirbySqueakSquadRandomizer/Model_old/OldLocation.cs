using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KirbySqueakSquadRandomizer.Model
{
    class OldLocation
    {
        public string name { get; set; }
        public string type { get; set; }
        public string nodeId { get; set; }
        public string stage { get; set; }
        public List<string> requiredPowers { get; set; }
    }
}
