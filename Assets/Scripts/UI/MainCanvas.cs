using System;
using UnityEngine;
using Utils.Events;
using Zenject;

namespace UI
{
    public class MainCanvas : MonoBehaviour
    {
        public event Action OnBackButtonPress = default;

        private EventAggregator _eventAggregator;

        private EventAggregator EventAggregator => _eventAggregator ??= ProjectContext.Instance.Container.Resolve<EventAggregator>();

        private void Awake()
        {
            ProjectContext.Instance.Container.BindInstances(this);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackButtonPress?.Invoke();
            }
        }
    }
}