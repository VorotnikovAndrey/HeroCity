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

            foreach (var view in Buildings)
            {
                var model = _buildingsManager.GetBuildingModel(view.BuildingId);

                model.State.AddListener(view.SetState);
                model.Stage.AddListener(view.SetStage);

                if (model.State.Value == BuildingState.Upgrade)
                {
                    view.UpgradeBar.Initialize(model.UpgradeStartUnixTime, model.UpgradeEndUnixTime);
                }
            }
        }

        public void DeInitialize()
        {
            foreach (var view in Buildings)
            {
                var model = _buildingsManager.GetBuildingModel(view.BuildingId);

                model.State.RemoveListener(view.SetState);
                model.Stage.RemoveListener(view.SetStage);
            }
        }

        private void OnDestroy()
        {
            ProjectContext.Instance.Container.Unbind<LocationView>();
        }
    }
}