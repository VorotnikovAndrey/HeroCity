using System;
using System.Collections.Generic;
using Gameplay.Craft;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Equipments
{
    [Serializable]
    public class WeaponItem : EquipingItem
    {
        [JsonProperty] public WeaponType WeaponType;
        [JsonProperty] public List<string> AffixesIds;
        [JsonProperty] public Vector2 Damage = Vector2.zero;
    }
}