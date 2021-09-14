using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Economies
{
    [CreateAssetMenu(fileName = "LocationsEconomyData", menuName = "Economy/Locations Economy")]
    public class LocationsEconomy : EconomyFile
    {
        public List<LocationData> Data = new List<LocationData>();

        public LocationData Get(string id)
        {
            return Data.FirstOrDefault(x => x.Id == id);
        }
    }

    [Serializable]
    public class LocationData
    {
        public string Id;
        public List<string> BuildingsIds = new List<string>();
    }
}