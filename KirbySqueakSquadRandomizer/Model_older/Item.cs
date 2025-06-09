using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KirbySqueakSquadRandomizer.Model_older
{
    class Item
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public string ItemId { get; set; }
        public string ProgressionType { get; set; }
        public string LevelId { get; set; }
        public string NewLevelId { get; set; }
        public string[] RequiredPowers { get; set; }

        public Item(Item item)
        {
            Name = item.Name;
            Adress = item.Adress;
            ItemId = item.ItemId;
            ProgressionType = item.ProgressionType;
            LevelId = item.LevelId;
            NewLevelId = item.NewLevelId;
            RequiredPowers = item.RequiredPowers;
        }

        public Item() { }
    }
}
