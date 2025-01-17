﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kotor_Randomizer_2
{
    public partial class ModuleForm : Form
    {
        // Prevents Construction from triggering certain events
        private bool Constructed = false;

        #region Public Members

        public ModuleForm()
        {
            InitializeComponent();
            SetBorder(flpSaveData, Color.FromArgb(0, 175, 255), 1, BorderStyle.None);
            SetBorder(flpTimeSavers, Color.FromArgb(0, 175, 255), 1, BorderStyle.None);
            SetBorder(flpUnlockDoors, Color.FromArgb(0, 175, 255), 1, BorderStyle.None);
            SetBorder(RandomizedListBox, Color.FromArgb(0, 175, 255), 1, BorderStyle.None);
            SetBorder(OmittedListBox, Color.FromArgb(0, 175, 255), 1, BorderStyle.None);
            SetBorder(lblWIP, Color.FromArgb(211, 216, 8), 1, BorderStyle.None);
            SetBorder(pnlGoals, Color.FromArgb(0, 175, 255), 1, BorderStyle.None);
            SetBorder(pnlGlitches, Color.FromArgb(0, 175, 255), 1, BorderStyle.None);
            SetBorder(pnlOther, Color.FromArgb(0, 175, 255), 1, BorderStyle.None);

            var settings = Properties.Settings.Default;

            // Set up the controls
            RandomizedListBox.DisplayMember = "name";
            OmittedListBox.DisplayMember = "name";

            cbDeleteMilestones.Checked = !settings.ModuleExtrasValue.HasFlag(ModuleExtras.NoSaveDelete);
            cbSaveMiniGame.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.SaveMiniGames);
            cbSaveAllMods.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.SaveAllModules);
            cbSaveMiniGame.Enabled = !cbSaveAllMods.Checked; // If all save checked, disable mg save checkbox.

            cbVulkSpiceLZ.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.VulkarSpiceLZ);
            cbFixDream.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.FixDream);
            cbFixMindPrison.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.FixMindPrison);
            cbFixCoordinates.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.FixCoordinates);

            cbUnlockDanRuins.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockDanRuins);
            cbUnlockGalaxyMap.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockGalaxyMap);
            cbUnlockKorValley.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockKorValley);
            cbUnlockLevHangar.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockLevElev);
            cbEnableLevHangarElev.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.EnableLevHangarElev);
            cbUnlockManHangar.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockManHangar);
            cbUnlockManEmbassy.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockManEmbassy);
            cbUnlockStaBastila.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockStaBastila);
            cbUnlockTarUndercity.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockTarUndercity);
            cbUnlockTarVulkar.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockTarVulkar);
            cbUnlockUnkSummit.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockUnkSummit);
            cbUnlockUnkTempleExit.Checked = settings.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockUnkTempleExit);

            // Initialize reachability settings
            cbUseRandoRules.Checked = settings.UseRandoRules;
            cbReachability.Checked = settings.VerifyReachability;

            pnlGoals.Enabled = cbReachability.Checked;
            pnlGlitches.Enabled = cbReachability.Checked;
            pnlOther.Enabled = cbReachability.Checked;

            cbIgnoreOnceEdges.Checked = settings.IgnoreOnceEdges;
            cbStrongGoals.Checked = settings.StrongGoals;
            cbGoalMalak.Checked = settings.GoalIsMalak;
            cbGoalStarMaps.Checked = settings.GoalIsStarMaps;
            cbGoalPazaak.Checked = settings.GoalIsPazaak;
            cbGoalParty.Checked = settings.GoalIsParty;
            cbGlitchClip.Checked = settings.AllowGlitchClip;
            cbGlitchDlz.Checked = settings.AllowGlitchDlz;
            cbGlitchFlu.Checked = settings.AllowGlitchFlu;
            cbGlitchGpw.Checked = settings.AllowGlitchGpw;

            PresetComboBox.DataSource = Globals.K1_MODULE_OMIT_PRESETS.Keys.ToList();
            Constructed = true;

            if (Properties.Settings.Default.LastPresetComboIndex < 0)
            {
                Properties.Settings.Default.LastPresetComboIndex = -1;
                PresetComboBox.SelectedIndex = Properties.Settings.Default.LastPresetComboIndex;
            }
            else
            {
                PresetComboBox.SelectedIndex = Properties.Settings.Default.LastPresetComboIndex;
                Properties.Settings.Default.ModulePresetSelected = true;
                LoadPreset(PresetComboBox.Text);
            }

            UpdateListBoxes();
        }

        #endregion
        #region Private Members

        // Stores omitted modules
        private void UpdateOmittedModulesSetting()
        {
            Properties.Settings.Default.LastPresetComboIndex = PresetComboBox.SelectedIndex;
            Properties.Settings.Default.OmittedModules.Clear();
            Properties.Settings.Default.OmittedModules.AddRange(Globals.BoundModules.Where(x => x.Omitted).Select(x => x.Code).ToArray());
        }

        // Makes list work
        private void UpdateListBoxes()
        {
            RandomizedListBox.DataSource = Globals.BoundModules.Where(x => !x.Omitted).ToList();
            RandomizedListBox.Update();
            OmittedListBox.DataSource = Globals.BoundModules.Where(x => x.Omitted).ToList();
            OmittedListBox.Update();
            UpdateOmittedModulesSetting();
        }

        // How we load the built in presets. (May be subject to change if I change my mind about how I want User-presets to work.)
        private void LoadPreset(string preset)
        {
            if (PresetComboBox.SelectedIndex < 0 || !Properties.Settings.Default.ModulePresetSelected) { return; }

            Properties.Settings.Default.LastPresetComboIndex = PresetComboBox.SelectedIndex;
            for (int i = 0; i < Globals.BoundModules.Count; i++)
            {
                Globals.BoundModules[i] = new Globals.Mod_Entry(Globals.BoundModules[i].Code, false);

                if (Globals.K1_MODULE_OMIT_PRESETS[preset].Contains(Globals.BoundModules[i].Code))
                {
                    Globals.BoundModules[i] = new Globals.Mod_Entry(Globals.BoundModules[i].Code, true);
                }
            }
        }

        /// <summary>
        /// Adds a border to the given control by adding a panel that contains it.
        /// </summary>
        private void SetBorder(Control ctl, Color col, int width, BorderStyle style)
        {
            Panel pnl = new Panel();
            pnl.BorderStyle = style;
            pnl.Size = new Size(ctl.Width + width * 2, ctl.Height + width * 2);
            pnl.Location = new Point(ctl.Left - width, ctl.Top - width);
            pnl.BackColor = col;
            pnl.Visible = true;
            pnl.Parent = ctl.Parent;
            ctl.Parent = pnl;
            ctl.Location = new Point(width, width);
        }

        #endregion
        #region Events
        // ListBox Functions
        private void RandomizedListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PresetComboBox.SelectedIndex = -1;

            for (int i = 0; i < Globals.BoundModules.Count; i++)
            {
                if (RandomizedListBox.SelectedItems.Contains(Globals.BoundModules[i]))
                {
                    Globals.BoundModules[i] = new Globals.Mod_Entry(Globals.BoundModules[i].Code, true);
                }
            }
            UpdateListBoxes();
        }

        private void OmittedListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PresetComboBox.SelectedIndex = -1;

            for (int i = 0; i < Globals.BoundModules.Count; i++)
            {
                if (OmittedListBox.SelectedItems.Contains(Globals.BoundModules[i]))
                {
                    Globals.BoundModules[i] = new Globals.Mod_Entry(Globals.BoundModules[i].Code, false);
                }
            }
            UpdateListBoxes();
        }

        private void RandomizedListBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                PresetComboBox.SelectedIndex = -1;

                for (int i = 0; i < Globals.BoundModules.Count; i++)
                {
                    if (RandomizedListBox.SelectedItems.Contains(Globals.BoundModules[i]))
                    {
                        Globals.BoundModules[i] = new Globals.Mod_Entry(Globals.BoundModules[i].Code, true);
                    }
                }
                UpdateListBoxes();
            }
        }

        private void OmittedListBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                PresetComboBox.SelectedIndex = -1;

                for (int i = 0; i < Globals.BoundModules.Count; i++)
                {
                    if (OmittedListBox.SelectedItems.Contains(Globals.BoundModules[i]))
                    {
                        Globals.BoundModules[i] = new Globals.Mod_Entry(Globals.BoundModules[i].Code, false);
                    }
                }
                UpdateListBoxes();
            }
        }

        // Built-in Preset control functions
        private void PresetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Constructed) return;

            if (PresetComboBox.SelectedIndex >= 0)
            {
                Properties.Settings.Default.ModulePresetSelected = true;
                LoadPreset(PresetComboBox.Text);
                UpdateListBoxes();
            }
            else
            {
                Properties.Settings.Default.ModulePresetSelected = false;
            }
        }

        private void PresetComboBox_Enter(object sender, EventArgs e)
        {
            Properties.Settings.Default.ModulePresetSelected = true;
        }

        // Check box functions
        private void ModuleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.LastPresetComboIndex = PresetComboBox.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void cbDeleteMilestones_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.NoSaveDelete;
        }

        private void cbSaveMiniGame_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.SaveMiniGames;
        }

        private void cbSaveAllMods_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.SaveAllModules;
            cbSaveMiniGame.Checked = true;
            cbSaveMiniGame.Enabled = !cbSaveAllMods.Checked;    // If all save checked, disable mg save.
        }

        private void cbFixDream_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.FixDream;
        }

        private void cbUnlockGalaxyMap_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockGalaxyMap;
        }

        private void cbFixCoordinates_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.FixCoordinates;
        }

        private void lblRandomized_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString(Properties.Settings.Default.ModuleExtrasValue), "Active Settings");
        }

        private void cbFixMindPrison_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.FixMindPrison;
        }

        private void cbSpeedySuit_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.FastEnvirosuit;
        }

        private void cbUnlockDanRuins_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockDanRuins;
        }

        private void cbUnlockLevHangarAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockLevElev;
        }

        private void cbEnableLevHangarElevator_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.EnableLevHangarElev;
        }

        private void cbUnlockManSub_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockManEmbassy;
        }

        private void cbUnlockStaBastila_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockStaBastila;
        }

        private void cbUnlockUnkSummit_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockUnkSummit;
        }

        private void cbUnlockKorValley_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockKorValley;
        }

        private void cbUnlockManHangar_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockManHangar;
        }

        private void cbUnlockTarUndercity_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockTarUndercity;
        }

        private void cbUnlockTarVulkar_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockTarVulkar;
        }

        private void cbUnlockUnkTempleExit_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.UnlockUnkTempleExit;
        }

        private void cbVulkSpiceLZ_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.ModuleExtrasValue ^= ModuleExtras.VulkarSpiceLZ;
        }

        private void cbReachability_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.VerifyReachability = cbReachability.Checked;
            pnlGoals.Enabled = cbReachability.Checked;
            pnlGlitches.Enabled = cbReachability.Checked;
            pnlOther.Enabled = cbReachability.Checked;
        }

        private void cbGoalMalak_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.GoalIsMalak = cbGoalMalak.Checked;
        }

        private void cbGoalStarMaps_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.GoalIsStarMaps = cbGoalStarMaps.Checked;
        }

        private void cbGoalPazaak_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.GoalIsPazaak = cbGoalPazaak.Checked;
        }

        private void cbGoalParty_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.GoalIsParty = cbGoalParty.Checked;
        }

        private void cbGlitchClip_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.AllowGlitchClip = cbGlitchClip.Checked;
        }

        private void cbGlitchDlz_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.AllowGlitchDlz = cbGlitchDlz.Checked;
        }

        private void cbGlitchFlu_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.AllowGlitchFlu = cbGlitchFlu.Checked;
        }

        private void cbGlitchGpw_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.AllowGlitchGpw = cbGlitchGpw.Checked;
        }

        private void cbAllowOnceEdges_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.IgnoreOnceEdges = cbIgnoreOnceEdges.Checked;
        }

        private void cbStrongGoals_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.StrongGoals = cbStrongGoals.Checked;
        }

        private void cbUseRandoRules_CheckedChanged(object sender, EventArgs e)
        {
            if (!Constructed) { return; }
            Properties.Settings.Default.UseRandoRules = cbUseRandoRules.Checked;
        }

        private void ModuleForm_Activated(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.LastPresetComboIndex == -2)
            {
                Properties.Settings.Default.LastPresetComboIndex = -1;
                PresetComboBox.SelectedIndex = -1;
                UpdateListBoxes();
                Constructed = false;

                // Load new fix settings.
                cbDeleteMilestones.Checked = !Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.NoSaveDelete);
                cbSaveMiniGame.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.SaveMiniGames);
                cbSaveAllMods.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.SaveAllModules);
                cbSaveMiniGame.Enabled = !cbSaveAllMods.Checked; // If all save checked, disable mg save checkbox.

                cbVulkSpiceLZ.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.VulkarSpiceLZ);
                cbFixDream.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.FixDream);
                cbFixMindPrison.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.FixMindPrison);
                cbFixCoordinates.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.FixCoordinates);
                cbUnlockDanRuins.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockDanRuins);
                cbUnlockGalaxyMap.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockGalaxyMap);
                cbUnlockLevHangar.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockLevElev);
                cbEnableLevHangarElev.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.EnableLevHangarElev);
                cbUnlockManEmbassy.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockManEmbassy);
                cbUnlockStaBastila.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockStaBastila);
                cbUnlockUnkSummit.Checked = Properties.Settings.Default.ModuleExtrasValue.HasFlag(ModuleExtras.UnlockUnkSummit);

                // Load new reachability settings.
                cbUseRandoRules.Checked = Properties.Settings.Default.UseRandoRules;
                cbReachability.Checked = Properties.Settings.Default.VerifyReachability;
                cbIgnoreOnceEdges.Checked = Properties.Settings.Default.IgnoreOnceEdges;
                cbStrongGoals.Checked = Properties.Settings.Default.StrongGoals;
                cbGoalMalak.Checked = Properties.Settings.Default.GoalIsMalak;
                cbGoalStarMaps.Checked = Properties.Settings.Default.GoalIsStarMaps;
                cbGoalPazaak.Checked = Properties.Settings.Default.GoalIsPazaak;
                cbGoalParty.Checked = Properties.Settings.Default.GoalIsParty;
                cbGlitchClip.Checked = Properties.Settings.Default.AllowGlitchClip;
                cbGlitchDlz.Checked = Properties.Settings.Default.AllowGlitchDlz;
                cbGlitchFlu.Checked = Properties.Settings.Default.AllowGlitchFlu;
                cbGlitchGpw.Checked = Properties.Settings.Default.AllowGlitchGpw;

                pnlGoals.Enabled = cbReachability.Checked;
                pnlGlitches.Enabled = cbReachability.Checked;
                pnlOther.Enabled = cbReachability.Checked;

                Constructed = true;
            }
        }

        #endregion
    }
}
