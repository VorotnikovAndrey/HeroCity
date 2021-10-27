using System;
using Newtonsoft.Json;

namespace Gameplay.Characters
{
    [Serializable]
    public class Stats
    {
        [JsonProperty] public StatTypeDictionary BaseStats = new StatTypeDictionary();
    }
}