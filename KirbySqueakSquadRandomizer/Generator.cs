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
using System.Windows.Markup;

namespace KirbySqueakSquadRandomizer
{
    internal class Generator
    {
        public Generator() { }

        internal static void generateNewRomArchipelago(string server, string user, string password, string path)
        {
            //Connexion AP et remplacement des items
        }

        internal static Boolean generateNewRomClassic(Options opt)
        {
            return GenerateRom(opt);
        }

        private static Boolean GenerateRom(Options opt)
        {
            return randomize(opt);
        }

        static Boolean randomize(Options opt)
        {
            //get all items
            //get all locations
            //deposer les items requis en 1er 
            
            var staticItems = LoadJson<Item>("Data\\item_data.json", new Item());
            var items = LoadJson<Item>("Data\\item_data.json", new Item());
            var newItems = LoadJson<Item>("Data\\item_data.json", new Item());
            var locations = LoadJson<Location>("Data\\item_source.json", new Location());
            var regions = LoadJson<Region>("Data\\world_path.json", new Region());
            var bossLevels = LoadJson<BossLevel>("Data\\bossLevel_data.json", new BossLevel());
            
            var rnd = new Random();
            var randBossLevels = bossLevels.OrderBy(boss => rnd.Next()).ToList();
            var spoilerLog = "New boss".PadRight(20) + " | Old boss \n";
            byte[] otherData = File.ReadAllBytes(opt.path);
            if (opt.isBossW1_6Randomized)
            {
                for (int j = 0; j < randBossLevels.Count; j++)
                {
                    var bossLevel = randBossLevels[j];
                    String[] byteLink = bossLevel.dataBlock.Split(' ');
                    var adr = Convert.ToInt32(bossLevels[j].adress, 16);
                    spoilerLog += bossLevel.name.PadRight(20) + " | " + bossLevels[j].name + "\n";
                    locations.First(x => x.name.Contains(bossLevel.name)).nodeId = bossLevels[j].nodeId;
                    for (int i = 0; i < byteLink.Length; i++)
                    {
                        var val = Convert.ToByte(byteLink[i], 16);
                        otherData[adr + i] = val;
                    }
                }
            }
            spoilerLog += "\n\n";
            spoilerLog += "New item".PadRight(20) + " | Old item \n";
            List<String> reqItemsStr = getRequiredItems(regions);
            var requiredItems = items.Where(i => reqItemsStr.Contains(i.Name)).ToList();
            var randLocations = locations.OrderBy(item => rnd.Next()).ToList();
            //placement des items required
            for (int i = 0; i < requiredItems.Count; i++)
            {
                var bannedRegions = regions.Where(r => r.requiredItems.Contains(requiredItems[i].Name)).ToList();
                var bannedRegionsNames = new List<string>();
                foreach (var bannedRegion in bannedRegions)
                {
                    bannedRegionsNames.Add(bannedRegion.toId);
                }
                var validLocation = randLocations.First(l => !bannedRegionsNames.Contains(l.nodeId));
                if (validLocation != null)
                {
                    var indexToModify = newItems.IndexOf(newItems.First(it => it.Name == validLocation.name));
                    newItems[indexToModify].Name = requiredItems[i].Name;
                    newItems[indexToModify].ItemId = requiredItems[i].ItemId;
                    randLocations.Remove(validLocation);
                    items.Remove(requiredItems[i]);
                }
                else
                {
                    MessageBox.Show("ERROR GENERATION");
                    throw new Exception();
                }
            }
            //placement du reste des items
            for (int i = 0; i < items.Count; i++)
            {
                var indexToModify = newItems.IndexOf(newItems.First(it => it.Name == randLocations[i].name));
                newItems[indexToModify].Name = items[i].Name;
                newItems[indexToModify].ItemId = items[i].ItemId;
            }

            //remplacement des ID des newItems aux adresses des oldItems           
            foreach (var item in newItems)
            {
                string oldItemName = staticItems.First(x => x.Adress == item.Adress).Name;
                spoilerLog += item.Name.PadRight(20) + " | " + locations.First(x => x.name == oldItemName).nodeId + " - " + oldItemName + "\n";
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
            MessageBox.Show("File generated " + selectedFileName + "\\KSS_rando.nds");

            File.WriteAllBytes(selectedFileName + "\\KSS_rando_spoiler.txt", Encoding.ASCII.GetBytes(spoilerLog));
            return true;
        }

        private static List<string> getRequiredItems(List<Region> regions)
        {
            List<string> items = new List<string>();
            foreach (var region in regions)
            {
                foreach (var requiredItem in region.requiredItems)
                {
                    if (!items.Contains(requiredItem))
                    {
                        items.Add(requiredItem);
                    }

                }
            }

            return items;
        }

        public static List<T> LoadJson<T>(string file, T type)
        {
            List<T> returnList = new List<T>();
            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                returnList = JsonConvert.DeserializeObject<List<T>>(json);
            }
            return returnList;
        }

    }
}
