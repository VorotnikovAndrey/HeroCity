using System;
using System.Collections.Generic;
using Gameplay.Craft;
using Newtonsoft.Json;

namespace Gameplay.Equipments
{
    [Serializable]
    public class WeaponItem : EquipingItem
    {
        [JsonProperty] public WeaponType WeaponType;
        [JsonProperty] public List<string> AffixesIds;
    }
}