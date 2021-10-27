using System;
using Newtonsoft.Json;

namespace Gameplay.Equipments
{
    [Serializable]
    public class EquipingItem : ProductionItem
    {
        [JsonProperty] public EquipSlotType EquipSlotType;
        [JsonProperty] public bool Equipped;
    }
}