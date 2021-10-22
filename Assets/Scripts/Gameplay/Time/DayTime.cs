using System;
using System.Collections.Generic;
using System.Linq;
using Content;
using DG.Tweening;
using Gameplay.LightSystem;
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

        private readonly TimeTicker _timeTicker;
        private readonly UserManager _userManager;
        private readonly DayTimeParams _dayTimeParams;

        private MainDirectionLight _directionLight;
        private List<Tweener> _tweeners = new List<Tweener>();
        private DayTimeType _currentType;

        public DayTime()
        {
            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _directionLight = ProjectContext.Instance.Container.Resolve<MainDirectionLight>();
            _dayTimeParams = ContentProvider.Graphic.DayTimeParams;
        }

        public void Initialize()
        {
            SetDayType(_dayTimeParams.TimeData[_userManager.CurrentUser.Time.Hours].Type, true);

            _timeTicker.OnTick += OnUpdate;
        }

        public void DeInitialize()
        {
            _timeTicker.OnTick -= OnUpdate;

#if UNITY_EDITOR
            ResetMaterials();
#endif
        }

        private void OnUpdate()
        {
            _userManager.CurrentUser.Time += TimeSpan.FromMinutes(UnityEngine.Time.deltaTime * _dayTimeParams.TimeFactor);

            OnValueChanged?.Invoke(_userManager.CurrentUser.Time);

            SetDayType(_dayTimeParams.TimeData[_userManager.CurrentUser.Time.Hours].Type);
        }

        private void SetDayType(DayTimeType type, bool force = false)
        {
            if (_currentType == type)
            {
                return;
            }

            var data = _dayTimeParams.Data.FirstOrDefault(x => x.Type == type);
            if (data == null)
            {
                Debug.LogError("Data is null".AddColorTag(Color.red));
                return;
            }

            _currentType = type;

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

        private void ResetMaterials()
        {
            var data = _dayTimeParams.Data.FirstOrDefault(x => x.Type == DayTimeType.Afternoon);
            if (data == null)
            {
                Debug.LogError("Data is null".AddColorTag(Color.red));
                return;
            }

            foreach (var material in _dayTimeParams.Materials)
            {
                material.color = data.MaterialColor;
            }

            _directionLight.Light.color = data.LightColor;

            foreach (var material in _dayTimeParams.GlassMaterials)
            {
                material.color = _dayTimeParams._glassColorOff;
            }

            Debug.Log("Reset Meterials".AddColorTag(Color.yellow));
        }
    }
}