using System.Linq;
using CameraSystem;
using Content;
using Economies;
using Events;
using Gameplay.Building.Models;
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
            _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _locationCamera = ProjectContext.Instance.Container.Resolve<CameraManager>().ActiveCameraView as LocationCamera;
            _buildingsEconomy = ContentProvider.Economies.BuildingsEconomy;
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
            var model = GetBuildingModel(sender.View.BuildingId);
            if (model == null)
            {
                Debug.LogError("Model is null".AddColorTag(Color.red));
                return;
            }

            if (model.Stage.Value == 0 && model.State.Value == BuildingState.Inactive)
            {
                _popupManager.ShowPopup(PopupType.BuildingUpgrade, sender.View.BuildingId);
            }
            else if (sender.View.ShowablePopup != PopupType.None)
            {
                _popupManager.ShowPopup(sender.View.ShowablePopup, sender.View.BuildingId);
            }
            else
            {
                return;
            }

            if (_locationCamera.CameraState != CameraStates.Default)
            {
                return;
            }

            _locationCamera.SwitchToViewTransform(
                sender.View.transform,
                sender.View.ActiveStageElement.CameraOffset != null ?
                sender.View.ActiveStageElement.CameraOffset.Offset :
                Vector3.zero);
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

            model.UpgradeStartUnixTime = currentTime;
            model.UpgradeEndUnixTime = currentTime + upgradeInfo.Duration;
            model.State.Value = BuildingState.Upgrade;
            model.Stage.ForceEvent();

            view.UpgradeBar.Initialize(model.UpgradeStartUnixTime, model.UpgradeEndUnixTime);

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

                UpgradeCompleted(model);
            }
        }

        private void UpgradeCompleted(BuildingModel model)
        {
            var economy = _buildingsEconomy.Get(model.Id);
            var upgrade = economy.Upgrades[model.Stage];
            var improvements = upgrade.ImprovementOpen;

            foreach (var improvement in improvements)
            {
                if (_userManager.CurrentUser.Improvement.Contains(improvement))
                {
                    continue;
                }

                _userManager.CurrentUser.Improvement.Add(improvement);

                _eventAggregator.SendEvent(new ImprovementReceivedEvent
                {
                    Improvements = improvements
                });

                Debug.Log($"Improvement {improvement.AddColorTag(Color.yellow)} unlocked".AddColorTag(Color.cyan));
            }

            model.State.Value = BuildingState.Active;
            model.Stage.Value++;

            _userManager.Save();

            Debug.Log($"{model.Id.AddColorTag(Color.yellow)} upgraded to stage {model.Stage.Value.AddColorTag(Color.yellow)}".AddColorTag(Color.cyan));
        }
    }
}