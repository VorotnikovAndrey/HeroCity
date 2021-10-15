using Newtonsoft.Json;

namespace Gameplay.Equipments
{
    public class Item
    {
        [JsonProperty] public string Title;
        [JsonProperty] public string Description;
        [JsonProperty] public ItemSlotType SlotType;
        [JsonProperty] public Rarity Rarity;
        [JsonProperty] public string IconId;
        [JsonProperty] public bool Equipped;
    }
}