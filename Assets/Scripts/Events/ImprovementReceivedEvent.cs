using System.Collections.Generic;
using Gameplay;
using ResourceSystem;
using Utils.Events;

namespace Events
{
    public class ImprovementReceivedEvent : BaseEvent
    {
        public List<string> Improvements = new List<string>();
    }
}