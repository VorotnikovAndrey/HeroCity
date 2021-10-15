using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gameplay.Equipments
{
    [Serializable]
    public class CharacterInventory
    {
        [JsonProperty] public List<Item> Items = new List<Item>();
    }
}