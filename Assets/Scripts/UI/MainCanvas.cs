using System;
using UnityEngine;
using Utils.Events;
using Zenject;

namespace UI
{
    public class MainCanvas : MonoBehaviour
    {
        private EventAggregator _eventAggregator;

        private EventAggregator EventAggregator => _eventAggregator ??= ProjectContext.Instance.Container.Resolve<EventAggregator>();

        private void Awake()
        {
            ProjectContext.Instance.Container.BindInstances(this);
        }
    }
}