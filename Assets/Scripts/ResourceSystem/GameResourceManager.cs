using System;
using System.Collections.Generic;
using System.Linq;
using Economies;
using Events;
using UnityEngine;
using UserSystem;
using Utils.Events;
using Zenject;

namespace ResourceSystem
{
    public class GameResourceManager
    {
        private UserManager _userManager;
        private EventAggregator _eventAggregator;

        [Inject]
        public GameResourceManager(EventAggregator eventAggregator, UserManager userManager)
        {
            _userManager = userManager;
            _eventAggregator = eventAggregator;
        }

        public void AddResourceValue(ResourceType type, int value)
        {
            ModifyResourceValue(type, _userManager.CurrentUser.Resources[type] + value);
        }

        public void ModifyResourceValue(ResourceType type, int value)
        {
            value = Mathf.Clamp(value, 0, int.MaxValue);
            int prevValue = _userManager.CurrentUser.Resources[type];
            _userManager.CurrentUser.Resources[type] = value;

            _eventAggregator.SendEvent(new ResourceModifiedEvent
            {
                NewValue = value,
                PrevValue = prevValue,
                Type = type
            });
        }

        public bool HasResource(ResourceType type, int value)
        {
            return _userManager.CurrentUser.Resources[type] >= value;
        }

        public string GetPriceText(List<ResourcesData> data, bool ignoreFormat = false)
        {
            string result = string.Empty;
            List<string> array = new List<string>();

            foreach (var resourceData in data)
            {
                string value = ignoreFormat ? $"{resourceData.Value}" : $"{GetValueColor(resourceData.Type, resourceData.Value)}";

                switch (resourceData.Type)
                {
                    case ResourceType.Coins:
                        array.Add(
                            $"{GameConstants.Resources.Coins} {value}");
                        break;
                    case ResourceType.Gems:
                        array.Add(
                            $"{GameConstants.Resources.Gems} {value}");
                        break;
                    case ResourceType.Wood:
                        array.Add(
                            $"{GameConstants.Resources.Wood} {value}");
                        break;
                    case ResourceType.Stone:
                        array.Add(
                            $"{GameConstants.Resources.Stone} {value}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return array.Aggregate(result, (current, value) => current + value + "  ");

            string GetValueColor(ResourceType type, int value)
            {
                return HasResource(type, value)
                    ? value.ToString()
                    : string.Format(GameConstants.ColorFormat.Red, value);
            }
        }
    }
}