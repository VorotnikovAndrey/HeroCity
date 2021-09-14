using System;
using System.Collections.Generic;
using System.Linq;
using Economies;
using Events;
using Gameplay;
using Gameplay.Locations.Models;
using Newtonsoft.Json;
using ResourceSystem;
using UnityEngine;

namespace UserSystem
{
    public class UserModel : EventBehavior
    {
        [JsonProperty] public Dictionary<ResourceType, int> Resources;
        [JsonProperty] public Dictionary<string, LocationModel> Locations;
        [JsonProperty] public string CurrentLocationId;
        [JsonProperty] public TimeSpan Time;

        public UserModel(Dictionary<ResourceType, int> resources)
        {
            Resources = resources;
        }

        public LocationModel CurrentLocation => Locations[CurrentLocationId];
    }
}