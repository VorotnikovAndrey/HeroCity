using UnityEngine;
using Utils.Events;
using Zenject;

public class EventMonoBehavior : MonoBehaviour
{
    private EventAggregator _eventAggregator;

    protected EventAggregator EventAggregator => _eventAggregator ??= ProjectContext.Instance.Container.Resolve<EventAggregator>();
}