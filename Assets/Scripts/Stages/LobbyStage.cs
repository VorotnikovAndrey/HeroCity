using Events;
using Gameplay.Locations.Models;
using UnityEngine;
using UserSystem;
using Utils.Events;
using Utils.GameStageSystem;
using Zenject;

namespace Stages
{
    public class LobbyStage : AbstractStageBase
    {
        public override StageType StageType => StageType.Lobby;

        private readonly EventAggregator _eventAggregator;
        private readonly UserManager _userManager;

        [Inject]
        public LobbyStage(EventAggregator eventAggregator, UserManager userManager)
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
            _userManager.CurrentUser.Locations.TryGetValue(_userManager.CurrentUser.LocationId, out LocationModel model);

            _eventAggregator.SendEvent(new ChangeStageEvent
            {
                Stage = StageType.Gameplay,
                Data = new LocationModel
                {
                    LocationId = model.LocationId
                }
            });
        }
    }
}