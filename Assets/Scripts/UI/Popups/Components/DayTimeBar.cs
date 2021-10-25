using System;
using Gameplay.Time;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Popups.Components
{
    public class DayTimeBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private DayTime _dayTime;

        private void Start()
        {
            _dayTime = ProjectContext.Instance.Container.Resolve<DayTime>();
            _dayTime.OnValueChanged += OnUpdate;
        }

        private void OnUpdate(TimeSpan value)
        {
            _text.text = value.ToString(@"hh\:mm");
        }

        private void OnDestroy()
        {
            _dayTime.OnValueChanged -= OnUpdate;
        }
    }
}
