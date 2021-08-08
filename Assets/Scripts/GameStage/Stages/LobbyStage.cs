using CameraSystem;
using Defong.Utils;
using Events;
using PopupSystem;
using Utils.Events;
using Utils.GameStageSystem;
using Utils.PopupSystem;
using Zenject;
using CameraType = CameraSystem.CameraType;

namespace GameStage.Stages
{
    public class LobbyStage : AbstractStageBase
    {
        public override StageType StageType => StageType.Lobby;

        private readonly PopupManager<PopupType> _popupManager;
        private readonly EventAggregator _eventAggregator;
        private readonly TimeTicker _timeTicker;
        private readonly CameraManager _cameraManager;

        [Inject]
        public LobbyStage(PopupManager<PopupType> popupManager, EventAggregator eventAggregator,
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

            _eventAggregator.Add<StartLevelButtonPressedEvent>(LoadLevel);
            _popupManager.ShowPopup(PopupType.Lobby);
            _cameraManager.SetCameraType(CameraType.Main);
        }

        public override void DeInitialize()
        {
            base.DeInitialize();

            _eventAggregator.Remove<StartLevelButtonPressedEvent>(LoadLevel);
            _popupManager.HidePopupByType(PopupType.Lobby);
        }

        public override void Show()
        {
        }

        public override void Hide()
        {
        }

        private void LoadLevel(StartLevelButtonPressedEvent sender)
        {
            _eventAggregator.SendEvent(new ChangeStageEvent
            {
                Stage = StageType.Gameplay,
                Data = null
            });
        }
    }
}