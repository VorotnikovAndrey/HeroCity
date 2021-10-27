using Newtonsoft.Json;

namespace Gameplay.Equipments
{
    public class CraftedItem : Item
    {
        [JsonProperty] public long TimeCreationTick;
    }
}