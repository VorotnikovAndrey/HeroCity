using Economies;
using Gameplay.Building.Models;
using Source;
using UnityEngine;
using Utils;

namespace Gameplay.Building
{
    public static class BuildingModelFactory
    {
        public static BuildingModel Get(BuildingData data)
        {
            switch (data.Type)
            {
                case BuildingType.Default:
                    return GetDefaultModel(data);
                case BuildingType.Production:
                    return GetProductionModel(data);
                case BuildingType.Service:
                    return GetServiceModel(data);
                default:
                    Debug.LogError("Incorrect type".AddColorTag(Color.red));
                    return null;
            }
        }

        private static BuildingModel GetDefaultModel(BuildingData data)
        {
            return new BuildingModel
            {
                Id = data.Id,
                Type = data.Type,
                Stage = new EventVariable<int>(),
                State = new EventVariable<BuildingState>(data.State)
            };
        }

        private static ProductionBuildingModel GetProductionModel(BuildingData data)
        {
            return new ProductionBuildingModel
            {
                Id = data.Id,
                Type = data.Type,
                Stage = new EventVariable<int>(),
                State = new EventVariable<BuildingState>(data.State)
            };
        }

        private static ServiceBuildingModel GetServiceModel(BuildingData data)
        {
            return new ServiceBuildingModel
            {
                Id = data.Id,
                Type = data.Type,
                Stage = new EventVariable<int>(),
                State = new EventVariable<BuildingState>(data.State)
            };
        }
    }
}