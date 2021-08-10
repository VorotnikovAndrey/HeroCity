using Gameplay.Building.View;
using UnityEngine;
using Utils.Events;

namespace Events
{
    public class BuildingViewEnterEvent : BaseEvent
    {
        public BuildingView View;
        public Vector3 TapPosition;
    }
}