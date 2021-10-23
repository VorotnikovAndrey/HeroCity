using System;
using System.Collections.Generic;
using Gameplay.Characters.Models;
using Gameplay.Locations.Models;
using Newtonsoft.Json;
using ResourceSystem;

namespace UserSystem
{
    public class UserModel : EventBehavior
    {
        [JsonProperty] public Dictionary<ResourceType, int> Resources;
        [JsonProperty] public Dictionary<string, LocationModel> Locations;
        [JsonProperty] public string CurrentLocationId;
        [JsonProperty] public TimeSpan Time;
        [JsonProperty] public DateTime LastPlayTime;
        [JsonProperty] public List<string> Improvement;
        [JsonProperty] public List<BaseCharacterModel> Characters;

        public UserModel(Dictionary<ResourceType, int> resources)
        {
            Resources = resources;
        }

        public LocationModel CurrentLocation => Locations[CurrentLocationId];
    }
}