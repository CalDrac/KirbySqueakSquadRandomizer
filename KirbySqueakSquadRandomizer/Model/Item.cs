using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KirbySqueakSquadRandomizer.Model
{
    class Item
    {
        private string name;
        private string itemId1;

        public string Name { get => name; set => name = value; }
        public string Adress { get; set; }
        public string ItemId { get => itemId1; set => itemId1 = value; }
        public string progressionType { get; set; }
    }
}
