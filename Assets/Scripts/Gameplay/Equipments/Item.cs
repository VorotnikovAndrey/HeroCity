using System;
using Newtonsoft.Json;

namespace Gameplay.Equipments
{
    [Serializable]
    public class Item
    {
        [JsonProperty] public string Title;
        [JsonProperty] public string Description;
        [JsonProperty] public Rarity Rarity;
        [JsonProperty] public string IconId;
    }
}