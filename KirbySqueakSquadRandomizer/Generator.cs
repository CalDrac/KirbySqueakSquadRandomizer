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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace KirbySqueakSquadRandomizer
{
    internal class Generator
    {
        public static List<string> alreadyPickedPowers;
        public static List<OldItem> item_old;
        public static List<OldItem> newItems_old;
        public static List<OldLocation> randLocations;

        public static List<Item> items;
        public static List<Item> baseItems;
        public static List<Level> levels;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        static Boolean randomize(Options opt)
        {
            var spoilerLog = "";
            byte[] otherData = File.ReadAllBytes(opt.path);

            items = LoadJson<Item>("Data\\item.json", new Item());
            //var alreadyRandomizedAddressed
            baseItems = LoadJson<Item>("Data\\item.json", new Item());
            levels = LoadJson<Level>("Data\\level.json", new Level());
            var bossLevels = LoadJson<BossLevel>("Data\\bossLevel_data.json", new BossLevel());
            var rnd = new Random();
            var randBossLevels = bossLevels.OrderBy(boss => rnd.Next()).ToList();
            if (opt.isBossRandomized)
            {
                spoilerLog += "New boss".PadRight(20) + " | Old boss \n";
                for (int j = 0; j < randBossLevels.Count; j++)
                {
                    var bossLevel = randBossLevels[j];
                    String[] byteLink = bossLevel.dataBlock.Split(' ');
                    var adr = Convert.ToInt32(bossLevels[j].adress, 16);
                    spoilerLog += bossLevel.id.PadRight(20) + " | " + bossLevels[j].id + "\n";
                    items.First(it=> it.LevelId == bossLevels[j].id);
                    for (int i = 0; i < byteLink.Length; i++)
                    {
                        var val = Convert.ToByte(byteLink[i], 16);
                        otherData[adr + i] = val;
                    }
                }
                spoilerLog += "-------------------------------------------------";
                spoilerLog += "\n\n";
            }
            List<Item> newListItem = new List<Item>();
            if (opt.isLevelRando)
            {
                Random rand = new Random();
                List<Level> levelsWithAdress = levels.Where(l => l.Adress.Length > 0).ToList();
                var randLevels = levelsWithAdress.OrderBy(level => rand.Next()).ToList();
                var newLevelList = new List<Level>();
                //var newItemList = new List<Item>();
                //newItemList.AddRange(items);
                spoilerLog += "Map level | Inside level\n";
                for (int i = 0; i < randLevels.Count; i++)
                {
                    spoilerLog +=   randLevels[i].Id + "|" + levelsWithAdress[i].Id + "\n";
                    levelsWithAdress[i].World = randLevels[i].World;
                    levelsWithAdress[i].Stage = randLevels[i].Stage;
                    newLevelList.Add(levelsWithAdress[i]);
                    var adr = Convert.ToInt32(levelsWithAdress[i].Adress, 16);
                    var worldAdr = adr + 1;
                    var stageAdr = adr + 3;
                    var worldAndStage = randLevels[i].Id.Split('_');
                    byte byteWorld = Encoding.ASCII.GetBytes(worldAndStage[0])[0];
                    byte byteStage = Encoding.ASCII.GetBytes(worldAndStage[1])[0];
                    otherData[worldAdr] = byteWorld;
                    otherData[stageAdr] = byteStage;
                    items.ForEach(item =>
                    {
                        if (item.LevelId == randLevels[i].Id)
                        {
                            item.NewLevelId = levelsWithAdress[i].World + "_" + levelsWithAdress[i].Stage;
                        }
                    }
                    );
                }
                //Random rand = new Random();
                //List<Level> levelsWithAdress = levels.Where(l => l.Adress.Length > 0).ToList();
                //var randLevels = levelsWithAdress.OrderBy(level => rand.Next()).ToList();
                //var newLevelList = new List<Level>();
                //spoilerLog += "Old level | New level\n";
                //for (int i = 0; i < randLevels.Count; i++)
                //{
                //    spoilerLog += levelsWithAdress[i].Id + " | " + randLevels[i].Id + "\n";
                //    newLevelList.Add(levelsWithAdress[i]/*randLevels[i]*/);
                //    //newLevelList[i].Adress = levelsWithAdress[i].Adress;
                //    newLevelList[i].World = randLevels[i].World;
                //    newLevelList[i].Stage = randLevels[i].Stage;
                //    newLevelList[i].RequiredItems = randLevels[i].RequiredItems;

                //    var worldAndStage = newLevelList[i].Id.Split('_');

                //    var adr = Convert.ToInt32(randLevels[i].Adress, 16);
                //    var worldAdr = adr + 1;
                //    var stageAdr = adr + 3;
                //    byte byteWorld = Encoding.ASCII.GetBytes(worldAndStage[0])[0];
                //    byte byteStage = Encoding.ASCII.GetBytes(worldAndStage[1])[0];
                //    otherData[worldAdr] = byteWorld;
                //    otherData[stageAdr] = byteStage;

                //    //set newLevelId pour les items
                //    /*items.ForEach(x=> 
                //    {
                //        if (x.NewLevelId == newLevelList[i].Id)
                //        {
                //            x.NewLevelId = newLevelList[i].Id;
                //        }
                //    });*/
                //    foreach (Item item in items.Where(x=> x.LevelId == randLevels[i].Id).ToList())
                //    {
                //        item.NewLevelId = newLevelList[i].Id; //newLevelList[i].World + "_" + newLevelList[i].Stage;
                //    }
                //}

                newLevelList.Insert(0, levels[0]);
                newLevelList.Insert(1, levels[1]);
                newLevelList.Insert(6, levels[6]);
                newLevelList.Insert(13, levels[13]);
                newLevelList.Insert(20, levels[20]);
                newLevelList.Insert(27, levels[27]);
                newLevelList.Insert(33, levels[33]);
                newLevelList.Insert(40, levels[40]);
                newLevelList.Insert(41, levels[41]);
                newLevelList.Insert(42, levels[42]);
                newLevelList.Insert(43, levels[43]);
                newLevelList.Insert(44, levels[44]);
                newLevelList.Insert(45, levels[45]);
                newLevelList.Insert(46, levels[46]);
                newLevelList.Insert(47, levels[47]);
                newLevelList.Insert(48, levels[48]);
                newLevelList.Insert(49, levels[49]);
                newLevelList.Insert(50, levels[50]);
                newLevelList.Insert(51, levels[51]);

                levels = newLevelList;


            }
            //if (!opt.isLevelRando)
            //{
                spoilerLog += "Before".PadRight(25) + " | Now\n";//"New item".PadRight(20) + " | Old item \n";

                HashSet<string> itemIdRequiredSet = new HashSet<string>();
                List<Level> levelsWithRequiredItem = levels.Where(l => l.RequiredItems.Length > 0).ToList();
                foreach (var level in levelsWithRequiredItem)
                {
                    foreach (var itemId in level.RequiredItems)
                    {
                        itemIdRequiredSet.Add(itemId);
                    }
                }
                int itemIndex;
                foreach (var itemIdRequired in itemIdRequiredSet)
                {
                    List<Level> levelsWithoutRequiredItem = levels.Where(l => !l.RequiredItems.Contains(itemIdRequired)).ToList();
                    var validLevelIdList = getValidLevelIdList(levelsWithoutRequiredItem);
                    var tmpItems = new List<Item>(items);
                    tmpItems = tmpItems.Where(it => validLevelIdList.Contains(it.NewLevelId)).ToList();
                    itemIndex = rnd.Next(0, tmpItems.Count);
                    var itemBefore = new Item(tmpItems[itemIndex]);
                    var itemAfter = baseItems.FirstOrDefault(it => it.ItemId == itemIdRequired);

                    items.Remove(items.First(it => it.ItemId == itemBefore.ItemId));

                    spoilerLog += "[" + itemBefore.LevelId + "]" + itemBefore.NewLevelId + "-" + itemBefore.Name.PadRight(20) + " | " + itemAfter.Name + "\n";

                    itemBefore.ItemId = itemAfter.ItemId;
                    itemBefore.Name = itemAfter.Name;
                    itemBefore.ProgressionType = itemAfter.ProgressionType;

                    newListItem.Add(itemBefore);
                    baseItems.Remove(itemAfter);
                }

                while (baseItems.Count > 0)
                {
                    var randomItem = baseItems[rnd.Next(0, baseItems.Count)];

                    spoilerLog += "[" + items.First().LevelId + "]" + items.First().NewLevelId + "-" + items.First().Name.PadRight(20) + " | " + randomItem.Name + "\n";

                    var newItem = new Item(items.First());

                    newItem.ItemId = randomItem.ItemId;
                    newItem.Name = randomItem.Name;
                    newItem.ProgressionType = randomItem.ProgressionType;

                    newListItem.Add(newItem);
                    items.Remove(items.First());
                    baseItems.Remove(randomItem);
                }
            //}
            //else
            //{
            //    
            //    newListItem.AddRange(items);
            //}

            CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.IsFolderPicker = true;

            if (openFileDialog1.ShowDialog() != CommonFileDialogResult.Ok)
            {
                MessageBox.Show("Generation cancelled");
                return false;
            }

            foreach (var item in newListItem)
            {
                var adr = Convert.ToInt32(item.Adress, 16);
                var val = Convert.ToByte(item.ItemId, 16);
                otherData[adr] = val;
            }

            string selectedFileName = openFileDialog1.FileName;
            File.WriteAllBytes(selectedFileName + "\\KSS_rando.nds", otherData);
            MessageBox.Show("File generated " + selectedFileName + "\\KSS_rando.nds");

            File.WriteAllBytes(selectedFileName + "\\KSS_rando_spoiler.txt", Encoding.ASCII.GetBytes(spoilerLog));
            return true;
        }

        private static List<String> getValidLevelIdList(List<Level> levelsWithoutRequiredItem)
        {
            HashSet<String> validLevelIdSet = new HashSet<String>();
            foreach (var level in levelsWithoutRequiredItem)
            {
                validLevelIdSet.Add(level.Id);
            }
            return validLevelIdSet.ToList();
        }

        static Boolean randomize_old(Options opt)
        {
            //get all items
            //get all locations
            //deposer les items requis en 1er 
            alreadyPickedPowers = new List<string>();
            item_old = LoadJson<OldItem>("Data\\item_data.json", new OldItem());
            newItems_old = LoadJson<OldItem>("Data\\item_data.json", new OldItem());
            var staticItems = LoadJson<OldItem>("Data\\item_data.json", new OldItem());
            //var items = LoadJson<Item>("Data\\item_data.json", new Item());
            //var newItems = LoadJson<Item>("Data\\item_data.json", new Item());
            var locations = LoadJson<OldLocation>("Data\\item_source.json", new OldLocation());
            var regions = LoadJson<OldRegion>("Data\\world_path.json", new OldRegion());
            var bossLevels = LoadJson<BossLevel>("Data\\bossLevel_data.json", new BossLevel());
            var monsters = LoadJson<Enemy>("Data\\enemy_data.json", new Enemy());
            var levels = LoadJson<OldLevel>("Data\\level_data.json", new OldLevel());

            var baseMonstersLocation = LoadJson<EnemyLocation>("Data\\enemy_location.json", new EnemyLocation());

            var rnd = new Random();
            var randBossLevels = bossLevels.OrderBy(boss => rnd.Next()).ToList();
            var randLevels = levels.OrderBy(lvl=> rnd.Next()).ToList();
            var randMonsterLocation = baseMonstersLocation.OrderBy(monster => rnd.Next()).ToList();

            var spoilerLog = "";
            byte[] otherData = File.ReadAllBytes(opt.path);
            if (opt.isLevelRando)
            {
                for(int i = 0; i < randLevels.Count; i++)
                {
                    var level = randLevels[i];
                    var adr = Convert.ToInt32(levels[i].adress, 16);
                    spoilerLog +=  level.worldNumber + "-" + level.stage + " | " + levels[i].worldNumber + "-" + levels[i].stage +  "\n";
                    var worldAdr = adr + 1;
                    var stageAdr = adr + 3;
                    byte byteWorld = Encoding.ASCII.GetBytes(level.worldNumber)[0];
                    byte byteStage = Encoding.ASCII.GetBytes(level.stage)[0];
                    otherData[worldAdr] = byteWorld;
                    otherData[stageAdr] = byteStage;
                    foreach (OldLocation item in locations.Where(loc => loc.stage == levels[i].stage && loc.nodeId == levels[i].worldId).ToList())
                    {
                        item.stage = level.stage;
                        item.nodeId = level.worldId;   
                    }
                }
            }
            if (opt.isPowerBlocking) //power blocking or evertything else for now
            {
                if (opt.isBossRandomized)
                {
                    spoilerLog = randomizeBoss(locations, bossLevels, randBossLevels, spoilerLog, otherData);
                }
                spoilerLog += "New item".PadRight(20) + " | Old item \n";
                List<String> reqItemsStr = getRequiredItems(regions);
                var requiredItems = item_old.Where(i => reqItemsStr.Contains(i.Name)).ToList();
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
                        var indexToModify = newItems_old.IndexOf(newItems_old.First(it => it.Name == validLocation.name));
                        newItems_old[indexToModify].Name = requiredItems[i].Name;
                        newItems_old[indexToModify].ItemId = requiredItems[i].ItemId;
                        randLocations.Remove(validLocation);
                        item_old.Remove(requiredItems[i]);
                        addScrollPower(bannedRegionsNames, validLocation);
                    }
                    else
                    {
                        MessageBox.Show("ERROR GENERATION");
                        throw new Exception();
                    }
                }
                //placement du reste des items
                while (item_old.Count > 0)
                {
                    var pickedLocation = randLocations.First();
                    var indexToModify = newItems_old.IndexOf(newItems_old.First(it => it.Name == pickedLocation.name));
                    newItems_old[indexToModify].Name = item_old.First().Name;
                    newItems_old[indexToModify].ItemId = item_old.First().ItemId;

                    if (item_old.First().Name.Contains("scroll"))
                    {
                        alreadyPickedPowers.Add(item_old.First().Name.Replace(" scroll", ""));
                    }

                    randLocations.Remove(pickedLocation);

                    item_old.Remove(item_old.First());
                    var pickedLocationReqPwrs = pickedLocation.requiredPowers;
                    pickedLocationReqPwrs.RemoveAll(rp => alreadyPickedPowers.Contains(rp));
                    if (pickedLocationReqPwrs.Count != 0)
                    {
                        addScrollPower(new List<string>(), pickedLocation);
                        //placer un scroll ?
                    }
                }

                //remplacement des ID des newItems aux adresses des oldItems           
                foreach (var item in newItems_old)
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
                    spoilerLog = randomizeBoss(locations, bossLevels, randBossLevels, spoilerLog, otherData);
                }

                spoilerLog += "New item".PadRight(20) + " | Old item \n";
                List<String> reqItemsStr = getRequiredItems(regions);
                var requiredItems = item_old.Where(i => reqItemsStr.Contains(i.Name)).ToList();
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
                        var indexToModify = newItems_old.IndexOf(newItems_old.First(it => it.Name == validLocation.name));
                        newItems_old[indexToModify].Name = requiredItems[i].Name;
                        newItems_old[indexToModify].ItemId = requiredItems[i].ItemId;
                        randLocations.Remove(validLocation);
                        item_old.Remove(requiredItems[i]);
                    }
                    else
                    {
                        MessageBox.Show("ERROR GENERATION");
                        throw new Exception();
                    }
                }
                //placement du reste des items
                for (int i = 0; i < item_old.Count; i++)
                {
                    var indexToModify = newItems_old.IndexOf(newItems_old.First(it => it.Name == randLocations[i].name));
                    newItems_old[indexToModify].Name = item_old[i].Name;
                    newItems_old[indexToModify].ItemId = item_old[i].ItemId;
                }

                //remplacement des ID des newItems aux adresses des oldItems           
                foreach (var item in newItems_old)
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

        private static string randomizeBoss(List<OldLocation> locations, List<BossLevel> bossLevels, List<BossLevel> randBossLevels, string spoilerLog, byte[] otherData)
        {
           
            spoilerLog += "New boss".PadRight(20) + " | Old boss \n";
            for (int j = 0; j < randBossLevels.Count; j++)
            {
                var bossLevel = randBossLevels[j];
                String[] byteLink = bossLevel.dataBlock.Split(' ');
                var adr = Convert.ToInt32(bossLevels[j].adress, 16);
                spoilerLog += bossLevel.id.PadRight(20) + " | " + bossLevels[j].id + "\n";
                //locations.First(x => x.name.Contains(bossLevel.name)).nodeId = bossLevels[j].nodeId;
                for (int i = 0; i < byteLink.Length; i++)
                {
                    var val = Convert.ToByte(byteLink[i], 16);
                    otherData[adr + i] = val;
                }
            }
            spoilerLog += "-------------------------------------------------";
            spoilerLog += "\n\n";

            return spoilerLog;
        }

        //Add scroll if a reqItem location has a reqPower
        private static void addScrollPower(List<string> bannedRegionsNames, OldLocation validLocation)
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
                var itemScroll = item_old.First(l => l.Name == scroll);
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

        private static List<string> addScrollForScroll(List<string> bannedRegionsNames, List<string> filteredPowers, OldItem itemScroll)
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

                    itemScroll = item_old.First(l => l.Name == scroll);
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

        private static void setNewitem(OldItem itemScroll, OldLocation? validLocationScrollItem)
        {
            var indexToModifyScroll = newItems_old.IndexOf(newItems_old.First(it => it.Name == validLocationScrollItem.name));
            newItems_old[indexToModifyScroll].Name = itemScroll.Name;
            newItems_old[indexToModifyScroll].ItemId = itemScroll.ItemId;
            randLocations.Remove(validLocationScrollItem);
            item_old.Remove(itemScroll);
        }

        private static List<string> getRequiredItems(List<OldRegion> regions)
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
