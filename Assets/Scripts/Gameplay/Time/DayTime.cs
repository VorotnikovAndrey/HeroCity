using System;
using UserSystem;
using Utils;
using Zenject;

namespace Gameplay.Time
{
    public class DayTime
    {
        public static event Action<TimeSpan> OnValueChanged;

        public static TimeSpan Time { get; private set; }

        private readonly TimeTicker _timeTicker;
        private readonly UserManager _userManager;

        public DayTime()
        {
            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();

            Time = _userManager.CurrentUser.Time;
        }

        public void Initialize()
        {
            _timeTicker.OnSecondTick += OnUpdate;
        }

        public void DeInitialize()
        {
            _timeTicker.OnSecondTick -= OnUpdate;
        }

        private void OnUpdate()
        {
            Time += TimeSpan.FromMinutes(1);
            OnValueChanged?.Invoke(Time);
        }
    }
}