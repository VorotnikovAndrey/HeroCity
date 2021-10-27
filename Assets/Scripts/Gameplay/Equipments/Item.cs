using System;
using System.Collections.Generic;
using Economies;
using Gameplay.Characters;
using Newtonsoft.Json;

namespace Gameplay.Equipments
{
    [Serializable]
    public class Item
    {
        [JsonProperty] public string Id;
        [JsonProperty] public string Title;
        [JsonProperty] public string Description;
        [JsonProperty] public Rarity Rarity;
        [JsonProperty] public string IconId;
        [JsonProperty] public List<ResourcesData> Price = new List<ResourcesData>();
        [JsonProperty] public Stats Stats = new Stats();
    }
}