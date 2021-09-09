using System;
using System.Collections.Generic;
using Events;
using Gameplay;
using Gameplay.Building.Models;
using Gameplay.Locations.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace UserSystem
{
    public class UserModel : EventBehavior
    {
        [JsonProperty] public Dictionary<ResourceType, int> Resources;
        [JsonProperty] public Dictionary<string, LocationModel> Locations;
        [JsonProperty] public Dictionary<string, BuildingModel> Buildings;
        [JsonProperty] public string LocationId;
        [JsonProperty] public TimeSpan Time;

        public UserModel(Dictionary<ResourceType, int> resources)
        {
            Resources = resources;
        }

        public void AddResourceValue(ResourceType type, int value)
        {
            ModifyResourceValue(type, Resources[type] + value);
        }

        public void ModifyResourceValue(ResourceType type, int value)
        {
            value = Mathf.Clamp(value, 0, int.MaxValue);
            int prevValue = Resources[type];
            Resources[type] = value;

            EventAggregator.SendEvent(new ResourceModifiedEvent
            {
                NewValue = value,
                PrevValue = prevValue,
                Type = type
            });
        }
    }
}