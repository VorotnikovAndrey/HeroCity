using System;
using System.Collections.Generic;
using System.Linq;
using Content;
using DG.Tweening;
using Gameplay.LightSystem;
using UI;
using UnityEngine;
using UserSystem;
using Utils;
using Zenject;

namespace Gameplay.Time
{
    public class DayTime
    {
        public static event Action<TimeSpan> OnValueChanged;
        public static event Action<DayTimeType, bool> OnDayTimeTypeChanged;

        public static TimeSpan Time { get; private set; }

        private readonly TimeTicker _timeTicker;
        private readonly UserManager _userManager;
        private readonly DayTimeParams _dayTimeParams;

        private MainDirectionLight _directionLight;
        private float _timeFactor = 1;
        private List<Tweener> _tweeners = new List<Tweener>();

        private int _test = 1;

        public DayTime()
        {
            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _directionLight = ProjectContext.Instance.Container.Resolve<MainDirectionLight>();
            _dayTimeParams = ContentProvider.Graphic.DayTimeParams;

            Time = _userManager.CurrentUser.Time;
        }

        public void Initialize()
        {
            _timeTicker.OnTick += OnUpdate;

            SetDayType(DayTimeType.Afternoon, true);
        }

        public void DeInitialize()
        {
            _timeTicker.OnTick -= OnUpdate;
        }

        private void OnUpdate()
        {
            Time += TimeSpan.FromMinutes(UnityEngine.Time.deltaTime * _timeFactor);
            OnValueChanged?.Invoke(Time);

            if (Input.GetKeyDown(KeyCode.A))
            {
                _test--;

                if (_test < 0)
                {
                    _test = 3;
                }

                SetDayType((DayTimeType)_test);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _test++;

                if (_test > 3)
                {
                    _test = 0;
                }

                SetDayType((DayTimeType)_test);
            }
        }

        private void SetDayType(DayTimeType type, bool force = false)
        {
            var data = _dayTimeParams.Data.FirstOrDefault(x => x.Type == type);
            if (data == null)
            {
                Debug.LogError("Data is null".AddColorTag(Color.red));
                return;
            }

            var duration = force ? 0f : data.SwitchDuratation;

            _tweeners.ForEach(x => x?.Kill());
            _tweeners.Clear();

            foreach (var material in _dayTimeParams.Materials)
            {
                _tweeners.Add(material.DOColor(data.MaterialColor, duration).SetEase(data.SwitchEase));
            }

            _tweeners.Add(_directionLight.Transform.DORotate(data.LightRotation, duration).SetEase(data.SwitchEase));
            _tweeners.Add(_directionLight.Light.DOColor(data.LightColor, duration).SetEase(data.SwitchEase));

            foreach (var material in _dayTimeParams.GlassMaterials)
            {
                _tweeners.Add(material.DOColor(type == DayTimeType.Night ? _dayTimeParams._glassColorOn : _dayTimeParams._glassColorOff, duration).SetEase(data.SwitchEase));
            }

            OnDayTimeTypeChanged?.Invoke(type, force);
        }
    }
}