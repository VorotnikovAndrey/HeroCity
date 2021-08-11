using CameraSystem;
using Events;
using PopupSystem;
using Utils.Events;
using Utils.PopupSystem;
using Zenject;

namespace Gameplay.Building
{
    public class BuildingsManager
    {
        private readonly PopupManager<PopupType> _popupManager;
        private readonly EventAggregator _eventAggregator;
        private readonly CameraManager _cameraManager;
        private readonly LocationCamera _locationCamera;

        [Inject]
        public BuildingsManager(PopupManager<PopupType> popupManager, EventAggregator eventAggregator, CameraManager cameraManager)
        {
            _popupManager = popupManager;
            _eventAggregator = eventAggregator;
            _cameraManager = cameraManager;

            _locationCamera = _cameraManager.ActiveCamera as LocationCamera;
        }

        public void Initialize()
        {
            _eventAggregator.Add<BuildingViewEnterEvent>(OnBuildingViewEnterEvent);
            _eventAggregator.Add<BuildingViewExitEvent>(OnBuildingViewExitEvent);
        }

        public void DeInitialize()
        {
            _eventAggregator.Remove<BuildingViewEnterEvent>(OnBuildingViewEnterEvent);
            _eventAggregator.Remove<BuildingViewExitEvent>(OnBuildingViewExitEvent);
        }

        private void OnBuildingViewExitEvent(BuildingViewExitEvent sender)
        {
            _locationCamera.SwitchToDefaultState(sender.ReturnToPrevPos);
        }

        private void OnBuildingViewEnterEvent(BuildingViewEnterEvent sender)
        {
            if (_locationCamera.CameraState != CameraStates.Default)
            {
                return;
            }

            _locationCamera.SwitchToViewTransform(sender.View.transform);
            _popupManager.ShowPopup(PopupType.Building, sender.View);
        }
    }
}