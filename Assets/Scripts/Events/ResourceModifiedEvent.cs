﻿using Gameplay;
using ResourceSystem;
using Utils.Events;

namespace Events
{
    public class ResourceModifiedEvent : BaseEvent
    {
        public ResourceType Type;
        public int PrevValue;
        public int NewValue;
    }
}