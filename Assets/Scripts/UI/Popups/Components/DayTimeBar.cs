using System;
using Gameplay.Time;
using TMPro;
using UnityEngine;

namespace UI.Popups.Components
{
    public class DayTimeBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            DayTime.OnValueChanged += OnUpdate;
        }

        private void OnUpdate(TimeSpan value)
        {
            _text.text = value.ToString(@"hh\:mm");
        }

        private void OnDestroy()
        {
            DayTime.OnValueChanged -= OnUpdate;
        }
    }
}
