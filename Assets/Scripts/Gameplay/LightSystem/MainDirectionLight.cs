using UnityEngine;
using Zenject;

namespace Gameplay.LightSystem
{
    [RequireComponent(typeof(Light))]
    public class MainDirectionLight : EventMonoBehavior
    {
        [HideInInspector] [SerializeField] private Light _light;

        [SerializeField] private Light _secondLight;

        public Light Light => _light;
        public Light SecondLight => _secondLight;

        public Transform Transform { get; private set; }

        private void OnValidate()
        {
            _light = GetComponent<Light>();
        }

        public void Awake()
        {
            Transform = GetComponent<Transform>();

            ProjectContext.Instance.Container.BindInstances(this);
        }

        public void OnDestroy()
        {
            ProjectContext.Instance.Container.Unbind<MainDirectionLight>();
        }
    }
}