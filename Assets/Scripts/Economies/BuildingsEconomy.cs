using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Building;
using UnityEngine;

namespace Economies
{
    [CreateAssetMenu(fileName = "BuildingsEconomyData", menuName = "Economy/Buildings Economy")]
    public class BuildingsEconomy : EconomyFile
    {
        public List<BuildingData> Data = new List<BuildingData>();

        public BuildingData Get(string id)
        {
            return Data.FirstOrDefault(x => x.Id == id);
        }
    }

    [Serializable]
    public class BuildingData
    {
        public string Id;
        public BuildingType Type;
        public BuildingState State;
        public List<BuildingUpgradeData> Upgrades = new List<BuildingUpgradeData>();
    }

    [Serializable]
    public class BuildingUpgradeData
    {
        public int Stage;
        public int Duration;
        public List<ResourcesData> Price = new List<ResourcesData>();
        public List<string> ImprovementDependencies = new List<string>();
        public List<string> ImprovementOpen = new List<string>();
    }
}