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

        public BuildingsManager()
        {
            _popupManager = ProjectContext.Instance.Container.Resolve<PopupManager<PopupType>>();
            _eventAggregator = ProjectContext.Instance.Container.Resolve<EventAggregator>();
            _cameraManager = ProjectContext.Instance.Container.Resolve<CameraManager>();
            _locationCamera = _cameraManager.ActiveCamera as LocationCamera;
        }

        public void Initialize()
        {
            _eventAggregator.Add<BuildingViewSelectedEvent>(OnBuildingViewSelected);
            _eventAggregator.Add<BuildingViewUnSelectedEvent>(OnBuildingViewUnSelected);
        }

        public void DeInitialize()
        {
            _eventAggregator.Remove<BuildingViewSelectedEvent>(OnBuildingViewSelected);
            _eventAggregator.Remove<BuildingViewUnSelectedEvent>(OnBuildingViewUnSelected);
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

            _locationCamera.SwitchToViewTransform(sender.View.transform);
            _popupManager.ShowPopup(PopupType.Building, sender.View);
        }
    }
}