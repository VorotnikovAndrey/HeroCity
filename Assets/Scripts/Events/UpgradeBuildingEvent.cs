using Gameplay.Building.View;
using Utils.Events;

namespace Events
{
    public class UpgradeBuildingEvent : BaseEvent
    {
        public BuildingView View;
    }
}