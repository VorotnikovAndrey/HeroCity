using System;
using System.Collections.Generic;
using Events;
using Gameplay;
using Gameplay.Locations.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace UserSystem
{
    public class UserModel : EventBehavior
    {
        [JsonProperty] public Dictionary<ResourceType, int> Resources { get; private set; }
        [JsonProperty] public Dictionary<string, LocationModel> Locations { get; private set; } = new Dictionary<string, LocationModel>();
        [JsonProperty] public string LocationId { get; private set; }
        [JsonProperty] public TimeSpan Time { get; private set; }

        public UserModel(Dictionary<ResourceType, int> resources)
        {
            Resources = resources;
        }

        public void SetTime(TimeSpan value)
        {
            Time = value;
        }

        public void SetLocation(string locationId)
        {
            LocationId = locationId;
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

        public int GetResourceValue(ResourceType type)
        {
            return Resources[type];
        }
    }
}