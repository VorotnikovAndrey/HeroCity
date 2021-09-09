using Gameplay.Building.View;
using UnityEngine;
using Utils.Events;

namespace Events
{
    public class BuildingViewSelectedEvent : BaseEvent
    {
        public BuildingView View;
        public Vector3 TapPosition;
    }
}