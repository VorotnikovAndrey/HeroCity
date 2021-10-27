using Newtonsoft.Json;

namespace Gameplay.Characters
{
    public class CharacterStats : Stats
    {
        [JsonProperty] public int Level = 1;
        [JsonProperty] public int CurrentHealthPoint;
        [JsonProperty] public int CurrentManaPoint;
        [JsonProperty] public int MaxHealthPoint = 100;
        [JsonProperty] public int MaxManaPoint = 10;
    }
}