using System;
using System.Collections.Generic;
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
        public int Stages;
        public BuildingType Type;
        public BuildingState State;
    }
}