using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KirbySqueakSquadRandomizer.Model_older
{
    class Level
    {
        public string Id { get; set; }
        public string World { get; set; }
        public string Stage { get; set; }
        public string Adress { get; set; }
        public string[] RequiredItems { get; set; }
    }
}
