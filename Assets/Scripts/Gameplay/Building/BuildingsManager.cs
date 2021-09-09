using System.Collections.Generic;
using CameraSystem;
using Content;
using Economies;
using Events;
using Gameplay.Building.Models;
using PopupSystem;
using Unity.VisualScripting;
using UnityEngine;
using UserSystem;
using Utils;
using Utils.Events;
using Utils.PopupSystem;
using Zenject;

namespace Gameplay.Building
{
    public class BuildingsManager
    {
        private readonly PopupManager<PopupType> _popupManager;
        private readonly EventAggregator _eventAggregator;
        private readonly TimeTicker _timeTicker;
        private readonly CameraManager _cameraManager;
        private readonly LocationCamera _locationCamera;
        private readonly UserManager _userManager;
        private readonly BuildingsEconomy _buildingsEconomy;

        public BuildingsManager()
        {
            _popupManager = ProjectContext.Instance.Container.Resolve<PopupManager<PopupType>>();
            _eventAggregator = ProjectContext.Instance.Container.Resolve<EventAggregator>();
            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            _cameraManager = ProjectContext.Instance.Container.Resolve<CameraManager>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _locationCamera = _cameraManager.ActiveCameraView as LocationCamera;
            _buildingsEconomy = ContentProvider.BuildingsEconomy;
        }

        public void Initialize()
        {
            ProjectContext.Instance.Container.BindInstance(this);

            _eventAggregator.Add<BuildingViewSelectedEvent>(OnBuildingViewSelected);
            _eventAggregator.Add<BuildingViewUnSelectedEvent>(OnBuildingViewUnSelected);
            _eventAggregator.Add<BuildBuildingEvent>(OnBuildBuilding);
            _eventAggregator.Add<UpgradeBuildingEvent>(OnUpgradeBuilding);

            _timeTicker.OnSecondTick += OnSecondTick;
        }

        public void DeInitialize()
        {
            _eventAggregator.Remove<BuildingViewSelectedEvent>(OnBuildingViewSelected);
            _eventAggregator.Remove<BuildingViewUnSelectedEvent>(OnBuildingViewUnSelected);
            _eventAggregator.Remove<BuildBuildingEvent>(OnBuildBuilding);
            _eventAggregator.Remove<UpgradeBuildingEvent>(OnUpgradeBuilding);

            ProjectContext.Instance.Container.Unbind<BuildingsManager>();

            _timeTicker.OnSecondTick -= OnSecondTick;
        }

        public BuildingModel GetBuildingModel(string id)
        {
            _userManager.CurrentUser.Buildings.TryGetValue(id, out BuildingModel model);
            return model;
        }

        private void OnBuildingViewUnSelected(BuildingViewUnSelectedEvent sender)
        {
            _locationCamera.SwitchToDefaultState(sender.ReturnToPrevPos);
        }

        private void OnBuildingViewSelected(BuildingViewSelectedEvent sender)
        {
            if (_locationCamera.CameraState != CameraStates.Default)
            {
                return;
            }

            _locationCamera.SwitchToViewTransform(sender.View.transform, sender.View.CameraOffset != null ? sender.View.CameraOffset.Offset : Vector3.zero);
            _popupManager.ShowPopup(PopupType.Building, sender.View);
        }

        private void OnBuildBuilding(BuildBuildingEvent sender)
        {
            var model = GetBuildingModel(sender.View.BuildingId);
            model.State = BuildingState.Upgrade;

            _userManager.Save();
        }

        private void OnUpgradeBuilding(UpgradeBuildingEvent sender)
        {
        }

        private void OnSecondTick()
        {

        }
    }
}