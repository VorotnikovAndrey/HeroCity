using Asyncoroutine;
using Gameplay.Locations.View;
using Gameplay.Movement;
using Source;
using UnityEngine;
using Utils.Pathfinding;
using Zenject;

namespace Gameplay.Characters.AI.Behaviors
{
    public class WanderingAIBehavior : AbstractAIBehavior
    {
        private LocationView _locationView;
        private MapWaypoint _point;
        private int _iterations;
        private int _count;

        public WanderingAIBehavior(int iterations = 1)
        {
            _iterations = iterations;
        }

        public override void Begin()
        {
            _locationView = ProjectContext.Instance.Container.Resolve<LocationView>();

            Model.Movement = new WaypointMovement();
            Model.Movement.Initialize(Model.View, Model.View.WorldPosition, Model.Stats.MovementSpeed);

            GoToNext();

            SendBeginning();
        }

        protected override void End()
        {
            Model.Movement.Stop();

            SendEnded();
            DeInitialize();
        }

        public override void Interrupt()
        {
            Model.Movement.Stop();

            SendInterrupted(GameConstants.Reason.DefaultInterruptedReason);
            DeInitialize();
        }

        private void GoToNext()
        {
            if (Model == null)
            {
                return;
            }

            _count++;

            _point = _locationView.WaypointsContainer.GetTypePositions(MapWaypointType.Undefined, true).GetRandom();
            Model.Movement.GoTo(_point.Position, value =>
            {
                if (_count < _iterations)
                {
                    Wait();
                }
                else
                {
                    End();
                }
            });
        }

        private async void Wait()
        {
            await new WaitForSeconds(Random.Range(1f, 4f));

            GoToNext();
        }
    }
}