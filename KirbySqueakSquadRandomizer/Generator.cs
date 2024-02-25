﻿using KirbySqueakSquadRandomizer.Model;
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
            var items = LoadJson<Item>("Data\\item_data.json",new Item());
            var newItems = LoadJson<Item>("Data\\item_data.json", new Item());
            var locations = LoadJson<Location>("Data\\item_source.json", new Location());
            var regions = LoadJson<Region>("Data\\world_path.json", new Region());

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
                    MessageBox.Show("ERROR GENERATION");
                    throw new Exception();
                }
            }
            //placement du reste des items
            for (int i = 0; i < items.Count; i++) {
                var indexToModify = newItems.IndexOf(newItems.First(it => it.Name == randLocations[i].name));
                newItems[indexToModify].Name = items[i].Name;
                newItems[indexToModify].ItemId = items[i].ItemId;
            }

            //remplacement des ID des newItems aux adresses des oldItems
            byte[] otherData = File.ReadAllBytes(opt.path);

            foreach (var item in newItems)
            {
                var adr = Convert.ToInt32(item.Adress, 16);
                var val = Convert.ToByte(item.ItemId, 16);
                otherData[adr] = val;
            }
            /*
            if (opt.isBossRandomized) { 
                //randomize boss locations sauf W8 ?
            }
            */
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
