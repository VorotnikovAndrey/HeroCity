using Asyncoroutine;
using Gameplay.Locations.View;
using Source;
using UnityEngine;
using Utils.Pathfinding;
using Zenject;

namespace Characters.AI.Behaviors
{
    [CreateAssetMenu(fileName = "WanderingElementBehavior", menuName = "Characters/AI/Behaviors/WanderingElementBehavior")]
    public class WanderingElementBehavior : ElementBehavior
    {
        public int WanderingAmount = 3;
        public float RespiteTime = 3f;
        public bool LockPosition = false;

        private MapWaypoint _nextMapWaypoint;
        private int _wanderingStepCount = 0;
        private LocationView _locationView;

        public override void Begin()
        {
            _locationView = ProjectContext.Instance.Container.Resolve<LocationView>();
            _wanderingStepCount = 0;

            GoToNext();

            SendBeginning();
        }

        public override void End()
        {
            SendEnded();
        }

        public override void Interrupt()
        {
            if (_nextMapWaypoint != null)
            {
                _nextMapWaypoint.Locked = false;
            }

            SendInterrupted(string.Empty);
        }

        private void GoToNext()
        {
            if (_wanderingStepCount >= WanderingAmount)
            {
                End();
                return;
            }

            if (_nextMapWaypoint != null)
            {
                _nextMapWaypoint.Locked = false;
            }

            _nextMapWaypoint = _locationView.WaypointsContainer.GetTypePositions(MapWaypointType.Undefined).GetRandom();
            if (LockPosition)
            {
                _nextMapWaypoint.Locked = true;
            }

            Model.Movement.GoTo(_nextMapWaypoint.Position, x =>
            {
                Respite();
            });

            _wanderingStepCount++;
        }

        private async void Respite()
        {
            await new WaitForSeconds(RespiteTime);

            GoToNext();
        }
    }
}