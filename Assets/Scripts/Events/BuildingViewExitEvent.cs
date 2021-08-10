using Utils.Events;

namespace Events
{
    public class BuildingViewExitEvent : BaseEvent
    {
        public bool ReturnToPrevPos = true;
    }
}