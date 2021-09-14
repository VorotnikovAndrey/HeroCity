using System;
using System.Collections.Generic;
using Gameplay;
using ResourceSystem;
using UnityEngine;

namespace Economies
{
    [CreateAssetMenu(fileName = "ResourcesEconomyData", menuName = "Economy/Resources Economy")]
    public class ResourcesEconomy : EconomyFile
    {
        public List<ResourcesData> Data = new List<ResourcesData>();
    }

    [Serializable]
    public class ResourcesData
    {
        public ResourceType Type;
        public int Value;
    }
}