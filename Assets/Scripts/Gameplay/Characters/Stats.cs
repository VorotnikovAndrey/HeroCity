using Newtonsoft.Json;

namespace Gameplay.Characters
{
    public class Stats
    {
        [JsonProperty] public int Level = 1;
        [JsonProperty] public int CurrentHealthPoint;
        [JsonProperty] public int CurrentManaPoint;
        [JsonProperty] public int MaxHealthPoint = 100;
        [JsonProperty] public int MaxManaPoint = 10;
        [JsonProperty] public int Strength = 10;
        [JsonProperty] public int Dexterity = 10;
        [JsonProperty] public int Intelligence = 10;
        [JsonProperty] public int Concentration = 10;
        [JsonProperty] public float MovementSpeed = 2f;
        [JsonProperty] public float CriticalChance = 0.1f;
        [JsonProperty] public float CriticalFactor = 1f;
        [JsonProperty] public float Accuracy = 0.75f;
        [JsonProperty] public float Evasion = 0.75f;
        [JsonProperty] public float BlockChance = 0.5f;
        [JsonProperty] public float Protection = 10f;
        [JsonProperty] public float MagicProtection = 5f;
    }
}