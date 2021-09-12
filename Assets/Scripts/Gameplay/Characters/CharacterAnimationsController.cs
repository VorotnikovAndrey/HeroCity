using UnityEngine;

namespace Characters
{
    public class CharacterAnimationsController
    {
        private static readonly int Move = Animator.StringToHash("Move");

        private void Awake()
        {
            //_view.Movement.IsMoving.AddListener(UpdateMovement);
        }

        private void UpdateMovement(bool value)
        {
            //_animator.SetBool(Move, value);
        }

        private void OnDestroy()
        {
            //_view.Movement.IsMoving.RemoveListener(UpdateMovement);
        }
    }
}