using Gameplay.Movement;
using UnityEngine;

namespace Gameplay.Characters
{
    public class CharacterAnimatorController : MonoBehaviour
    {
        public Animator Animator { get; private set; }

        private static readonly int Move = Animator.StringToHash("Move");

        private IMovable _movable;

        public void SetAnimator(Animator animator)
        {
            Animator = animator;
        }

        public void ConnectToMovement(IMovable movable)
        {
            _movable = movable;
            _movable?.IsMoving.AddListener(MovementListener);
        }

        public void DisconnectFromMovement()
        {
            Animator?.SetBool(Move, false);
            _movable?.IsMoving.RemoveListener(MovementListener);
            _movable = null;
        }

        private void MovementListener(bool isMove)
        {
            Animator?.SetBool(Move, isMove);
        }
    }
}