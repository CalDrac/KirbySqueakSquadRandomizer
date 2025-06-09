using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbySqueakSquadRandomizer.Model
{
    internal class Stage
    {
        public int number {  get; set; }
        public int world {  get; set; }
        public List<ItemV2> items { get; set; }
        public List<ItemV2> requiredItems { get; set; }
        public string adress { get; set; }

        public Stage(int world,int number, string adress, List<ItemV2> items, List<ItemV2> requiredItems)
        {
            this.world = world;
            this.number = number;
            this.world = world;
            this.items = items;
            this.requiredItems = requiredItems;
        }
    }
}
