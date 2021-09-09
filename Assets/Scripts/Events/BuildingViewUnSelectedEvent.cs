using Utils.Events;

namespace Events
{
    public class BuildingViewUnSelectedEvent : BaseEvent
    {
        public bool ReturnToPrevPos = true;
    }
}