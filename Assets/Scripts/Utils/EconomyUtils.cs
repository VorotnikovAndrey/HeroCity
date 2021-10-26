using System;
using Economies;
using ResourceSystem;

namespace Utils
{
    public static class EconomyUtils
    {
        public static ResourcesData GetPrice(string type, int value)
        {
            var result = new ResourcesData
            {
                Type = (ResourceType)Enum.Parse(typeof(ResourceType), type),
                Value = value
            };

            return result;
        }
    }
}