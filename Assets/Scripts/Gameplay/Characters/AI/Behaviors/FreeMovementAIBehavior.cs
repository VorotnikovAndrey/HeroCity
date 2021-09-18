using Gameplay.Locations.View;
using Gameplay.Movement;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Characters.AI.Behaviors
{
    public class FreeMovementAIBehavior : AbstractAIBehavior
    {
        private LocationView _locationView;
        private int _iterations;
        private int _count;

        public FreeMovementAIBehavior(int iterations = 1)
        {
            _iterations = iterations;
        }

        public override void Begin()
        {
            _locationView = ProjectContext.Instance.Container.Resolve<LocationView>();

            Model.Movement = new NavMeshMovement();
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

            Vector3 position = _locationView.WorldPosition + new Vector3(Random.Range(-25f, 25f), 0, Random.Range(-25f, 25f));
            Model.Movement.GoTo(position, value =>
            {
                if (_count < _iterations)
                {
                    GoToNext();
                }
                else
                {
                    End();
                }
            });
        }
    }
}