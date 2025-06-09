using KirbySqueakSquadRandomizer.Model;
using Microsoft.WindowsAPICodePack.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbySqueakSquadRandomizer.Utils
{
    internal class ItemUtils
    {

        public static List<ItemV2> createNewListItems()
        {
            ItemV2 item1 = new ItemV2("Beginning Notes", "34", "107EA5D");
            ItemV2 item2 = new ItemV2("Fire scroll", "1C", "10A748D");
            ItemV2 item3 = new ItemV2("Sound player", "05", "10AC09D");
            ItemV2 item4 = new ItemV2("Green", "48", "10D085D");
            ItemV2 item5 = new ItemV2("Graphic piece_1", "64", "10D50CD");
            ItemV2 item6 = new ItemV2("Prism Plains key", "0E", "10D9C2D");
            ItemV2 item7 = new ItemV2("Copy palette 1", "58", "10E9C8D");
            ItemV2 item8 = new ItemV2("King DeDeDe badge", "3E", "114162D");
            ItemV2 item9 = new ItemV2("Graphic piece_2", "71", "112A01D");
            ItemV2 item10 = new ItemV2("Vitality half_1", "06", "1131CAD");
            ItemV2 item11 = new ItemV2("Animal scroll", "30", "146C45D");
            ItemV2 item12 = new ItemV2("Kirby's Sounds", "3A", "14732AD");
            ItemV2 item13 = new ItemV2("Star seal 1", "00", "147836D");
            ItemV2 item14 = new ItemV2("Citrus", "55", "14D941D");
            ItemV2 item15 = new ItemV2("Ghost medal_1", "15", "14DC85D");
            ItemV2 item16 = new ItemV2("Wheel scroll", "27", "1535B7D");
            ItemV2 item17 = new ItemV2("Copy palette 2", "5C", "153B25D");
            ItemV2 item18 = new ItemV2("Graphic piece_3", "6F", "15448CD");
            ItemV2 item19 = new ItemV2("Cutter scroll", "24", "15A827D");
            ItemV2 item20 = new ItemV2("Beam scroll", "1F", "162622D");
            ItemV2 item21 = new ItemV2("Graphic piece_4", "6B", "162985D");
            ItemV2 item22 = new ItemV2("Nature Notch key", "0F", "16336FD");
            ItemV2 item23 = new ItemV2("Mrs Moley badge", "3F", "1679E2D");
            ItemV2 item24 = new ItemV2("Graphic piece_5", "6D", "165981D");
            ItemV2 item25 = new ItemV2("Secret Map_1", "5D", "1665C2D");
            ItemV2 item26 = new ItemV2("Vitality half_2", "07", "16696CD");
            ItemV2 item27 = new ItemV2("Graphic piece_6", "72", "17AC6ED");
            ItemV2 item28 = new ItemV2("Grape", "4D", "17B5A7D");
            ItemV2 item29 = new ItemV2("Spark scroll", "1E", "17B98CD");
            ItemV2 item30 = new ItemV2("Familiar Notes", "38", "17DAA1D");
            ItemV2 item31 = new ItemV2("Star seal 2", "01", "17E5C8D");
            ItemV2 item32 = new ItemV2("Graphic piece_7", "6A", "1819A3D");
            ItemV2 item33 = new ItemV2("Secret Map_2", "5E", "1820E2D");
            ItemV2 item34 = new ItemV2("Ghost medal_2", "16", "18244BD");
            ItemV2 item35 = new ItemV2("Lavender", "57", "184821D");
            ItemV2 item36 = new ItemV2("HiJump scroll", "28", "18544BD");
            ItemV2 item37 = new ItemV2("Cushy Cloud key", "10", "185A1ED");
            ItemV2 item38 = new ItemV2("Tornado scroll", "20", "186AE1D");
            ItemV2 item39 = new ItemV2("Mecha-Kracko badge", "40", "18C803D");
            ItemV2 item40 = new ItemV2("Orange", "08", "189F72D");
            ItemV2 item41 = new ItemV2("Graphic piece_8", "4F", "18AA61D");
            ItemV2 item42 = new ItemV2("Vitality half_3", "75", "18ADE6D");
            ItemV2 item43 = new ItemV2("Graphic piece_9", "67", "18CA66D");
            ItemV2 item44 = new ItemV2("Shadow", "53", "18DEECD");
            ItemV2 item45 = new ItemV2("Bubble scroll", "31", "18E7CBD");
            ItemV2 item46 = new ItemV2("Yellow", "46", "19022AD");
            ItemV2 item47 = new ItemV2("Star seal 3", "02", "1906C9D");
            ItemV2 item48 = new ItemV2("Metal scroll", "32", "194471D");
            ItemV2 item49 = new ItemV2("Copy palette 3", "5A", "195BA7D");
            ItemV2 item50 = new ItemV2("Vitality half_4", "09", "19607BD");
            ItemV2 item51 = new ItemV2("Graphic piece_10", "76", "199904D");
            ItemV2 item52 = new ItemV2("Secret Map_3", "5F", "198BC6D");
            ItemV2 item53 = new ItemV2("Jam Jungle key", "11", "199903D");
            ItemV2 item54 = new ItemV2("Laser scroll", "25", "19A321D");
            ItemV2 item55 = new ItemV2("Yadgaine badge", "41", "1A0D02D");
            ItemV2 item56 = new ItemV2("Ice scroll", "1D", "19F18AD");
            ItemV2 item57 = new ItemV2("Party Notes", "33", "19F56AD");
            ItemV2 item58 = new ItemV2("Ghost medal_3", "17", "1A004FD");
            ItemV2 item59 = new ItemV2("Parasol scroll", "3B", "1A21C9D");
            ItemV2 item60 = new ItemV2("Enemy Sounds", "21", "1A264ED");
            ItemV2 item61 = new ItemV2("Star seal 4", "03", "1A2B10D");
            ItemV2 item62 = new ItemV2("Ivory", "54", "1A46C7D");
            ItemV2 item63 = new ItemV2("Graphic piece_11", "73", "1A4F6BD");
            ItemV2 item64 = new ItemV2("Hammer scroll", "22", "1A54F5D");
            ItemV2 item65 = new ItemV2("Graphic piece_12", "69", "1A6AE5D");
            ItemV2 item66 = new ItemV2("Ninja scroll", "2C", "1A6AE8D");
            ItemV2 item67 = new ItemV2("Vitality half_5", "0A", "1A6AE7D");
            ItemV2 item68 = new ItemV2("Secret Map_4", "60", "1AB462D");
            ItemV2 item69 = new ItemV2("Red", "47", "1AB769D");
            ItemV2 item70 = new ItemV2("Vocal Volcano key", "12", "1ABC50D");
            ItemV2 item71 = new ItemV2("Bohboh badge", "42", "1AFB06D");
            ItemV2 item72 = new ItemV2("Copy palette 4", "2A", "1AD8A2D");
            ItemV2 item73 = new ItemV2("Sleep scroll", "59", "1ADCC2D");
            ItemV2 item74 = new ItemV2("Ghost medal_4", "18", "1AE646D");
            ItemV2 item75 = new ItemV2("Sound Effects", "74", "1B2B2ED");
            ItemV2 item76 = new ItemV2("Graphic piece_13", "3C", "1B3681D");
            ItemV2 item77 = new ItemV2("Sword scroll", "2B", "1B3D85D");
            ItemV2 item78 = new ItemV2("Snow", "49", "1B5D61D");
            ItemV2 item79 = new ItemV2("Star seal 5", "04", "1B65FCD");
            ItemV2 item80 = new ItemV2("Fighter scroll", "2D", "1B932CD");
            ItemV2 item81 = new ItemV2("Graphic piece_14", "6E", "1BA6E9E");
            ItemV2 item82 = new ItemV2("Vitality half_6", "0B", "1BAED7D");
            ItemV2 item83 = new ItemV2("Happy Notes", "35", "1BB3ADD");
            ItemV2 item84 = new ItemV2("Chalk", "52", "1BCEC5D");
            ItemV2 item85 = new ItemV2("Ice Island key", "13", "1BDB2DD");
            ItemV2 item86 = new ItemV2("Graphic piece_15", "66", "1BF2C2D");
            ItemV2 item87 = new ItemV2("Cupid scroll", "23", "1C03C2D");
            ItemV2 item88 = new ItemV2("Copy palette 5", "5B", "1BFB42D");
            ItemV2 item89 = new ItemV2("Daroach badge", "43", "1C4782D");
            ItemV2 item90 = new ItemV2("Cherry", "51", "1C150ED");
            ItemV2 item91 = new ItemV2("Secret Map_5", "61", "1C3123D");
            ItemV2 item92 = new ItemV2("Ghost medal_5", "19", "1C34C2D");
            ItemV2 item93 = new ItemV2("White", "56", "1C678BD");
            ItemV2 item94 = new ItemV2("Secret Sounds", "3D", "1C5BE4D");
            ItemV2 item95 = new ItemV2("Throw scroll", "2E", "1C6BC7D");
            ItemV2 item96 = new ItemV2("Sapphire", "4C", "1C8EE8D");
            ItemV2 item97 = new ItemV2("Graphic piece_16", "6C", "1C996CD");
            ItemV2 item98 = new ItemV2("Ghost medal_6", "1A", "1C9DE2D");
            ItemV2 item99 = new ItemV2("Graphic piece_17", "36", "1CB04CD");
            ItemV2 item100 = new ItemV2("Spunky Notes", "65", "1CBB01D");
            ItemV2 item101 = new ItemV2("Bomb scroll", "26", "1CB5D5D");
            ItemV2 item102 = new ItemV2("Ocean", "4B", "1CDA49D");
            ItemV2 item103 = new ItemV2("Magic scroll", "2F", "1CE68FD");
            ItemV2 item104 = new ItemV2("Secret Sea key", "14", "1CEB8BD");
            ItemV2 item105 = new ItemV2("Chocolate", "50", "1CFBA2D");
            ItemV2 item106 = new ItemV2("Meta Knight badge", "44", "1D91E2D");
            ItemV2 item107 = new ItemV2("Secret Map_6", "62", "1D8CC2D");
            ItemV2 item108 = new ItemV2("Graphic piece_18", "68", "1D5D01D");
            ItemV2 item109 = new ItemV2("Vitality half_7", "0C", "1D6006D");
            ItemV2 item110 = new ItemV2("Graphic piece_19", "70", "1DF421D");
            ItemV2 item111 = new ItemV2("Battle Notes", "37", "1DBA88D");
            ItemV2 item112 = new ItemV2("UFO scroll", "29", "1DCA13D");
            ItemV2 item113 = new ItemV2("Emerald", "4E", "1E1D42D");
            ItemV2 item114 = new ItemV2("Secret Map_7", "63", "1E28C7D");
            ItemV2 item115 = new ItemV2("Ghost medal_7", "1B", "1E2C65D");
            ItemV2 item116 = new ItemV2("Carbon", "4A", "1E5A8AD");
            ItemV2 item117 = new ItemV2("Secret Notes", "39", "1E77A1D");
            ItemV2 item118 = new ItemV2("Vitality half_8", "0D", "1E3D88D");
            ItemV2 item119 = new ItemV2("Dark Nebula badge", "45", "1E98C2D");

            return new List<ItemV2> { item1, item2, item3, item4, item5, item6, item7, item8, item9, 
                item10, item11, item12, item13, item14, item15,item16,item17,item18,item19,
                item20, item21, item22, item23, item24, item25,item26,item27,item28,item29, 
                item30, item31, item32, item33, item34, item35, item36, item37, item38, item39,
                item40, item41, item42, item43, item44, item45, item46, item47, item48, item49,
                item50, item51, item52, item53, item54, item55, item56, item57, item58, item59,
                item60, item61, item62, item63, item64, item65, item66, item67, item68, item69,
                item70, item71, item72, item73, item74, item75, item76, item77, item78, item79,
                item80, item81, item82, item83, item84, item85, item86, item87, item88, item89,
                item90, item91, item92, item93, item94, item95, item96, item97, item98, item99,
                item100, item101, item102, item103, item104, item105, item106, item107, item108, item109,
                item110, item111, item112, item113, item114, item115, item116, item117, item118, item119
            };
        }
    }
}
