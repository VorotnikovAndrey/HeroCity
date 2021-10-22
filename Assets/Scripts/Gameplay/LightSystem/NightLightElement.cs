using System.Linq;
using Content;
using DG.Tweening;
using Gameplay.Time;
using UnityEngine;
using Utils;

namespace Gameplay.LightSystem
{
    public class NightLightElement : MonoBehaviour
    {
        [SerializeField] private Light _light;
        [SerializeField] private float _value;

        private Tweener _tweener;

        private void Awake()
        {
            DayTime.OnDayTimeTypeChanged += DayTypeChanged;
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
                _light.enabled = true;
                _tweener?.Kill();
                _tweener = DOTween.To(() => _light.intensity, x => _light.intensity = x, _value, duration).SetEase(ContentProvider.Graphic.DayTimeParams._glassEase).OnKill(() => _tweener = null);
            }
            else if (type == DayTimeType.Morning && _light.intensity > 0)
            {
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
            DayTime.OnDayTimeTypeChanged -= DayTypeChanged;
        }
    }
}