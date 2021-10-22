using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Gameplay;
using ResourceSystem;
using UnityEngine;
using UnityEngine.UI;
using UserSystem;
using Zenject;

namespace UI.Popups.Components
{
    public class ResourcesBar : EventMonoBehavior
    {
        public List<ResourcesPair> ResourcesList = new List<ResourcesPair>();

        [SerializeField] private RectTransform _layoutGroup;

        private void OnValidate()
        {
            if (ResourcesList.Count != 0)
            {
                return;
            }

            foreach (ResourceContainer element in transform.GetComponentsInChildren<ResourceContainer>())
            {
                if (ResourcesList.Any(x => x.Type == element.ResourceType))
                {
                    continue;
                }

                ResourcesList.Add(new ResourcesPair
                {
                    Container = element,
                    Type = element.ResourceType
                });
            }
        }

        private void Start()
        {
            EventAggregator.Add<ResourceModifiedEvent>(OnUpdateResource);

            foreach (ResourcesPair pair in ResourcesList)
            {
                pair.Container.SetValue(ProjectContext.Instance.Container.Resolve<UserManager>().CurrentUser.Resources[pair.Type], true);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup);
        }

        private void OnUpdateResource(ResourceModifiedEvent sender)
        {
            UpdateResource(sender.Type, sender.NewValue, sender.PrevValue);
        }

        private void UpdateResource(ResourceType type, int newValue, int prevValue = 0)
        {
            ResourcesPair element = ResourcesList.FirstOrDefault(x => x.Type == type);
            element?.Container.SetValue(newValue);
        }

        private void OnDestroy()
        {
            EventAggregator.Remove<ResourceModifiedEvent>(OnUpdateResource);
        }
    }

    [Serializable]
    public class ResourcesPair
    {
        public ResourceType Type;
        public ResourceContainer Container;
    }
}