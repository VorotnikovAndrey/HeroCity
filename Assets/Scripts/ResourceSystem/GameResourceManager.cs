using System.Collections.Generic;
using Economies;
using Events;
using UnityEditor.iOS;
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
            SetResourceValue(type, _userManager.CurrentUser.Resources[type] + value);
        }

        public void RemoveResourceValue(ResourceType type, int value)
        {
            SetResourceValue(type, _userManager.CurrentUser.Resources[type] - value);
        }

        public void AddResourceValue(List<ResourcesData> data)
        {
            if (data == null)
            {
                return;
            }

            foreach (var element in data)
            {
                SetResourceValue(element.Type, _userManager.CurrentUser.Resources[element.Type] + element.Value);
            }
        }

        public void RemoveResourceValue(List<ResourcesData> data)
        {
            if (data == null)
            {
                return;
            }

            foreach (var element in data)
            {
                SetResourceValue(element.Type, _userManager.CurrentUser.Resources[element.Type] - element.Value);
            }
        }

        public void SetResourceValue(ResourceType type, int value)
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
    }
}