using CameraSystem;
using UnityEngine;
using Utils.ObjectPool;
using Utils.Pathfinding;
using Zenject;

namespace Gameplay.Locations.View
{
    public class LocationView : AbstractBaseView
    {
        [HideInInspector] public string LocationId;

        public CameraSettings CameraSettings;
        public WaypointsContainer WaypointsContainer;

        private void Awake()
        {
            ProjectContext.Instance.Container.BindInstances(this);
        }

        private void OnDestroy()
        {
            ProjectContext.Instance.Container.Unbind<LocationView>();
        }
    }
}