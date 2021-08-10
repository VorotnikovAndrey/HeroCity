using Defong.Events;
using Gameplay;

namespace Events
{
    public class ResourceModifiedEvent : BaseEvent
    {
        public ResourceType Type;
        public int PrevValue;
        public int NewValue;
    }
}