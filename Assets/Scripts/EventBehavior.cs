using Utils.Events;
using Zenject;

public class EventBehavior
{
    private EventAggregator _eventAggregator;

    protected EventAggregator EventAggregator => _eventAggregator ??= ProjectContext.Instance.Container.Resolve<EventAggregator>();
}