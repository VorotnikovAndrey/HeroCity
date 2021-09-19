using Utils.Events;

namespace Events
{
    public class CharacterViewUnSelectedEvent : BaseEvent
    {
        public bool ReturnToPrevPos = true;
    }
}