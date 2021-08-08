using UnityEngine;
using Utils.Events;
using Zenject;

namespace Utils.ObjectPool
{
    public abstract class AbstractBaseView : MonoBehaviour, IView
    {
        private static int _index = 0;
        public bool isActive { get; set; }
        public int Index { get; } = _index++;

        private EventAggregator _eventAggregator;

        protected EventAggregator EventAggregator
        {
            get
            {
                return _eventAggregator ??= ProjectContext.Instance.Container.Resolve<EventAggregator>();
            }
        }

        public virtual Vector3 WorldPosition => Transform.position;

        public Transform Transform
        {
            get
            {
                if (_transform == null)
                    _transform = transform;
                return _transform;
            }
        }

        private Transform _transform;

        public GameObject GameObject
        {
            get
            {
                if (_gameObject == null)
                    _gameObject = gameObject;
                return _gameObject;
            }
        }

        private GameObject _gameObject;

        
        public virtual string Name
        {
            get => gameObject.name;
            set => gameObject.name = value;
        }

        public virtual void Initialize(object data)
        {
        }

        public virtual void Deinitialize()
        {
            
        }

        public virtual void SetPosition(Vector3 target)
        {
            Transform.position = target;
        }

        public virtual void SetLocalPosition(Vector3 target)
        {
            Transform.localPosition = target;
        }

        public virtual void SetEulerAngles(Vector3 diretion)
        {
            Transform.localEulerAngles = diretion;
        }

        public virtual void SetRotation(Quaternion rotation)
        {
            Transform.rotation = rotation;
        }

        public virtual void SetPosition(Vector2 target)
        {
            Transform.position = new Vector3(target.x, 0.1f, target.y);
        }

        public void SetViewActive(bool isActive)
        {
            gameObject.SetActive(isActive);

            if (isActive)
            {
                SwitchOn();
            }
            else
            {
                SwitchOff();
            }
        }

        public virtual void SetParent(Transform parent)
        {
            Transform.SetParent(parent);
        }

        protected virtual void SwitchOn()
        {
        }

        protected virtual void SwitchOff()
        {
        }

        private void OnDestroy()
        {
            this.DestroyAndRemoveFromPool();
        }
    }
}