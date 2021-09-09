using System;
using Newtonsoft.Json;

namespace Gameplay.Building.Models
{
    [Serializable]
    public class BuildingModel
    {
        [JsonProperty] public string Id;
        [JsonProperty] public int Stage;
        [JsonProperty] public BuildingType Type;
        [JsonProperty] public BuildingState State;
    }
}