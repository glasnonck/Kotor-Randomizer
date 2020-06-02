﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KotOR_IO;
using System.Text.RegularExpressions;


namespace kotor_Randomizer_2
{
    public static class ItemRando
    {
        public static void item_rando(Globals.KPaths paths)
        {
            KEY k = KReader.ReadKEY(File.OpenRead(paths.chitin));


        }


        #region Regexes
        //Armor Regexes
        private static Regex RegexArmor { get { return new Regex("^g1*_a_", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All armor
        private static Regex RegexArmorC4 { get { return new Regex("^g1*_a_class4", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Armor Class 4
        private static Regex RegexArmorC5 { get { return new Regex("^g1*_a_class5", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Armor Class 5
        private static Regex RegexArmorC6 { get { return new Regex("^g1*_a_class6", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Armor Class 6
        private static Regex RegexArmorC7 { get { return new Regex("^g1*_a_class7", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Armor Class 7
        private static Regex RegexArmorC8 { get { return new Regex("^g1*_a_class8", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Armor Class 8
        private static Regex RegexArmorC9 { get { return new Regex("^g1*_a_class9", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Armor Class 9
        private static Regex RegexArmorClothes { get { return new Regex("^g1*_a_clothes", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Clothes
        private static Regex RegexArmorRobes { get { return new Regex("^g1*_a_jedi", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Basic Robes
        private static Regex RegexArmorKnightRobes { get { return new Regex("^g1*_a_kght", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Knight Robes
        private static Regex RegexArmorMasterRobes { get { return new Regex("^g1*_a_mstr", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Master Robes

        //Stims Regexes
        private static Regex RegexStims { get { return new Regex("^g1*_i_(adrn|cmbt|medeq)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Stims/Medpacs
        private static Regex RegexStimsA { get { return new Regex("^g1*_i_adrn", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Adrenals
        private static Regex RegexStimsB { get { return new Regex("^g1*_i_cmbt", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Battle Stims
        private static Regex RegexStimsM { get { return new Regex("^g1*_i_medeq", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Medpacs

        //Belt Regexs
        private static Regex RegexBelts { get { return new Regex("^g1*_i_belt", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Belts

        //Various Regexes
        private static Regex RegexBith { get { return new Regex("^g1*_i_bith", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Bith items
        private static Regex Regexcredits { get { return new Regex("^g1*_i_credit", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Credits

        //Creature Hides
        private static Regex RegexHides { get { return new Regex("^g1*_i_crhide", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Creature Hides

        //Droid equipment 
        private static Regex RegexDroid { get { return new Regex("^g1*_i_drd", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Droid Equipment
        private static Regex RegexDroidPlating { get { return new Regex("^g1*_i_drd.{0,2}plat", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Droid Plating
        private static Regex RegexDroidProbes { get { return new Regex("^g1*_i_drd(comspk|secspk)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Droid probes
        private static Regex RegexDroidSensors { get { return new Regex("^g1*_i_drd(mtn|snc)sen", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Droid Sensors
        private static Regex RegexDroidRepair { get { return new Regex("^g1*_i_drdrep", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Droid repair kits
        private static Regex RegexDroidShields { get { return new Regex("^g1*_i_drdshld", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Droid Shields
        private static Regex RegexDroidScope { get { return new Regex("^g1*_i_drdsrc", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Droid Equipment
        private static Regex RegexDroidComputers { get { return new Regex("^g1*_i_drdtrgcom", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Droid Computers
        private static Regex RegexDroidDevice { get { return new Regex("^g1*_i_drdutldev", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Droid Devices

        //Armbands
        private static Regex RegexArmbands { get { return new Regex("^g1*_i_frarmbnds", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Armbands
        private static Regex RegexArmbandsSheild { get { return new Regex("^g1*_i_frarmbnds0", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Armbands
        private static Regex RegexArmbandsStat { get { return new Regex("^g1*_i_frarmbnds(1|2)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Armbands

        //Gauntlets
        private static Regex RegexGloves { get { return new Regex("^g1*_i_gauntlet", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Gloves

        //Implants
        private static Regex RegexImplants { get { return new Regex("^g1*_i_implant", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Implants
        private static Regex RegexImplants1 { get { return new Regex("^g1*_i_implant1", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Implant level 1
        private static Regex RegexImplants2 { get { return new Regex("^g1*_i_implant2", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Implant level 2
        private static Regex RegexImplants3 { get { return new Regex("^g1*_i_implant3", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Implant level 3

        //Mask
        private static Regex RegexMask { get { return new Regex("^g1*_i_mask", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Masks
        private static Regex RegexMaskN { get { return new Regex("^g1*_i_mask(08|09|10|11|13|16|17|18|22|23|24)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Mask No Armor Prof
        private static Regex RegexMaskL { get { return new Regex("^g1*_i_mask(01|02|03|04|05|07|19|20|21)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Mask Light
        private static Regex RegexMaskM { get { return new Regex("^g1*_i_mask(06|12|15)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Mask Medium
        private static Regex RegexMaskH { get { return new Regex("^g1*_i_mask14", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Mask Heavy

        //Paz
        private static Regex RegexPaz { get { return new Regex("^g1*_i_pazcard", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Pazaak Cards

        //Mines
        private static Regex RegexMines { get { return new Regex("^g1*_i_trapkit", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Mines

        //Upgrades/Crystals
        private static Regex RegexUpgrade { get { return new Regex("^g1*_(i_upgrade|w_sbrcrstl)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Upgrades
        private static Regex RegexUpgradeNorm { get { return new Regex("^g1*_i_upgrade", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Normal Upgrades
        private static Regex RegexUpgradeCryst { get { return new Regex("^g1*_w_sbrcrstl", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Crystal Upgrades

        //Blaster
        private static Regex RegexBlasters { get { return new Regex("^g1*_w_.*(bls*tr*|rfl|pstl|cstr)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Blasters
        private static Regex RegexBlastersHeavy { get { return new Regex("^g1*_w_.*(rptn)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Heavy Blasters
        private static Regex RegexBlastersPistol { get { return new Regex("^g1*_w_.*(pstl|hldoblst|hvyblstr|ionblstr)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Blaster Pistols
        private static Regex RegexBlastersRifle { get { return new Regex("^g1*_w_.*(crbn|rfl|cstr)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Blaster Rifles

        //Creature Weapons
        private static Regex RegexCreature { get { return new Regex("^g1*_w_cr(go|sl)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Creature weapons
        private static Regex RegexCreaturePierce { get { return new Regex("^g1*_w_crgore", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Piercing Creature Weapons
        private static Regex RegexCreatureSlash { get { return new Regex("^g1*_w_crslash", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Slashing Creature Weapons
        private static Regex RegexCreaturePierceSlash { get { return new Regex("^g1*_w_crslprc", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Piercing/slashing Creature weapons

        //Lightsabers
        private static Regex RegexLightsabers { get { return new Regex("^g1*_w_.{1,}sbr", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Lightsabers
        private static Regex RegexLightsabersDouble { get { return new Regex("^g1*_w_dblsbr", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Double Lightsabers
        private static Regex RegexLightsabersRegular { get { return new Regex("^g1*_w_(lght|drkjdi)sbr", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Regular Lightsabers
        private static Regex RegexLightsabersShort { get { return new Regex("^g1*_w_shortsbr", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Short Lightsabers

        //Grenades
        private static Regex RegexGrenades { get { return new Regex("^g1*_w_(.*gren|thermldet)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Grenades

        //Melee
        private static Regex RegexMelee { get { return new Regex("^g1*_w_(stunbaton|war|.*swr*d|vi*bro|gaffi|qtrstaff)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//All Melee Weapons
        private static Regex RegexMeleeBatons { get { return new Regex("^g1*_w_stunbaton", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Stun Batons
        private static Regex RegexMeleeLongSword { get { return new Regex("^g1*_w_lngswrd", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Long Swords
        private static Regex RegexMeleeShortSword { get { return new Regex("^g1*_w_shortswrd", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Short Swords
        private static Regex RegexMeleeVibroShort { get { return new Regex("^g1*_w_vbroshort", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Vibro Shortblades
        private static Regex RegexMeleeVibro { get { return new Regex("^g1*_w_vbroswrd", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Vibroblades
        private static Regex RegexMeleeDoubleSword { get { return new Regex("^g1*_w_dblswrd", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Double Swords
        private static Regex RegexMeleeQuarterStaff { get { return new Regex("^g1*_w_qtrstaff", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Quarter Staves
        private static Regex RegexMeleeVibroDouble { get { return new Regex("^g1*_w_vbrdblswd", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//Vibro Doubleblades
        private static Regex RegexMeleeWar { get { return new Regex("^g1*_w_war", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }//War blade/axes

        #endregion
    }

}
