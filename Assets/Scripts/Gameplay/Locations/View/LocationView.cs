using System.Collections.Generic;
using CameraSystem;
using Gameplay.Building;
using Gameplay.Building.View;
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
        [Space]
        public List<BuildingView> Buildings;

        private BuildingsManager _buildingsManager;

        private void Awake()
        {
            ProjectContext.Instance.Container.BindInstances(this);
        }

        public void Initialize()
        {
            _buildingsManager = ProjectContext.Instance.Container.Resolve<BuildingsManager>();

            foreach (var building in Buildings)
            {
                var model = _buildingsManager.GetBuildingModel(building.BuildingId);

                model.State.AddListener(building.SetState);
                model.Stage.AddListener(building.SetStage);
            }
        }

        public void DeInitialize()
        {
            foreach (var building in Buildings)
            {
                var model = _buildingsManager.GetBuildingModel(building.BuildingId);

                model.State.RemoveListener(building.SetState);
                model.Stage.RemoveListener(building.SetStage);
            }
        }

        private void OnDestroy()
        {
            ProjectContext.Instance.Container.Unbind<LocationView>();
        }
    }
}