using System;
using CameraSystem;
using Characters;
using Defong.Utils;
using Events;
using Gameplay.Building;
using Gameplay.Characters;
using Gameplay.Locations.Models;
using Gameplay.Locations.View;
using Gameplay.Time;
using InputSystem;
using PopupSystem;
using UnityEngine;
using UserSystem;
using Utils;
using Utils.Events;
using Utils.GameStageSystem;
using Utils.ObjectPool;
using Utils.Pathfinding;
using Utils.PopupSystem;
using Zenject;

namespace Stages
{
    public class GameplayStage : AbstractStageBase
    {
        public override StageType StageType => StageType.Gameplay;

        private readonly PopupManager<PopupType> _popupManager;
        private readonly EventAggregator _eventAggregator;
        private readonly TimeTicker _timeTicker;
        private readonly CameraManager _cameraManager;
        private readonly UserManager _userManager;

        private LocationCamera _locationCamera;
        private GameInput _gameInput;
        private BuildingsManager _buildingsManager;
        private CharactersSystem _charactersSystem;
        private WaypointSystem _waypointSystem;
        private LocationView _locationView;
        private DayTime _dayTime;

        [Inject]
        public GameplayStage(PopupManager<PopupType> popupManager, EventAggregator eventAggregator,
            TimeTicker timeTicker, CameraManager cameraManager, UserManager userManager)
        {
            _popupManager = popupManager;
            _eventAggregator = eventAggregator;
            _timeTicker = timeTicker;
            _cameraManager = cameraManager;
            _userManager = userManager;
        }

        public override void Initialize(object data)
        {
            base.Initialize(data);

            if (!(data is LocationModel locationModel))
            {
                Debug.LogError("LocationModel is null".AddColorTag(Color.red));
                return;
            }

            // Main
            _dayTime = new DayTime();
            _dayTime.Initialize();

            // Location
            if (!TryLoadLocation(locationModel))
            {
                return;
            }

            // Camera
            _cameraManager.SetCameraType(GameCameraType.Location);
            _locationCamera = _cameraManager.ActiveCameraView as LocationCamera;
            _gameInput = new GameInput();
            _locationCamera?.Init(_gameInput, _locationView.CameraSettings);
            _timeTicker.OnTick += _gameInput.Update;

            // UI
            _popupManager.ShowPopup(PopupType.Hud);

            // Gameplay
            _waypointSystem = new WaypointSystem();
            _waypointSystem.SetWaypointsContainer(_locationView.WaypointsContainer);
            _buildingsManager = new BuildingsManager();
            _buildingsManager.Initialize();
            _charactersSystem = new CharactersSystem();
            _charactersSystem.Initialize();
            _locationView.Initialize();
        }

        public override void DeInitialize()
        {
            base.DeInitialize();

            // Camera
            _timeTicker.OnTick -= _gameInput.Update;
            _gameInput = null;
            _locationCamera.SwitchToDefaultState();
            _cameraManager.SetCameraType(GameCameraType.Preloader);

            // UI
            _popupManager.HidePopupByType(PopupType.Hud);

            // Gameplay
            _locationView.DeInitialize();
            _buildingsManager.DeInitialize();
            _buildingsManager = null;
            _charactersSystem.DeInitialize();
            _charactersSystem = null;
            _waypointSystem = null;

            // Location
            _locationView.DestroyAndRemoveFromPool();
            _locationView = null;

            // Main
            _dayTime.DeInitialize();
            _dayTime = null;

            //GC.Collect(0, GCCollectionMode.Forced);
        }

        public override void Show()
        {
        }

        public override void Hide()
        {
        }

        private bool TryLoadLocation(LocationModel locationModel)
        {
            var view = ViewGenerator.GetOrCreateItemView<LocationView>(string.Format(GameConstants.Base.LocationsFormat, locationModel.LocationId));

            if (view != null)
            {
                _locationView = view;
                Debug.Log($"Location {locationModel.LocationId.AddColorTag(Color.yellow)} loaded".AddColorTag(Color.cyan));
            }
            else
            {
                Debug.LogError($"Location {locationModel.LocationId.AddColorTag(Color.yellow)} is not found!".AddColorTag(Color.red));
                return false;
            }

            return true;
        }
    }
}