﻿using kotor_Randomizer_2.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace kotor_Randomizer_2.Interfaces
{
    public interface IGeneralSettings
    {
        Dictionary<string, Tuple<float, float, float>> FixedCoordinates { get; }
        SavePatchOptions GeneralSaveOptions { get; set; }
        ObservableCollection<QualityOfLifeOption> GeneralUnlockedDoors { get; set; }
        ObservableCollection<QualityOfLifeOption> GeneralLockedDoors { get; set; }
    }
}