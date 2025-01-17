﻿using kotor_Randomizer_2.DTOs;
using kotor_Randomizer_2.Digraph;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace kotor_Randomizer_2.Interfaces
{
    /// <summary>
    /// Settings related to randomizing modules.
    /// </summary>
    public interface IRandomizeModules
    {
        bool DoRandomizeModules { get; }

        bool ModuleAllowGlitchClip { get; set; }
        bool ModuleAllowGlitchDlz { get; set; }
        bool ModuleAllowGlitchFlu { get; set; }
        bool ModuleAllowGlitchGpw { get; set; }

        bool ModuleLogicStrongGoals { get; set; }
        bool ModuleLogicIgnoreOnceEdges { get; set; }
        bool ModuleLogicRandoRules { get; set; }
        bool ModuleLogicReachability { get; set; }

        ObservableCollection<ReachabilityGoal> ModuleGoalList { get; set; }

        //bool ModuleGoalIsMalak { get; set; }
        //bool ModuleGoalIsPazaak { get; set; }
        //bool ModuleGoalIsStarMap { get; set; }
        //bool ModuleGoalIsFullParty { get; set; }

        ObservableCollection<ModuleVertex> ModuleRandomizedList { get; set; }
        ObservableCollection<ModuleVertex> ModuleOmittedList { get; set; }
        ObservableCollection<string> ModulePresetOptions { get; set; }
        Dictionary<string, List<string>> ModuleOmitPresets { get; }

        string ModuleShufflePreset { get; set; }
    }
}
