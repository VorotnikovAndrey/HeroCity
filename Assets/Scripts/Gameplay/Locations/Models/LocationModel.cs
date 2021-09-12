using System;
using System.Collections.Generic;
using Gameplay.Building.Models;
using Newtonsoft.Json;

namespace Gameplay.Locations.Models
{
    [Serializable]
    public class LocationModel
    {
        [JsonProperty] public string LocationId;
        [JsonProperty] public Dictionary<string, BuildingModel> Buildings;
    }
}