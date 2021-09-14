using System;
using Newtonsoft.Json;
using Source;

namespace Gameplay.Building.Models
{
    [Serializable]
    public class BuildingModel
    {
        [JsonProperty] public string Id;
        [JsonProperty] public BuildingType Type;
        [JsonProperty] public EventVariable<int> Stage = new EventVariable<int>();
        [JsonProperty] public EventVariable<BuildingState> State = new EventVariable<BuildingState>();
        [JsonProperty] public long UpgradeEndUnixTime;
    }
}