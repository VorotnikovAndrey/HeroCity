using CameraSystem;
using Defong.Utils;
using Events;
using Gameplay.Building;
using Gameplay.Locations.View;
using InputSystem;
using PopupSystem;
using UnityEngine;
using UserSystem;
using Utils;
using Utils.Events;
using Utils.GameStageSystem;
using Utils.ObjectPool;
using Utils.PopupSystem;
using Zenject;
using CameraType = CameraSystem.CameraType;

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

        private LocationView _locationView;
        private LocationCamera _locationCamera;
        private LocationInput _locationInput;
        private BuildingsManager _buildingsManager;

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

            // Subscribe
            _eventAggregator.Add<ExitLevelButtonPressedEvent>(OnExitLevelButtonPressedEvent);

            // Location & UI
            if (!TryLoadLocation(data))
            {
                return;
            }
            _popupManager.ShowPopup(PopupType.Hud);

            // Camera
            _cameraManager.SetCameraType(CameraType.Location);
            _locationCamera = _cameraManager.ActiveCamera as LocationCamera;
            _locationInput = new LocationInput();
            _locationCamera?.Init(_locationInput, _locationView.CameraSettings);
            _timeTicker.OnTick += _locationInput.Update;

            // Buildings
            _buildingsManager = ProjectContext.Instance.Container.Resolve<BuildingsManager>();
            _buildingsManager.Initialize();
        }

        public override void DeInitialize()
        {
            base.DeInitialize();

            // Unsubscribe
            _eventAggregator.Remove<ExitLevelButtonPressedEvent>(OnExitLevelButtonPressedEvent);

            // Location & UI
            _locationView?.ReleaseItemView();
            _locationView = null;
            _popupManager.HidePopupByType(PopupType.Hud);

            // Camera
            _timeTicker.OnTick -= _locationInput.Update;
            _locationInput = null;
            _locationCamera.SwitchToDefaultState();
            _cameraManager.SetCameraType(CameraType.Lobby);

            // Buildings
            _buildingsManager.DeInitialize();
            _buildingsManager = null;
        }

        public override void Show()
        {
        }

        public override void Hide()
        {
        }

        private bool TryLoadLocation(object data)
        {
            _locationView = ViewGenerator.GetOrCreateItemView<LocationView>(string.Format(GameConstants.Base.LocationsFormat, data));
            if (_locationView != null)
            {
                Debug.Log($"Location {data.AddColorTag(Color.yellow)} loaded".AddColorTag(Color.cyan));
            }
            else
            {
                Debug.LogError($"Location {data.AddColorTag(Color.yellow)} is not found!".AddColorTag(Color.red));
                return false;
            }

            return true;
        }

        private void OnExitLevelButtonPressedEvent(ExitLevelButtonPressedEvent sender)
        {
            _eventAggregator.SendEvent(new ChangeStageEvent
            {
                Stage = StageType.Lobby,
                Data = null
            });
        }
    }
}