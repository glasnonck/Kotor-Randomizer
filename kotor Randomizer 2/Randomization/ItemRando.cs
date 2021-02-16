﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KotOR_IO;
using System.Text.RegularExpressions;
using ClosedXML.Excel;

namespace kotor_Randomizer_2
{
    public static class ItemRando
    {
        /// <summary>
        /// A lookup table used to know how the items are randomized.
        /// </summary>
        private static Dictionary<string, string> LookupTable { get; set; } = new Dictionary<string, string>();

        public static void item_rando(KPaths paths)
        {
            // Prepare lists for new randomization.
            Max_Rando.Clear();
            Type_Lists.Clear();
            LookupTable.Clear();

            // Load KEY file.
            KEY k = new KEY(paths.chitin);

            // Handle categories
            HandleCategory(k, ArmbandsRegs, Properties.Settings.Default.RandomizeArmbands);
            HandleCategory(k, ArmorRegs, Properties.Settings.Default.RandomizeArmor);
            HandleCategory(k, BeltsRegs, Properties.Settings.Default.RandomizeBelts);
            HandleCategory(k, BlastersRegs, Properties.Settings.Default.RandomizeBlasters);
            HandleCategory(k, HidesRegs, Properties.Settings.Default.RandomizeHides);
            HandleCategory(k, CreatureRegs, Properties.Settings.Default.RandomizeCreature);
            HandleCategory(k, DroidRegs, Properties.Settings.Default.RandomizeDroid);
            HandleCategory(k, GlovesRegs, Properties.Settings.Default.RandomizeGloves);
            HandleCategory(k, GrenadesRegs, Properties.Settings.Default.RandomizeGrenades);
            HandleCategory(k, ImplantsRegs, Properties.Settings.Default.RandomizeImplants);
            HandleCategory(k, LightsabersRegs, Properties.Settings.Default.RandomizeLightsabers);
            HandleCategory(k, MaskRegs, Properties.Settings.Default.RandomizeMask);
            HandleCategory(k, MeleeRegs, Properties.Settings.Default.RandomizeMelee);
            HandleCategory(k, MinesRegs, Properties.Settings.Default.RandomizeMines);
            HandleCategory(k, PazRegs, Properties.Settings.Default.RandomizePaz);
            HandleCategory(k, StimsRegs, Properties.Settings.Default.RandomizeStims);
            HandleCategory(k, UpgradeRegs, Properties.Settings.Default.RandomizeUpgrade);

            //handle Various
            switch ((RandomizationLevel)Properties.Settings.Default.RandomizeVarious)
            {
                default:
                case RandomizationLevel.None:
                    break;
                case RandomizationLevel.Type:
                    List<string> type = new List<string>(k.KeyTable.Where(x => Matches_None(x.ResRef) && !Is_Forbidden(x.ResRef) && x.ResourceType == (short)ResourceType.UTI).Select(x => x.ResRef));
                    Type_Lists.Add(type);
                    break;
                case RandomizationLevel.Max:
                    Max_Rando.AddRange(k.KeyTable.Where(x => Matches_None(x.ResRef) && !Is_Forbidden(x.ResRef) && x.ResourceType == (short)ResourceType.UTI).Select(x => x.ResRef));
                    break;
            }

            //Max Rando
            List<string> Max_Rando_Iterator = new List<string>(Max_Rando);
            Randomize.FisherYatesShuffle(Max_Rando);
            int j = 0;
            foreach (KEY.KeyEntry ke in k.KeyTable.Where(x => Max_Rando_Iterator.Contains(x.ResRef)))
            {
                LookupTable.Add(ke.ResRef, Max_Rando[j]);
                ke.ResRef = Max_Rando[j];
                j++;
            }

            //Type Rando
            foreach (List<string> li in Type_Lists)
            {
                List<string> type_copy = new List<string>(li);
                Randomize.FisherYatesShuffle(type_copy);
                j = 0;
                foreach (KEY.KeyEntry ke in k.KeyTable.Where(x => li.Contains(x.ResRef)))
                {
                    LookupTable.Add(ke.ResRef, type_copy[j]);
                    ke.ResRef = type_copy[j];
                    j++;
                }
            }

            k.WriteToFile(paths.chitin);
        }

        private static void HandleCategory(KEY k, List<Regex> r, int randomizationLevel)
        {
            switch ((RandomizationLevel)randomizationLevel)
            {
                case RandomizationLevel.None:
                default:
                    break;
                case RandomizationLevel.Subtype:
                    for (int i = 1; i < r.Count; i++)
                    {
                        List<string> temp = new List<string>(k.KeyTable.Where(x => r[i].IsMatch(x.ResRef) && !Is_Forbidden(x.ResRef) && x.ResourceType == (short)ResourceType.UTI).Select(x => x.ResRef));
                        Type_Lists.Add(temp);
                    }
                    break;
                case RandomizationLevel.Type:
                    List<string> type = new List<string>(k.KeyTable.Where(x => r[0].IsMatch(x.ResRef) && !Is_Forbidden(x.ResRef) && x.ResourceType == (short)ResourceType.UTI).Select(x => x.ResRef));
                    Type_Lists.Add(type);
                    break;
                case RandomizationLevel.Max:
                    Max_Rando.AddRange(k.KeyTable.Where(x => r[0].IsMatch(x.ResRef) && !Is_Forbidden(x.ResRef) && x.ResourceType == (short)ResourceType.UTI).Select(x => x.ResRef));
                    break;
            }
        }

        private static bool Is_Forbidden(string s)
        {
            return Globals.OmitItems.Contains(s);
        }

        private static bool Matches_None(string s)
        {
            return
                (
                    !ArmorRegs[0].IsMatch(s) &&
                    !StimsRegs[0].IsMatch(s) &&
                    !BeltsRegs[0].IsMatch(s) &&
                    !HidesRegs[0].IsMatch(s) &&
                    !DroidRegs[0].IsMatch(s) &&
                    !ArmbandsRegs[0].IsMatch(s) &&
                    !GlovesRegs[0].IsMatch(s) &&
                    !ImplantsRegs[0].IsMatch(s) &&
                    !MaskRegs[0].IsMatch(s) &&
                    !PazRegs[0].IsMatch(s) &&
                    !MinesRegs[0].IsMatch(s) &&
                    !UpgradeRegs[0].IsMatch(s) &&
                    !BlastersRegs[0].IsMatch(s) &&
                    !CreatureRegs[0].IsMatch(s) &&
                    !LightsabersRegs[0].IsMatch(s) &&
                    !GrenadesRegs[0].IsMatch(s) &&
                    !MeleeRegs[0].IsMatch(s)
                );
        }

        private static List<string> Max_Rando = new List<string>();

        private static List<List<string>> Type_Lists = new List<List<string>>();

        /// <summary>
        /// Creates a CSV file containing a list of the changes made during randomization.
        /// If the file already exists, this method will append the data.
        /// If no randomization has been performed, no file will be created.
        /// </summary>
        /// <param name="path">Path to desired output file.</param>
        public static void GenerateSpoilerLog(string path)
        {
            if (LookupTable.Count == 0) { return; }
            var sortedLookup = LookupTable.OrderBy(kvp => kvp.Key);

            using (StreamWriter sw = new StreamWriter(path))
            {
                //sw.WriteLine("Items,");
                sw.WriteLine($"Seed,{Properties.Settings.Default.Seed}");
                sw.WriteLine();

                sw.WriteLine("Item Type,Rando Level");
                sw.WriteLine($"Armbands,{(RandomizationLevel)Properties.Settings.Default.RandomizeArmbands}");
                sw.WriteLine($"Armor,{(RandomizationLevel)Properties.Settings.Default.RandomizeArmor}");
                sw.WriteLine($"Belts,{(RandomizationLevel)Properties.Settings.Default.RandomizeBelts}");
                sw.WriteLine($"Blasters,{(RandomizationLevel)Properties.Settings.Default.RandomizeBlasters}");
                sw.WriteLine($"Creature Hides,{(RandomizationLevel)Properties.Settings.Default.RandomizeHides}");
                sw.WriteLine($"Creature Weapons,{(RandomizationLevel)Properties.Settings.Default.RandomizeCreature}");
                sw.WriteLine($"Droid Equipment,{(RandomizationLevel)Properties.Settings.Default.RandomizeDroid}");
                sw.WriteLine($"Gauntlets,{(RandomizationLevel)Properties.Settings.Default.RandomizeGloves}");
                sw.WriteLine($"Grenades,{(RandomizationLevel)Properties.Settings.Default.RandomizeGrenades}");
                sw.WriteLine($"Implants,{(RandomizationLevel)Properties.Settings.Default.RandomizeImplants}");
                sw.WriteLine($"Lightsabers,{(RandomizationLevel)Properties.Settings.Default.RandomizeLightsabers}");
                sw.WriteLine($"Masks,{(RandomizationLevel)Properties.Settings.Default.RandomizeMask}");
                sw.WriteLine($"Melee Weapons,{(RandomizationLevel)Properties.Settings.Default.RandomizeMelee}");
                sw.WriteLine($"Mines,{(RandomizationLevel)Properties.Settings.Default.RandomizeMines}");
                sw.WriteLine($"Pazaak Cards,{(RandomizationLevel)Properties.Settings.Default.RandomizePaz}");
                sw.WriteLine($"Stims/Medpacs,{(RandomizationLevel)Properties.Settings.Default.RandomizeStims}");
                sw.WriteLine($"Upgrades/Crystals,{(RandomizationLevel)Properties.Settings.Default.RandomizeUpgrade}");
                sw.WriteLine($"Various,{(RandomizationLevel)Properties.Settings.Default.RandomizeVarious}");
                sw.WriteLine();

                sw.WriteLine("Omitted Items");
                foreach (var item in Globals.OmitItems)
                {
                    sw.WriteLine(item);
                }
                sw.WriteLine();

                sw.WriteLine("Has Changed,Original,Randomized");
                foreach (var kvp in sortedLookup)
                {
                    sw.WriteLine($"{(kvp.Key != kvp.Value).ToString()},{kvp.Key},{kvp.Value}");
                }
                sw.WriteLine();
            }
        }

        internal static void Reset()
        {
            // Prepare lists for new randomization.
            Max_Rando.Clear();
            Type_Lists.Clear();
            LookupTable.Clear();
        }

        public static void GenerateSpoilerLog(XLWorkbook workbook)
        {
            if (LookupTable.Count == 0) { return; }
            var ws = workbook.Worksheets.Add("Items");

            int i = 1;
            ws.Cell(i, 1).Value = "Seed";
            ws.Cell(i, 2).Value = Properties.Settings.Default.Seed;
            ws.Cell(i, 1).Style.Font.Bold = true;
            i += 2;     // Skip a row.

            // Item Randomization Settings
            ws.Cell(i, 1).Value = "Item Type";
            ws.Cell(i, 2).Value = "Rando Level";
            ws.Cell(i, 1).Style.Font.Bold = true;
            ws.Cell(i, 2).Style.Font.Bold = true;
            i++;

            var settings = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Armbands", ((RandomizationLevel)Properties.Settings.Default.RandomizeArmbands).ToString()),
                new Tuple<string, string>("Armor", ((RandomizationLevel)Properties.Settings.Default.RandomizeArmor).ToString()),
                new Tuple<string, string>("Belts", ((RandomizationLevel)Properties.Settings.Default.RandomizeBelts).ToString()),
                new Tuple<string, string>("Blasters", ((RandomizationLevel)Properties.Settings.Default.RandomizeBlasters).ToString()),
                new Tuple<string, string>("Creature Hides", ((RandomizationLevel)Properties.Settings.Default.RandomizeHides).ToString()),
                new Tuple<string, string>("Creature Weapons", ((RandomizationLevel)Properties.Settings.Default.RandomizeCreature).ToString()),
                new Tuple<string, string>("Droid Equipment", ((RandomizationLevel)Properties.Settings.Default.RandomizeDroid).ToString()),
                new Tuple<string, string>("Gauntlets", ((RandomizationLevel)Properties.Settings.Default.RandomizeGloves).ToString()),
                new Tuple<string, string>("Grenades", ((RandomizationLevel)Properties.Settings.Default.RandomizeGrenades).ToString()),
                new Tuple<string, string>("Implants", ((RandomizationLevel)Properties.Settings.Default.RandomizeImplants).ToString()),
                new Tuple<string, string>("Lightsabers", ((RandomizationLevel)Properties.Settings.Default.RandomizeLightsabers).ToString()),
                new Tuple<string, string>("Masks", ((RandomizationLevel)Properties.Settings.Default.RandomizeMask).ToString()),
                new Tuple<string, string>("Melee Weapons", ((RandomizationLevel)Properties.Settings.Default.RandomizeMelee).ToString()),
                new Tuple<string, string>("Mines", ((RandomizationLevel)Properties.Settings.Default.RandomizeMines).ToString()),
                new Tuple<string, string>("Pazaak Cards", ((RandomizationLevel)Properties.Settings.Default.RandomizePaz).ToString()),
                new Tuple<string, string>("Stims/Medpacs", ((RandomizationLevel)Properties.Settings.Default.RandomizeStims).ToString()),
                new Tuple<string, string>("Upgrades/Crystals", ((RandomizationLevel)Properties.Settings.Default.RandomizeUpgrade).ToString()),
                new Tuple<string, string>("Various", ((RandomizationLevel)Properties.Settings.Default.RandomizeVarious).ToString()),
            };

            foreach (var setting in settings)
            {
                ws.Cell(i, 1).Value = setting.Item1;
                ws.Cell(i, 2).Value = setting.Item2;
                ws.Cell(i, 1).Style.Font.Italic = true;
                i++;
            }

            i++;    // Skip a row.

            // Omitted Items
            ws.Cell(i, 1).Value = "Omitted Items";
            ws.Cell(i, 1).Style.Font.Bold = true;
            i++;

            foreach (var item in Globals.OmitItems)
            {
                ws.Cell(i, 1).Value = item;
                i++;
            }
            i++;    // Skip a row.

            // Randomized Items
            ws.Cell(i, 1).Value = "Has Changed";
            ws.Cell(i, 2).Value = "Original";
            ws.Cell(i, 3).Value = "Randomized";
            ws.Cell(i, 1).Style.Font.Bold = true;
            ws.Cell(i, 2).Style.Font.Bold = true;
            ws.Cell(i, 3).Style.Font.Bold = true;
            i++;

            var sortedLookup = LookupTable.OrderBy(kvp => kvp.Key);
            foreach (var kvp in sortedLookup)
            {
                ws.Cell(i, 1).Value = (kvp.Key != kvp.Value).ToString();
                ws.Cell(i, 2).Value = kvp.Key;
                ws.Cell(i, 3).Value = kvp.Value;
                if (kvp.Key != kvp.Value) ws.Cell(i, 1).Style.Font.FontColor = XLColor.Green;
                else ws.Cell(i, 1).Style.Font.FontColor = XLColor.Red;
                i++;
            }

            // Resize Columns
            ws.Column(1).AdjustToContents();
            ws.Column(2).AdjustToContents();
            ws.Column(3).AdjustToContents();
        }

        #region Regexes
        //Armor Regexes
        static List<Regex> ArmorRegs = new List<Regex>()
        {
            new Regex("^g1*_a_", RegexOptions.Compiled | RegexOptions.IgnoreCase),// All Armor

            new Regex("^g1*_a_class4", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Armor Class 4
            new Regex("^g1*_a_class5", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Armor Class 5
            new Regex("^g1*_a_class6", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Armor Class 6
            new Regex("^g1*_a_class7", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Armor Class 7
            new Regex("^g1*_a_class8", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Armor Class 8
            new Regex("^g1*_a_class9", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Armor Class 9
            new Regex("^g1*_a_clothes", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Clothes
            new Regex("^g1*_a_jedi", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Basic Robes
            new Regex("^g1*_a_kght", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Knight Robes
            new Regex("^g1*_a_mstr", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Master Robes

        };

        //Stims Regexes
        static List<Regex> StimsRegs = new List<Regex>()
        {
            new Regex("^g1*_i_(adrn|cmbt|medeq)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Stims/Medpacs

            new Regex("^g1*_i_adrn", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Adrenals
            new Regex("^g1*_i_cmbt", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Battle Stims
            new Regex("^g1*_i_medeq", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Medpacs
        };

        //Belt Regexs
        static List<Regex> BeltsRegs = new List<Regex>()
        {
            new Regex("^g1*_i_belt", RegexOptions.Compiled | RegexOptions.IgnoreCase)//All Belts
        };

        ////Various Regexes
        //private static Regex RegexBith { get { return new Regex("^g1*_i_bith", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Bith items
        //private static Regex Regexcredits { get { return new Regex("^g1*_i_credit", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Credits

        //Creature Hides
        static List<Regex> HidesRegs = new List<Regex>()
        {
            new Regex("^g1*_i_crhide", RegexOptions.Compiled | RegexOptions.IgnoreCase)//Creature Hides
        };

        //Droid equipment 
        static List<Regex> DroidRegs = new List<Regex>()
        {
            new Regex("^g1*_i_drd", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Droid Equipment

            new Regex("^g1*_i_drd.{0,2}plat", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Droid Plating
            new Regex("^g1*_i_drd(comspk|secspk)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Droid probes
            new Regex("^g1*_i_drd(mtn|snc)sen", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Droid Sensors
            new Regex("^g1*_i_drdrep", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Droid repair kits
            new Regex("^g1*_i_drdshld", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Droid Shields
            new Regex("^g1*_i_drdsrc", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Droid Equipment
            new Regex("^g1*_i_drdtrgcom", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Droid Computers
            new Regex("^g1*_i_drdutldev", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Droid Devices
        };

        //Armbands
        static List<Regex> ArmbandsRegs = new List<Regex>()
        {
            new Regex("^g1*_i_frarmbnds", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Armbands

            new Regex("^g1*_i_frarmbnds0", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Shields
            new Regex("^g1*_i_frarmbnds(1|2)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Stats
        };

        //Gauntlets
        static List<Regex> GlovesRegs = new List<Regex>()
        {
            new Regex("^g1*_i_gauntlet", RegexOptions.Compiled | RegexOptions.IgnoreCase)//Gloves
        };

        //Implants
        static List<Regex> ImplantsRegs = new List<Regex>()
        {
            new Regex("^g1*_i_implant", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Implants

            new Regex("^g1*_i_implant1", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Implant level 1
            new Regex("^g1*_i_implant2", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Implant level 2
            new Regex("^g1*_i_implant3", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Implant level 3
        };

        //Mask
        static List<Regex> MaskRegs = new List<Regex>()
        {
            new Regex("^g1*_i_mask", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Masks

            new Regex("^g1*_i_mask(08|09|10|11|13|16|17|18|22|23|24)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Mask No Armor Prof
            new Regex("^g1*_i_mask(01|02|03|04|05|07|19|20|21)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Mask Light
            new Regex("^g1*_i_mask(06|12|15)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Mask Medium
            new Regex("^g1*_i_mask14", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Mask Heavy
        };

        //Paz
        static List<Regex> PazRegs = new List<Regex>()
        {
            new Regex("^g1*_i_pazcard", RegexOptions.Compiled | RegexOptions.IgnoreCase)//Pazaak Cards
        };

        //Mines
        static List<Regex> MinesRegs = new List<Regex>()
        {
            new Regex("^g1*_i_trapkit", RegexOptions.Compiled | RegexOptions.IgnoreCase)//Mines
        };

        //Upgrades/Crystals
        static List<Regex> UpgradeRegs = new List<Regex>()
        {
            new Regex("^g1*_(i_upgrade|w_sbrcrstl)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Upgrades

            new Regex("^g1*_i_upgrade", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Normal Upgrades
            new Regex("(^g1*_w_sbrcrstl(0|1([1-3]|9))|^tat18_dragonprl)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Crystal Upgrades
            new Regex("^g1*_w_sbrcrstl(1[4-8]|2)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Crystal Colors
        };

        //Blaster
        static List<Regex> BlastersRegs = new List<Regex>()
        {
            new Regex("^g1*_(w_.*(bls*tr*|rfl|pstl|cstr)|i_bithitem)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Blasters

            new Regex("^g1*_w_.*(rptn)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Heavy Weapons
            new Regex("^g1*_(w_.*(pstl|hldoblst|hvyblstr|ionblstr)|i_bithitem)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Blaster Pistols
            new Regex("^g1*_w_.*(crbn|rfl|cstr)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Blaster Rifles
        };

        //Creature Weapons
        static List<Regex> CreatureRegs = new List<Regex>()
        {
            new Regex("^g1*_w_cr(go|sl)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Creature weapons

            new Regex("^g1*_w_crgore", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Piercing Creature Weapons
            new Regex("^g1*_w_crslash", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Slashing Creature Weapons
            new Regex("^g1*_w_crslprc", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Piercing/slashing Creature weapons
        };

        //Lightsabers
        static List<Regex> LightsabersRegs = new List<Regex>()
        {
            new Regex("^g1*_w_.{1,}sbr", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Lightsabers

            new Regex("^g1*_w_dblsbr", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Double Lightsabers
            new Regex("^g1*_w_(lght|drkjdi)sbr", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Regular Lightsabers
            new Regex("^g1*_w_shortsbr", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Short Lightsabers
        };

        //Grenades
        static List<Regex> GrenadesRegs = new List<Regex>()
        {
            new Regex("^g1*_w_(.*gren|thermldet)", RegexOptions.Compiled | RegexOptions.IgnoreCase)//Grenades
        };

        //Melee
        static List<Regex> MeleeRegs = new List<Regex>()
        {
            new Regex("^g1*_w_(stunbaton|war|.*swr*d|vi*bro|gaffi|qtrstaff)", RegexOptions.Compiled | RegexOptions.IgnoreCase),//All Melee Weapons

            new Regex("^g1*_w_stunbaton", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Stun Batons
            new Regex("^g1*_w_lngswrd", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Long Swords
            new Regex("^g1*_w_shortswrd", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Short Swords
            new Regex("^g1*_w_vbroshort", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Vibro Shortblades
            new Regex("^g1*_w_vbroswrd", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Vibroblades
            new Regex("^g1*_w_dblswrd", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Double Swords
            new Regex("^g1*_w_qtrstaff", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Quarter Staves
            new Regex("^g1*_w_vbrdblswd", RegexOptions.Compiled | RegexOptions.IgnoreCase),//Vibro Doubleblades
            new Regex("^g1*_w_war", RegexOptions.Compiled | RegexOptions.IgnoreCase),//War blade/axes
        };
        #endregion
    }
}
