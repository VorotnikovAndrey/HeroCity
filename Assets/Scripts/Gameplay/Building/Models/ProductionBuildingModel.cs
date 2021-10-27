using System;
using System.Collections.Generic;
using Gameplay.Craft;
using Newtonsoft.Json;

namespace Gameplay.Building.Models
{
    [Serializable]
    public class ProductionBuildingModel : BuildingModel
    {
        [JsonProperty] public List<ProductionData> ProductionData = new List<ProductionData>();
    }
}