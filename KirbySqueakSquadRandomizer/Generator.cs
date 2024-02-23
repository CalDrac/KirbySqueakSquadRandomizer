using KirbySqueakSquadRandomizer.Model;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KirbySqueakSquadRandomizer
{
    internal class Generator
    {
        static List<Location> locations;
        static List<Item> items;
        static List<Region> regions;
        public Generator() { }

        internal static void generateNewRomArchipelago(string server, string user, string password, string path)
        {
            //Connexion AP et remplacement des items
        }

        internal static Boolean generateNewRomClassic(Options opt, string path)
        {
           return GenerateRom(path);
        }

        private static Boolean GenerateRom(string path)
        {
            return randomize(path);
        }

        static Boolean randomize(string path)
        {
            //get all items
            //get all locations
            //deposer les items de progression en 1er ?
            var oldItems = getAllItems();
            items = getAllItems();
            var newItems = getAllItems();
            locations = getAllLocations();
            regions = getAllRegions();
            var rnd = new Random();
            var randLocations = locations.OrderBy(item => rnd.Next()).ToList();

            List<String> reqItemsStr = getRequiredItems(regions);
            var requiredItems = items.Where(i=>reqItemsStr.Contains(i.Name)).ToList();
            //placement des items required
            for (int i = 0; i < requiredItems.Count; i++)
            {
                var bannedRegions = regions.Where(r => r.requiredItems.Contains(requiredItems[i].Name)).ToList();
                var bannedRegionsNames = new List<string>();
                foreach (var bannedRegion in bannedRegions)
                {
                    bannedRegionsNames.Add(bannedRegion.toId);
                }
                var validLocation = randLocations.First(l=>!bannedRegionsNames.Contains(l.nodeId));
                if(validLocation != null)
                {
                    var indexToModify = newItems.IndexOf(newItems.First(it=>it.Name == validLocation.name));
                    newItems[indexToModify].Name = requiredItems[i].Name;
                    newItems[indexToModify].ItemId = requiredItems[i].ItemId;
                    randLocations.Remove(validLocation);
                    items.Remove(requiredItems[i]);
                }
                else
                {
                    Console.WriteLine("ERROR GENERATION REQ ITEMS");
                    throw new Exception();
                }
            }
            //placement du reste des items
            for (int i = 0; i < items.Count; i++) {
                var indexToModify = newItems.IndexOf(newItems.First(it => it.Name == randLocations[i].name));
                newItems[indexToModify].Name = items[i].Name;
                newItems[indexToModify].ItemId = items[i].ItemId;
            }
            Console.WriteLine("GEN FINIE");

            //remplacement des ID des newItems aux adresses des oldItems
            byte[] otherData = File.ReadAllBytes(path);

            foreach (var item in newItems)
            {
                var adr = Convert.ToInt32(item.Adress, 16);
                var val = Convert.ToByte(item.ItemId, 16);
                otherData[adr] = val;
            }
            CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.IsFolderPicker = true;

            if (openFileDialog1.ShowDialog() != CommonFileDialogResult.Ok)
            {
                MessageBox.Show("Generation cancelled");
                return false;
            }
            

            string selectedFileName = openFileDialog1.FileName;
            File.WriteAllBytes(selectedFileName + "\\KSS_rando.nds", otherData);
            Console.WriteLine(otherData);

            MessageBox.Show("File generated");
            return true;
        }



        private static List<string> getRequiredItems(List<Region> regions)
        {
            List<string> items = new List<string>();

            foreach (var region in regions)
            {
                foreach (var requiredItem in region.requiredItems) {
                    if (!items.Contains(requiredItem)){
                        items.Add(requiredItem);
                    }
                        
                }
            }

            return items;
        }

        private static List<Item> getAllItems()
        {
            var items = new List<Item>();
            return LoadJsonItem("item_data.json");
            
        }

        private static List<Item> LoadJsonItem(string v)
        {
            using (StreamReader r = new StreamReader(v))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Item>>(json);
            }
        }

        private static List<Region> getAllRegions()
        {
            regions = new List<Region>();
            LoadJsonRegion("world_path.json");
            return regions;
        }

        private static void LoadJsonRegion(string v)
        {
            using (StreamReader r = new StreamReader(v))
            {
                string json = r.ReadToEnd();
                regions = JsonConvert.DeserializeObject<List<Region>>(json);
                Console.WriteLine(regions.Count);
            }
        }

        private static List<Location> getAllLocations()
        {
            locations = new List<Location>();
            LoadJsonLocation("item_source.json");
            return locations;
        }

        public static void LoadJsonLocation(string file)
        {
            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                locations = JsonConvert.DeserializeObject<List<Location>>(json);
                Console.WriteLine(locations.Count);
            }

        }

    }
}
