using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbySqueakSquadRandomizer.Model
{
    internal class ItemV2
    {
        public string name { get; set; }
        public string itemId { get; set; }
        public string adress { get; set; }
        public List<String> requiredPowers { get; set; }

        public ItemV2(string name,string itemId,string adress)
        {
            this.name = name;
            this.itemId = itemId;
            this.adress = adress;
            this.requiredPowers = new List<String>();
        }

        public ItemV2 (ItemV2 source)
        {
            this.name = source.name;
            this.itemId = source.itemId;
            this.adress = source.adress;
            this.requiredPowers = new List<String>();
        }
    }
}
