using Newtonsoft.Json;

namespace Gameplay.Characters
{
    public class Stats
    {
        [JsonProperty] public int MaxHP;
        [JsonProperty] public int CurrentHP;
        [JsonProperty] public float MovementSpeed = 2f;
    }
}