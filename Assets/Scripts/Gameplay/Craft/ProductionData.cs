using System;
using Newtonsoft.Json;
using Source;

namespace Gameplay.Craft
{
    [Serializable]
    public class ProductionData
    {
        [JsonProperty] public string ProductionId;
        [JsonProperty] public ProductionType ProductionType;
        [JsonProperty] public string ProductionBuildingId;
        [JsonProperty] public long ProductionStartUnixTime;
        [JsonProperty] public long ProductionEndUnixTime;
        [JsonProperty] public bool Finished;

        [JsonIgnore] public EventVariable<long> TimeLeft = new EventVariable<long>();
    }
}