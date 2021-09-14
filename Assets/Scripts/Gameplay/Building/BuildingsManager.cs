using System;
using System.Collections.Generic;
using System.Linq;
using CameraSystem;
using Content;
using Economies;
using Events;
using Gameplay.Building.Models;
using Gameplay.Building.View;
using Gameplay.Locations.Models;
using Gameplay.Locations.View;
using PopupSystem;
using ResourceSystem;
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
        private readonly GameResourceManager _gameResourceManager;
        private readonly BuildingsEconomy _buildingsEconomy;
        private readonly UserManager _userManager;

        private LocationModel _locationModel;
        private LocationView _locationView;

        public BuildingsManager()
        {
            _popupManager = ProjectContext.Instance.Container.Resolve<PopupManager<PopupType>>();
            _eventAggregator = ProjectContext.Instance.Container.Resolve<EventAggregator>();
            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            _cameraManager = ProjectContext.Instance.Container.Resolve<CameraManager>();
            _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _locationCamera = _cameraManager.ActiveCameraView as LocationCamera;
            _buildingsEconomy = ContentProvider.BuildingsEconomy;
        }

        public void Initialize()
        {
            ProjectContext.Instance.Container.BindInstance(this);

            _locationModel = _userManager.CurrentUser.CurrentLocation;
            _locationView = ProjectContext.Instance.Container.Resolve<LocationView>();

            _eventAggregator.Add<BuildingViewSelectedEvent>(OnBuildingViewSelected);
            _eventAggregator.Add<BuildingViewUnSelectedEvent>(OnBuildingViewUnSelected);
            _eventAggregator.Add<UpgradeBuildingEvent>(OnUpgradeBuilding);

            UpdateUpgrades();

            _timeTicker.OnSecondTick += OnSecondUpdate;
        }

        public void DeInitialize()
        {
            _eventAggregator.Remove<BuildingViewSelectedEvent>(OnBuildingViewSelected);
            _eventAggregator.Remove<BuildingViewUnSelectedEvent>(OnBuildingViewUnSelected);
            _eventAggregator.Remove<UpgradeBuildingEvent>(OnUpgradeBuilding);

            ProjectContext.Instance.Container.Unbind<BuildingsManager>();

            _timeTicker.OnSecondTick -= OnSecondUpdate;
        }

        private void OnSecondUpdate()
        {
            UpdateUpgrades();
        }

        public BuildingModel GetBuildingModel(string id)
        {
            _locationModel.Buildings.TryGetValue(id, out BuildingModel model);
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

            _locationCamera.SwitchToViewTransform(sender.View.transform, sender.View.ActiveStageElement.CameraOffset != null ?
                sender.View.ActiveStageElement.CameraOffset.Offset : Vector3.zero);

            _popupManager.ShowPopup(PopupType.Building, sender.View.BuildingId);
        }

        private void OnUpgradeBuilding(UpgradeBuildingEvent sender)
        {
            TryUpgradeBuilding(sender.BuildingId);
        }

        private void TryUpgradeBuilding(string buildingId)
        {
            var model = GetBuildingModel(buildingId);
            var view = _locationView.Buildings.FirstOrDefault(x => x.BuildingId == buildingId);
            var economy = _buildingsEconomy.Data.FirstOrDefault(x => x.Id == buildingId);

            if (model == null || view == null)
            {
                Debug.LogError($"Model or view {buildingId.AddColorTag(Color.yellow)} is null".AddColorTag(Color.red));
                return;
            }

            if (economy == null)
            {
                Debug.LogError($"Economy {buildingId.AddColorTag(Color.yellow)} is not found".AddColorTag(Color.red));
                return;
            }

            if (model.Stage.Value >= economy.Upgrades.Count)
            {
                Debug.LogError($"{buildingId.AddColorTag(Color.yellow)} stage is max".AddColorTag(Color.red));
                return;
            }

            var upgradeInfo = economy.Upgrades[model.Stage];

            foreach (var price in upgradeInfo.Price)
            {
                if (_gameResourceManager.HasResource(price.Type, price.Value))
                {
                    continue;
                }

                Debug.Log($"{price.Type.AddColorTag(Color.yellow)} not enough".AddColorTag(Color.red));
                return;
            }

            foreach (var price in upgradeInfo.Price)
            {
                _gameResourceManager.AddResourceValue(price.Type, -price.Value);
            }

            var currentTime = DateTimeUtils.GetCurrentTime();
            model.UpgradeEndUnixTime = currentTime + upgradeInfo.Duration;
            model.State.Value = BuildingState.Upgrade;
            model.Stage.ForceEvent();
            view.UpgradeBar.Initialize(model.UpgradeEndUnixTime);

            _userManager.Save();
        }

        private void UpdateUpgrades()
        {
            var currentTime = DateTimeUtils.GetCurrentTime();

            foreach (var model in _locationModel.Buildings.Values)
            {
                if (model.State.Value != BuildingState.Upgrade)
                {
                    continue;
                }

                if (currentTime < model.UpgradeEndUnixTime)
                {
                    continue;
                }

                model.State.Value = BuildingState.Active;
                model.Stage.Value++;

                Debug.Log($"{model.Id.AddColorTag(Color.yellow)} upgraded to stage {model.Stage.Value.AddColorTag(Color.yellow)}".AddColorTag(Color.red));

                _userManager.Save();
            }
        }
    }
}