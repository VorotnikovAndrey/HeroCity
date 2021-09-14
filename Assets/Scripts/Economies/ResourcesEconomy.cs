using System;
using System.Collections.Generic;
using System.Linq;
using ResourceSystem;
using UnityEngine;

namespace Economies
{
    [CreateAssetMenu(fileName = "ResourcesEconomyData", menuName = "Economy/Resources Economy")]
    public class ResourcesEconomy : EconomyFile
    {
        public List<ResourcesData> Data = new List<ResourcesData>();

        public ResourcesData Get(ResourceType type)
        {
            return Data.FirstOrDefault(x => x.Type == type);
        }
    }

    [Serializable]
    public class ResourcesData
    {
        public ResourceType Type;
        public int Value;
    }
}