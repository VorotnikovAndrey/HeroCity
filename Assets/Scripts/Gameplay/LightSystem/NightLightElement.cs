using System.Linq;
using Content;
using DG.Tweening;
using Gameplay.Time;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.LightSystem
{
    public class NightLightElement : MonoBehaviour
    {
        [SerializeField] private Light _light;
        [SerializeField] private float _value;

        private DayTime _dayTime;
        private Tweener _tweener;

        private void Awake()
        {
            _dayTime = ProjectContext.Instance.Container.Resolve<DayTime>();
            _dayTime.OnDayTimeTypeChanged += DayTypeChanged;

            DayTypeChanged(_dayTime.CurrentType, true);
        }

        private void DayTypeChanged(DayTimeType type, bool force)
        {
            var data = ContentProvider.Graphic.DayTimeParams.Data.FirstOrDefault(x => x.Type == type);

            if (data == null)
            {
                Debug.LogError("Data is null".AddColorTag(Color.red));
                return;
            }

            var duration = force ? 0f : ContentProvider.Graphic.DayTimeParams._glassDuration;

            if (type == DayTimeType.Night)
            {
                _light.intensity = 0f;
                _light.enabled = true;

                _tweener?.Kill();
                _tweener = DOTween.To(() => _light.intensity, x => _light.intensity = x, _value, duration).SetEase(ContentProvider.Graphic.DayTimeParams._glassEase).OnKill(() => _tweener = null);
            }
            else if (type == DayTimeType.Morning && _light.intensity > 0)
            {
                _light.intensity = _value;

                _tweener?.Kill();
                _tweener = DOTween.To(() => _light.intensity, x => _light.intensity = x, 0f, duration).SetEase(ContentProvider.Graphic.DayTimeParams._glassEase).OnKill(() =>
                {
                    _light.enabled = false;
                    _tweener = null;
                });
            }
            else
            {
                _light.enabled = false;
            }
        }

        private void OnDestroy()
        {
            _dayTime.OnDayTimeTypeChanged -= DayTypeChanged;
        }
    }
}