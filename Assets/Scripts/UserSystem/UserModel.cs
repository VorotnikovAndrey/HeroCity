using System.Collections.Generic;
using Events;
using Gameplay;
using Newtonsoft.Json;
using UnityEngine;

namespace UserSystem
{
    public class UserModel : EventBehavior
    {
        [JsonProperty] private Dictionary<ResourceType, int> Resources;

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

        public int GetResourceValue(ResourceType type)
        {
            return Resources[type];
        }
    }
}