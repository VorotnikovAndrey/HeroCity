using CameraSystem;
using Defong.Utils;
using Events;
using Gameplay.Building;
using Gameplay.Locations.View;
using PopupSystem;
using Zenject;
using InputSystem;
using Utils.Events;
using Utils.GameStageSystem;
using Utils.ObjectPool;
using Utils.PopupSystem;
using CameraType = CameraSystem.CameraType;

namespace GameStage.Stages
{
    public class GameplayStage : AbstractStageBase
    {
        public override StageType StageType => StageType.Gameplay;

        private readonly PopupManager<PopupType> _popupManager;
        private readonly EventAggregator _eventAggregator;
        private readonly TimeTicker _timeTicker;
        private readonly CameraManager _cameraManager;

        private LocationView _locationView;
        private LevelCamera _levelCamera;
        private BuildingSpawner _buildingSpawner;

        [Inject]
        public GameplayStage(PopupManager<PopupType> popupManager, EventAggregator eventAggregator,
            TimeTicker timeTicker, CameraManager cameraManager)
        {
            _popupManager = popupManager;
            _eventAggregator = eventAggregator;
            _timeTicker = timeTicker;
            _cameraManager = cameraManager;
        }

        public override void Initialize(object data)
        {
            base.Initialize(data);

            _eventAggregator.Add<ExitLevelButtonPressedEvent>(OnExitLevelButtonPressedEvent);

            _cameraManager.SetCameraType(CameraType.Level);

            _levelCamera = ProjectContext.Instance.Container.Resolve<LevelCamera>();
            _levelCamera.Init(new LevelInput());

            _locationView = ViewGenerator.GetOrCreateItemView<LocationView>(string.Format(GameConstants.Base.LocationsFormat, 1));
            _buildingSpawner = new BuildingSpawner();

            _popupManager.ShowPopup(PopupType.Hud);
        }

        private void OnExitLevelButtonPressedEvent(ExitLevelButtonPressedEvent sender)
        {
            _eventAggregator.SendEvent(new ChangeStageEvent
            {
                Stage = StageType.Lobby,
                Data = null
            });
        }

        public override void DeInitialize()
        {
            base.DeInitialize();

            _eventAggregator.Remove<ExitLevelButtonPressedEvent>(OnExitLevelButtonPressedEvent);

            _locationView?.ReleaseItemView();
            _locationView = null;

            _cameraManager.SetCameraType(CameraType.Main);

            _popupManager.HidePopupByType(PopupType.Hud);
        }

        public override void Show()
        {
        }

        public override void Hide()
        {
        }
    }
}