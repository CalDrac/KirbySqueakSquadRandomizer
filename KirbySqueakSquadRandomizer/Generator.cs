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
        public static List<string> alreadyPickedPowers;
        public static List<Item> items;
        public static List<Item> newItems;
        public static List<Location> randLocations;

        public Generator()
        {

        }


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
            alreadyPickedPowers = new List<string>();
            items = LoadJson<Item>("Data\\item_data.json", new Item());
            newItems = LoadJson<Item>("Data\\item_data.json", new Item());
            var staticItems = LoadJson<Item>("Data\\item_data.json", new Item());
            //var items = LoadJson<Item>("Data\\item_data.json", new Item());
            //var newItems = LoadJson<Item>("Data\\item_data.json", new Item());
            var locations = LoadJson<Location>("Data\\item_source.json", new Location());
            var regions = LoadJson<Region>("Data\\world_path.json", new Region());
            var bossLevels = LoadJson<BossLevel>("Data\\bossLevel_data.json", new BossLevel());
            var monsters = LoadJson<Enemy>("Data\\enemy_data.json", new Enemy());

            var baseMonstersLocation = LoadJson<EnemyLocation>("Data\\enemy_location.json", new EnemyLocation());

            var rnd = new Random();
            var randBossLevels = bossLevels.OrderBy(boss => rnd.Next()).ToList();
            var randMonsterLocation = baseMonstersLocation.OrderBy(monster => rnd.Next()).ToList();

            var spoilerLog = "";
            byte[] otherData = File.ReadAllBytes(opt.path);
            if (opt.isPowerBlocking) //power blocking or evertything else for now
            {
                spoilerLog += "New item".PadRight(20) + " | Old item \n";
                List<String> reqItemsStr = getRequiredItems(regions);
                var requiredItems = items.Where(i => reqItemsStr.Contains(i.Name)).ToList();
                randLocations = locations.OrderBy(item => rnd.Next()).ToList();
                var pickedPowers = new List<string>();
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
                        //placer reqItem
                        var indexToModify = newItems.IndexOf(newItems.First(it => it.Name == validLocation.name));
                        newItems[indexToModify].Name = requiredItems[i].Name;
                        newItems[indexToModify].ItemId = requiredItems[i].ItemId;
                        randLocations.Remove(validLocation);
                        items.Remove(requiredItems[i]);
                        addScrollPower(bannedRegionsNames, validLocation);
                    }
                    else
                    {
                        MessageBox.Show("ERROR GENERATION");
                        throw new Exception();
                    }
                }
                //placement du reste des items
                while(items.Count > 0)
                {
                    var pickedLocation = randLocations.First();
                    var indexToModify = newItems.IndexOf(newItems.First(it => it.Name == pickedLocation.name));
                    newItems[indexToModify].Name = items.First().Name;
                    newItems[indexToModify].ItemId = items.First().ItemId;

                    if (items.First().Name.Contains("scroll")) 
                    {
                        alreadyPickedPowers.Add(items.First().Name.Replace(" scroll", ""));
                    }

                    randLocations.Remove(pickedLocation);
                    
                    items.Remove(items.First());
                    var pickedLocationReqPwrs = pickedLocation.requiredPowers;
                    pickedLocationReqPwrs.RemoveAll(rp => alreadyPickedPowers.Contains(rp));
                    if (pickedLocationReqPwrs.Count != 0)
                    {
                        addScrollPower(new List<string>(), pickedLocation);
                        //placer un scroll ?
                    }
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
            }
            else
            {
                if (opt.isMonsterRandomized)
                {
                    //randomize monsters
                    spoilerLog += "New monster".PadRight(20) + " | Old monster \n";

                    for (int i = 0; i < randMonsterLocation.Count; i++)
                    {
                        var monsterLocation = randMonsterLocation[i];
                        var baseMonster = baseMonstersLocation[i];
                        var adr = Convert.ToInt32(baseMonster.adress, 16);
                        otherData[adr] = Convert.ToByte(monsterLocation.monsterId, 16);
                        var firstMonster = monsters.First(m => m.monsterId == monsterLocation.monsterId).name.PadRight(20);
                        var secondMonster = monsters.First(m => m.monsterId == baseMonster.monsterId).name;
                        spoilerLog += firstMonster
                            + " | " + secondMonster + "\n";
                    }

                    spoilerLog += "-------------------------------------------------";
                    spoilerLog += "\n\n";
                }

                if (opt.isBossRandomized)
                {
                    spoilerLog += "New boss".PadRight(20) + " | Old boss \n";
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
                    spoilerLog += "-------------------------------------------------";
                    spoilerLog += "\n\n";
                }

                spoilerLog += "New item".PadRight(20) + " | Old item \n";
                List<String> reqItemsStr = getRequiredItems(regions);
                var requiredItems = items.Where(i => reqItemsStr.Contains(i.Name)).ToList();
                randLocations = locations.OrderBy(item => rnd.Next()).ToList();
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

        //Add scroll if a reqItem location has a reqPower
        private static void addScrollPower(List<string> bannedRegionsNames, Location validLocation)
        {
            //Get reqPwr
            var filteredPowers = new List<string>(validLocation.requiredPowers);
            //retirer alreadyPickedPowers
            filteredPowers.RemoveAll(p => alreadyPickedPowers.Contains(p));
            //Si > 0, on va placer un scroll
            if (filteredPowers.Count > 0)
            {
                //pick one power
                int randIndex = new Random().Next(filteredPowers.Count);
                var scroll = filteredPowers[randIndex];
                alreadyPickedPowers.Add(scroll);
                scroll += " scroll";
                Console.WriteLine("Placement : " + scroll);
                var itemScroll = items.First(l => l.Name == scroll);
                if (itemScroll != null)
                {
                    addScrollForScroll(bannedRegionsNames, filteredPowers, itemScroll);
                }
                else
                {
                    MessageBox.Show($"ERROR GENERATION on {scroll}");
                    throw new Exception();
                }

            }
            //Sinon rieng
            else
            {
                //rieng
            }
        }

        private static List<string> addScrollForScroll(List<string> bannedRegionsNames, List<string> filteredPowers, Item itemScroll)
        {
            var validLocationScrollList = randLocations.Where(l => !bannedRegionsNames.Contains(l.nodeId)).ToList();
            foreach (var validLocationScrollItem in validLocationScrollList)
            {
                filteredPowers = new List<string>(validLocationScrollItem.requiredPowers);
                if (filteredPowers.Count == 0)
                {
                    //place the scroll, no more scroll adding
                    setNewitem(itemScroll, validLocationScrollItem);
                    
                    break;
                }
                filteredPowers.RemoveAll(p => alreadyPickedPowers.Contains(p));

                if (filteredPowers.Count != 0)
                {
                    //place the scroll, pick a new power, start over
                    setNewitem(itemScroll, validLocationScrollItem);

                    int randIndex = new Random().Next(filteredPowers.Count);
                    var scroll = filteredPowers[randIndex];
                    alreadyPickedPowers.Add(scroll);
                    scroll += " scroll";

                    itemScroll = items.First(l => l.Name == scroll);
                    if (itemScroll != null)
                    {
                        addScrollForScroll(bannedRegionsNames, filteredPowers, itemScroll);
                    }
                    else
                    {
                        MessageBox.Show($"ERROR GENERATION on {scroll}");
                        throw new Exception();
                    }
                    break;
                }
            }

            return filteredPowers;
        }

        private static void setNewitem(Item itemScroll, Location? validLocationScrollItem)
        {
            var indexToModifyScroll = newItems.IndexOf(newItems.First(it => it.Name == validLocationScrollItem.name));
            newItems[indexToModifyScroll].Name = itemScroll.Name;
            newItems[indexToModifyScroll].ItemId = itemScroll.ItemId;
            randLocations.Remove(validLocationScrollItem);
            items.Remove(itemScroll);
        }

        private static List<string> getRequiredItems(List<Region> regions)
        {
            List<string> reqItems = new List<string>();
            foreach (var region in regions)
            {
                foreach (var requiredItem in region.requiredItems)
                {
                    if (!reqItems.Contains(requiredItem) && requiredItem.Length != 0)
                    {
                        reqItems.Add(requiredItem);
                    }

                }
            }

            return reqItems;
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
