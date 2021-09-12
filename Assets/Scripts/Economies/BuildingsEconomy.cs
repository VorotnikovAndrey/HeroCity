using System;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Building;
using UnityEngine;

namespace Economies
{
    [CreateAssetMenu(fileName = "BuildingsEconomyData", menuName = "Economy/Buildings Economy")]
    public class BuildingsEconomy : EconomyFile
    {
        public List<BuildingData> Data = new List<BuildingData>();
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
    }
}