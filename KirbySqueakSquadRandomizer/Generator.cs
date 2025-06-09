using KirbySqueakSquadRandomizer.Model;
using KirbySqueakSquadRandomizer.Model_older;
using KirbySqueakSquadRandomizer.Utils;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
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

        public static List<ItemV2> itemsV2;
        public static List<Stage> stages;


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
            return randomizeV2(opt);
        }


        static Boolean randomizeV2(Options opt)
        {
            var spoilerLog = "";
            InitData();
            List<ItemV2> newItemList = new List<ItemV2>();
            List<ItemV2> shuffledItemList = ItemUtils.createNewListItems();

            shuffledItemList = shuffledItemList.OrderBy(x => Random.Shared.Next()).ToList();
            List<Stage> stagesWithRequiredItems = stages.FindAll(x => x.requiredItems.Count != 0);
            List<ItemV2> requiredItems = new List<ItemV2>();
            foreach (Stage stage in stagesWithRequiredItems)
            {
                foreach (ItemV2 item in stage.requiredItems)
                    if (!requiredItems.Contains(item))
                    {
                        requiredItems.Add(item);
                    }
            }
            var alreadyTakenItems = new List<ItemV2>();
            foreach (ItemV2 item in requiredItems)
            {
                
                
                ////Get item list where you can put the required item
                List<Stage> stagesWhereRequiredItemIsNotRequired = stages.FindAll(x => !x.requiredItems.Contains(item));
                List<ItemV2> itemsThatCanBeReplacedWithRequiredItem = new List<ItemV2>();
                foreach (Stage stage in stagesWhereRequiredItemIsNotRequired)
                {
                    itemsThatCanBeReplacedWithRequiredItem.AddRange(stage.items);
                }
                itemsThatCanBeReplacedWithRequiredItem = itemsThatCanBeReplacedWithRequiredItem.OrderBy(x => Random.Shared.Next()).ToList();
                foreach (ItemV2 alreadyTakenItem in alreadyTakenItems)
                {
                    itemsThatCanBeReplacedWithRequiredItem.Remove(alreadyTakenItem);
                }
                ItemV2 itemToReplace = new ItemV2(itemsThatCanBeReplacedWithRequiredItem[0]);

                var stageItem = stages.Find(x => x.items.Contains(itemsThatCanBeReplacedWithRequiredItem[0]));

                spoilerLog += "[" + stageItem.world + "_" + stageItem.number + "]" + itemToReplace.name.PadRight(20) + "|" + item.name + "\n";


                var removedFromItems = itemsV2.Remove(itemsV2.First(it => it.adress == itemToReplace.adress));
                alreadyTakenItems.Add(itemsThatCanBeReplacedWithRequiredItem[0]);
                var removedFromShuffle = shuffledItemList.Remove(shuffledItemList.First(it=>it.adress == item.adress));
                itemToReplace.name = item.name;
                itemToReplace.itemId = item.itemId;
                newItemList.Add(itemToReplace);
                //var itemToDel = shuffledItemList.Find(it => it.name == item.name);
                //var removedFromShuffle = shuffledItemList.Remove(itemToDel);
            }

            foreach (ItemV2 item in shuffledItemList)
            {
                var itemToReplace = new ItemV2(itemsV2[0]);
                itemsV2.Remove(itemsV2[0]);
                spoilerLog += itemToReplace.name.PadRight(20) + "|" + item.name + "\n";
                itemToReplace.name = item.name;
                itemToReplace.itemId = item.itemId;
                newItemList.Add(itemToReplace);

            }

            byte[] otherData = File.ReadAllBytes(opt.path);
            CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.IsFolderPicker = true;

            if (openFileDialog1.ShowDialog() != CommonFileDialogResult.Ok)
            {
                MessageBox.Show("Generation cancelled");
                return false;
            }

            foreach (var item in newItemList)
            {
                var adr = Convert.ToInt32(item.adress, 16);
                var val = Convert.ToByte(item.itemId, 16);
                otherData[adr] = val;
            }

            string selectedFileName = openFileDialog1.FileName;
            File.WriteAllBytes(selectedFileName + "\\KSS_rando.nds", otherData);
            MessageBox.Show("File generated " + selectedFileName + "\\KSS_rando.nds");

            File.WriteAllBytes(selectedFileName + "\\KSS_rando_spoiler.txt", Encoding.ASCII.GetBytes(spoilerLog));
            return true;
        }

        static void InitData()
        {
            //Init items

            initItems();

            //Init stages
            initLevels();
        }

        private static void initLevels()
        {
            var items = ItemUtils.createNewListItems();

            var stage1_1 = new Stage(1, 1, "", new List<ItemV2>(), new List<ItemV2>());
            var stage1_2 = new Stage(1, 2, "", new List<ItemV2>() { items[0] }, new List<ItemV2>());
            var stage1_3 = new Stage(1, 3, "0251225", new List<ItemV2>() { items[1], items[2] }, new List<ItemV2>());
            var stage1_4 = new Stage(1, 4, "025132F", new List<ItemV2>() { items[3], items[4], items[5] }, new List<ItemV2>());
            var stage1_5 = new Stage(1, 5, "0251439", new List<ItemV2>() { items[6] }, new List<ItemV2>());
            var stage1_6 = new Stage(1, 6, "025155B", new List<ItemV2>() { items[8], items[9] }, new List<ItemV2>() { items[5] });
            var stage1_7 = new Stage(1, 7, "", new List<ItemV2>() { items[7] }, new List<ItemV2>());

            var stage2_1 = new Stage(2, 1, "0251F4C", new List<ItemV2>() { items[10], items[11], items[12] }, new List<ItemV2>() { });
            var stage2_2 = new Stage(2, 2, "02520AB", new List<ItemV2>() { items[13], items[14] }, new List<ItemV2>() { });
            var stage2_3 = new Stage(2, 3, "025223A", new List<ItemV2>() { items[15], items[16], items[17] }, new List<ItemV2>() { });
            var stage2_4 = new Stage(2, 4, "02523F9", new List<ItemV2>() { items[18] }, new List<ItemV2>() { });
            var stage2_5 = new Stage(2, 5, "02525A0", new List<ItemV2>() { items[19], items[20], items[21] }, new List<ItemV2>() { });
            var stage2_6 = new Stage(2, 6, "02526B7", new List<ItemV2>() { items[23], items[24], items[25] }, new List<ItemV2>() { items[21] });
            var stage2_7 = new Stage(2, 7, "", new List<ItemV2>() { items[22] }, new List<ItemV2>() { });

            var stage3_1 = new Stage(3, 1, "02529CD", new List<ItemV2>() { items[26], items[27], items[28] }, new List<ItemV2>() { });
            var stage3_2 = new Stage(3, 2, "0252ACC", new List<ItemV2>() { items[29], items[30] }, new List<ItemV2>() { });
            var stage3_3 = new Stage(3, 3, "0252BCB", new List<ItemV2>() { items[31], items[32], items[33] }, new List<ItemV2>() { });
            var stage3_4 = new Stage(3, 4, "0252CFA", new List<ItemV2>() { items[34], items[35], items[36] }, new List<ItemV2>() { });
            var stage3_5 = new Stage(3, 5, "0252E29", new List<ItemV2>() { items[37] }, new List<ItemV2>() { });
            var stage3_6 = new Stage(3, 6, "0252EB6", new List<ItemV2>() { items[39], items[40], items[41] }, new List<ItemV2>() { items[36] });
            var stage3_7 = new Stage(3, 7, "", new List<ItemV2>() { items[38] }, new List<ItemV2>() { });

            var stage4_1 = new Stage(4, 1, "0252FEA", new List<ItemV2>() { items[42], items[43], items[44] }, new List<ItemV2>() { });
            var stage4_2 = new Stage(4, 2, "02530E9", new List<ItemV2>() { items[45], items[46] }, new List<ItemV2>() { });
            var stage4_3 = new Stage(4, 3, "02531B8", new List<ItemV2>() { items[47], items[48], items[49] }, new List<ItemV2>() { });
            var stage4_4 = new Stage(4, 4, "02532FF", new List<ItemV2>() { items[50], items[51], items[52] }, new List<ItemV2>() { });
            var stage4_5 = new Stage(4, 5, "0253446", new List<ItemV2>() { items[53] }, new List<ItemV2>() { });
            var stage4_6 = new Stage(4, 6, "02534D3", new List<ItemV2>() { items[55], items[56], items[57] }, new List<ItemV2>() { items[52] });
            var stage4_7 = new Stage(4, 7, "", new List<ItemV2>() { items[54] }, new List<ItemV2>() { });

            var stage5_1 = new Stage(5, 1, "0253607", new List<ItemV2>() { items[58], items[59], items[60] }, new List<ItemV2>() { });
            var stage5_2 = new Stage(5, 2, "0253706", new List<ItemV2>() { items[61], items[62], items[63] }, new List<ItemV2>() { });
            var stage5_3 = new Stage(5, 3, "02537ED", new List<ItemV2>() { items[64], items[65], items[66] }, new List<ItemV2>() { });
            var stage5_4 = new Stage(5, 4, "02538EC", new List<ItemV2>() { items[67], items[68], items[69] }, new List<ItemV2>() { });
            var stage5_6 = new Stage(5, 6, "0253A03", new List<ItemV2>() { items[71], items[72], items[73] }, new List<ItemV2>() { items[69] });
            var stage5_7 = new Stage(5, 7, "", new List<ItemV2>() { items[70] }, new List<ItemV2>() { });

            var stage6_1 = new Stage(6, 1, "0253B6C", new List<ItemV2>() { items[74], items[75], items[76] }, new List<ItemV2>() { });
            var stage6_2 = new Stage(6, 2, "0253C83", new List<ItemV2>() { items[77], items[78] }, new List<ItemV2>() { });
            var stage6_3 = new Stage(6, 3, "0253D82", new List<ItemV2>() { items[79], items[80], items[81] }, new List<ItemV2>() { });
            var stage6_4 = new Stage(6, 4, "0253EC9", new List<ItemV2>() { items[82], items[83], items[84] }, new List<ItemV2>() { });
            var stage6_5 = new Stage(6, 5, "0253FE0", new List<ItemV2>() { items[85], items[86], items[87] }, new List<ItemV2>() { });
            var stage6_6 = new Stage(6, 6, "02540DF", new List<ItemV2>() { items[89], items[90], items[91] }, new List<ItemV2>() { items[84] });
            var stage6_7 = new Stage(6, 7, "", new List<ItemV2>() { items[88] }, new List<ItemV2>() { });

            var stage7_1 = new Stage(7, 1, "254213", new List<ItemV2>() { items[92], items[93], items[94] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });
            var stage7_2 = new Stage(7, 2, "254312", new List<ItemV2>() { items[95], items[96], items[97] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });
            var stage7_3 = new Stage(7, 3, "254429", new List<ItemV2>() { items[98], items[99], items[100] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });
            var stage7_4 = new Stage(7, 4, "254510", new List<ItemV2>() { items[101], items[102], items[103] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });
            var stage7_5 = new Stage(7, 5, "25460F", new List<ItemV2>() { items[104] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });
            var stage7_6 = new Stage(7, 6, "254726", new List<ItemV2>() { items[106], items[107], items[108] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88], items[103] });
            var stage7_7 = new Stage(7, 7, "", new List<ItemV2>() { items[105] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });

            var stage8_1 = new Stage(8, 1, "", new List<ItemV2>() { items[109], items[110], items[111] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });
            var stage8_2 = new Stage(8, 2, "", new List<ItemV2>() { items[112], items[113], items[114] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });
            var stage8_3 = new Stage(8, 3, "", new List<ItemV2>() { items[115], items[116], items[117] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });
            var stage8_7 = new Stage(8, 7, "", new List<ItemV2>() { items[118] }, new List<ItemV2>() { items[12], items[30], items[46], items[60], items[78], items[88] });

            stages = fillListStages(
                stage1_1, stage1_2, stage1_3, stage1_4, stage1_5, stage1_6, stage1_7,
                stage2_1, stage2_2, stage2_3, stage2_4, stage2_5, stage2_6, stage2_7,
                stage3_1, stage3_2, stage3_3, stage3_4, stage3_5, stage3_6, stage3_7,
                stage4_1, stage4_2, stage4_3, stage4_4, stage4_5, stage4_6, stage4_7,
                stage5_1, stage5_2, stage5_3, stage5_4, stage5_6, stage5_7,
                stage6_1, stage6_2, stage6_3, stage6_4, stage6_5, stage6_6, stage6_7,
                stage7_1, stage7_2, stage7_3, stage7_4, stage7_5, stage7_6, stage7_7,
                stage8_1, stage8_2, stage8_3, stage8_7
            );
        }

        private static List<Stage> fillListStages(Stage stage1_1, Stage stage1_2, Stage stage1_3, Stage stage1_4, Stage stage1_5, Stage stage1_6, Stage stage1_7, Stage stage2_1, Stage stage2_2, Stage stage2_3, Stage stage2_4, Stage stage2_5, Stage stage2_6, Stage stage2_7, Stage stage3_1, Stage stage3_2, Stage stage3_3, Stage stage3_4, Stage stage3_5, Stage stage3_6, Stage stage3_7, Stage stage4_1, Stage stage4_2, Stage stage4_3, Stage stage4_4, Stage stage4_5, Stage stage4_6, Stage stage4_7, Stage stage5_1, Stage stage5_2, Stage stage5_3, Stage stage5_4, Stage stage5_6, Stage stage5_7, Stage stage6_1, Stage stage6_2, Stage stage6_3, Stage stage6_4, Stage stage6_5, Stage stage6_6, Stage stage6_7, Stage stage7_1, Stage stage7_2, Stage stage7_3, Stage stage7_4, Stage stage7_5, Stage stage7_6, Stage stage7_7, Stage stage8_1, Stage stage8_2, Stage stage8_3, Stage stage8_7)
        {
            return new List<Stage> {
                stage1_1, stage1_2, stage1_3,stage1_4,stage1_5,stage1_6,stage1_7,
                stage2_1, stage2_2, stage2_3,stage2_4,stage2_5,stage2_6,stage2_7,
                stage3_1, stage3_2, stage3_3,stage3_4,stage3_5,stage3_6,stage3_7,
                stage4_1, stage4_2, stage4_3,stage4_4,stage4_5,stage4_6,stage4_7,
                stage5_1, stage5_2, stage5_3,stage5_4,stage5_6,stage5_7,
                stage6_1, stage6_2, stage6_3,stage6_4,stage6_5,stage6_6,stage6_7,
                stage7_1, stage7_2, stage7_3,stage7_4,stage7_5,stage7_6,stage7_7,
                stage8_1, stage8_2, stage8_3,stage8_7
            };
        }

        private static void initItems()
        {

            itemsV2 = ItemUtils.createNewListItems();
        }

        private static List<ItemV2> fillListItems(ItemV2 item1, ItemV2 item2, ItemV2 item3, ItemV2 item4, ItemV2 item5, ItemV2 item6, ItemV2 item7, ItemV2 item8, ItemV2 item9, ItemV2 item10, ItemV2 item11, ItemV2 item12, ItemV2 item13, ItemV2 item14, ItemV2 item15, ItemV2 item16, ItemV2 item17, ItemV2 item18, ItemV2 item19, ItemV2 item20, ItemV2 item21, ItemV2 item22, ItemV2 item23, ItemV2 item24, ItemV2 item25, ItemV2 item26, ItemV2 item27, ItemV2 item28, ItemV2 item29, ItemV2 item30, ItemV2 item31, ItemV2 item32, ItemV2 item33, ItemV2 item34, ItemV2 item35, ItemV2 item36, ItemV2 item37, ItemV2 item38, ItemV2 item39, ItemV2 item40, ItemV2 item41, ItemV2 item42, ItemV2 item43, ItemV2 item44, ItemV2 item45, ItemV2 item46, ItemV2 item47, ItemV2 item48, ItemV2 item49, ItemV2 item50, ItemV2 item51, ItemV2 item52, ItemV2 item53, ItemV2 item54, ItemV2 item55, ItemV2 item56, ItemV2 item57, ItemV2 item58, ItemV2 item59, ItemV2 item60, ItemV2 item61, ItemV2 item62, ItemV2 item63, ItemV2 item64, ItemV2 item65, ItemV2 item66, ItemV2 item67, ItemV2 item68, ItemV2 item69, ItemV2 item70, ItemV2 item71, ItemV2 item72, ItemV2 item73, ItemV2 item74, ItemV2 item75, ItemV2 item76, ItemV2 item77, ItemV2 item78, ItemV2 item79, ItemV2 item80, ItemV2 item81, ItemV2 item82, ItemV2 item83, ItemV2 item84, ItemV2 item85, ItemV2 item86, ItemV2 item87, ItemV2 item88, ItemV2 item89, ItemV2 item90, ItemV2 item91, ItemV2 item92, ItemV2 item93, ItemV2 item94, ItemV2 item95, ItemV2 item96, ItemV2 item97, ItemV2 item98, ItemV2 item99, ItemV2 item100, ItemV2 item101, ItemV2 item102, ItemV2 item103, ItemV2 item104, ItemV2 item105, ItemV2 item106, ItemV2 item107, ItemV2 item108, ItemV2 item109, ItemV2 item110, ItemV2 item111, ItemV2 item112, ItemV2 item113, ItemV2 item114, ItemV2 item115, ItemV2 item116, ItemV2 item117, ItemV2 item118, ItemV2 item119)
        {
            return new List<ItemV2>
            {
                item1, item2, item3, item4, item5, item6, item7, item8, item9,
                item10,item11, item12, item13, item14, item15, item16, item17, item18, item19, item20, item21, item22, item23, item24, item25, item26, item27, item28, item29, item30, item31, item32, item33, item34, item35, item36, item37, item38, item39, item40, item41, item42, item43, item44, item45, item46, item47, item48, item49, item50, item51, item52, item53, item54, item55, item56, item57, item58, item59, item60, item61, item62, item63, item64, item65, item66, item67, item68, item69, item70, item71, item72, item73, item74, item75, item76, item77, item78, item79, item80, item81, item82, item83, item84, item85, item86, item87, item88, item89, item90, item91, item92, item93, item94, item95, item96, item97, item98, item99, item100, item101, item102, item103, item104, item105, item106, item107, item108, item109, item110, item111, item112, item113, item114, item115, item116, item117, item118, item119
            };
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
                    items.First(it => it.LevelId == bossLevels[j].id);
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
                    spoilerLog += randLevels[i].Id + "|" + levelsWithAdress[i].Id + "\n";
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
                        if (item.LevelId == levelsWithAdress[i].Id)
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
                //newLevelList.Insert(41, levels[41]);
                // newLevelList.Insert(42, levels[42]);
                //newLevelList.Insert(43, levels[43]);
                // newLevelList.Insert(44, levels[44]);
                //newLevelList.Insert(45, levels[45]);
                // newLevelList.Insert(46, levels[46]);
                newLevelList.Insert(47, levels[47]);
                //newLevelList.Insert(48, levels[48]);
                //newLevelList.Insert(49, levels[49]);
                //newLevelList.Insert(50, levels[50]);
                newLevelList.Insert(51, levels[51]);

                levels = newLevelList;

                baseItems.Clear();
                baseItems.AddRange(items);
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

                spoilerLog += "[" + itemBefore.LevelId + "]" + itemBefore.NewLevelId + "-" + itemBefore.Name.PadRight(20) + " | [" + itemAfter.LevelId + "]" + itemAfter.NewLevelId + "-" + itemAfter.Name + "\n";

                itemBefore.ItemId = itemAfter.ItemId;
                itemBefore.Name = itemAfter.Name;
                itemBefore.ProgressionType = itemAfter.ProgressionType;

                newListItem.Add(itemBefore);
                baseItems.Remove(itemAfter);
            }

            while (baseItems.Count > 0)
            {
                var randomItem = baseItems[rnd.Next(0, baseItems.Count)];

                spoilerLog += "[" + items.First().LevelId + "]" + items.First().NewLevelId + "-" + items.First().Name.PadRight(20) + " | [" + randomItem.LevelId + "]" + randomItem.NewLevelId + "-" + randomItem.Name + "\n";

                var newItem = new Item(items.First());

                newItem.ItemId = randomItem.ItemId;
                newItem.Name = randomItem.Name;
                newItem.ProgressionType = randomItem.ProgressionType;

                newListItem.Add(newItem);
                items.Remove(items.First());
                baseItems.Remove(randomItem);
            }

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
            var randLevels = levels.OrderBy(lvl => rnd.Next()).ToList();
            var randMonsterLocation = baseMonstersLocation.OrderBy(monster => rnd.Next()).ToList();

            var spoilerLog = "";
            byte[] otherData = File.ReadAllBytes(opt.path);
            if (opt.isLevelRando)
            {
                for (int i = 0; i < randLevels.Count; i++)
                {
                    var level = randLevels[i];
                    var adr = Convert.ToInt32(levels[i].adress, 16);
                    spoilerLog += level.worldNumber + "-" + level.stage + " | " + levels[i].worldNumber + "-" + levels[i].stage + "\n";
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

