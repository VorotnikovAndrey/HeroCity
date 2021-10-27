using Newtonsoft.Json;

namespace Gameplay.Equipments
{
    public class ProductionItem : Item
    {
        [JsonProperty] public long TimeCreationTick;
    }
}