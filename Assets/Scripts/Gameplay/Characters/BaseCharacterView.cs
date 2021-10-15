using Gameplay.Characters.Models;
using UnityEngine;
using Utils.ObjectPool;

namespace Gameplay.Characters
{
    public class BaseCharacterView : AbstractBaseView
    {
        [SerializeField] protected CharacterAnimatorController _animatorController;
        [SerializeField] protected Transform _graphicHolder;
        [SerializeField] protected Vector3 _graphicOffset;

        protected GameObject _graphic;

        public BaseCharacterModel Model { get; private set; }

        public CharacterAnimatorController AnimatorController => _animatorController;

        public override void Initialize(object data)
        {
            Model = data as BaseCharacterModel;
        }

        public override void Deinitialize()
        {
            Model = null;

            if (_graphic != null)
            {
                Destroy(_graphic);
            }

            base.Deinitialize();
        }

        public void SetGraphic(GameObject graphic)
        {
            if (_graphic != null)
            {
                Destroy(_graphic);
            }

            _graphic = Instantiate(graphic, _graphicHolder);
            _graphic.transform.localPosition = _graphicOffset;

            AnimatorController.SetAnimator(_graphic.GetComponent<Animator>());
        }
    }
}