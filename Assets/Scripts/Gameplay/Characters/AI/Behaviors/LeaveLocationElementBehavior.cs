using Events;
using Gameplay.Characters.AI.Behaviors;
using Gameplay.Locations.View;
using Source;
using UnityEngine;
using Utils.Pathfinding;
using Zenject;

namespace Characters.AI.Behaviors
{
    [CreateAssetMenu(fileName = "LeaveLocationElementBehavior", menuName = "Characters/AI/Behaviors/LeaveLocationElementBehavior")]
    public class LeaveLocationElementBehavior : ElementBehavior
    {
        private LocationView _locationView;

        public override void Begin()
        {
            _locationView = ProjectContext.Instance.Container.Resolve<LocationView>();
            Action();
            SendBeginning();
        }

        public override void End()
        {
            SendEnded();

            EventAggregator.SendEvent(new CharacterLeavedFromLocationEvent
            {
                Model = Model
            });
        }

        public override void Interrupt()
        {
            SendInterrupted(string.Empty);
        }

        private void Action()
        {
            var wayPoint = _locationView.WaypointsContainer.GetTypePositions(MapWaypointType.Exit).GetRandom();

            Model.Movement.GoTo(wayPoint.Position, x =>
            {
                End();
            });
        }
    }
}