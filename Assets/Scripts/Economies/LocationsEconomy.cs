using System;
using System.Collections.Generic;
using UnityEngine;

namespace Economies
{
    [CreateAssetMenu(fileName = "LocationsEconomyData", menuName = "Economy/Locations Economy")]
    public class LocationsEconomy : EconomyFile
    {
        public List<LocationData> Data = new List<LocationData>();
    }

    [Serializable]
    public class LocationData
    {
        public string Id;
        public List<string> BuildingsIds = new List<string>();
    }
}