using UnityEngine;
using Utils.Events;
using Zenject;

public class EventScriptableObjectBehavior : ScriptableObject
{
    private EventAggregator _eventAggregator;

    protected EventAggregator EventAggregator => _eventAggregator ??= ProjectContext.Instance.Container.Resolve<EventAggregator>();
}