﻿using kotor_Randomizer_2.DTOs;
using System;
using System.Collections.Generic;

namespace kotor_Randomizer_2.Models
{
    /// <summary>
    /// Encapsulates template items that can be randomized within the game.
    /// </summary>
    public partial class RandomizableItem
    {
        /// <summary>
        /// UNUSED. Constructs the object by parsing the ID and Tags from strings.
        /// </summary>
        public RandomizableItem(string id = "", string tags = "")
        {
            if (!string.IsNullOrWhiteSpace(id)) ID = int.Parse(id);
            if (!string.IsNullOrWhiteSpace(tags)) Tags.AddRange(tags.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary> Unique identifier for the item. </summary>
        public int ID { get; set; }
        /// <summary> Unique item code string. </summary>
        public string Code { get; set; }
        /// <summary> Description of the item. </summary>
        public string Label { get; set; }
        /// <summary> Label used to group this item with other similar items. </summary>
        public string Category { get; set; }
        /// <summary> Enum used to group this item with other similar items. </summary>
        public ItemRandoCategory CategoryEnum { get; set; }
        /// <summary> UNUSED. Collection of tags that identify item groups. </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"[{Code}] {Label}";
        }
    }
}
