using Events;
using Gameplay.Locations.Models;
using UserSystem;
using Utils.Events;
using Utils.GameStageSystem;
using Zenject;

namespace Stages
{
    public class PreloaderStage : AbstractStageBase
    {
        public override StageType StageType => StageType.Preloader;

        private readonly EventAggregator _eventAggregator;
        private readonly UserManager _userManager;

        [Inject]
        public PreloaderStage(EventAggregator eventAggregator, UserManager userManager)
        {
            _eventAggregator = eventAggregator;
            _userManager = userManager;
        }

        public override void Initialize(object data)
        {
            base.Initialize(data);

            LoadLocation();
        }

        public override void DeInitialize()
        {
            base.DeInitialize();
        }

        public override void Show()
        {
        }

        public override void Hide()
        {
        }

        private void LoadLocation()
        {
            _userManager.CurrentUser.Locations.TryGetValue(_userManager.CurrentUser.CurrentLocationId, out LocationModel model);

            _eventAggregator.SendEvent(new ChangeStageEvent
            {
                Stage = StageType.Gameplay,
                Data = new LocationModel
                {
                    LocationId = model?.LocationId
                }
            });
        }
    }
}