using System;
using DG.Tweening;
using Gameplay;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI.Popups.Components
{
    public class ResourceContainer : MonoBehaviour
    {
        public ResourceType ResourceType;
        [Space] 
        [SerializeField] private AbstractNumberToStringConverterScriptableObject _converter;
        [Space]
        [SerializeField] private float _animationDuration = 1f;
        [SerializeField] private Ease _animationEase = Ease.Unset;
        [Space]
        [SerializeField] private Image _icon = default;
        [SerializeField] private TextMeshProUGUI _text = default;

        private Tweener _tweener;
        private int _value;

        public virtual int Value
        {
            get => _value;
            private set
            {
                _value = value;
                _text.text = _converter != null ? _converter.Convert(_value) : _value.ToString();
            }
        }

        private void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public void SetValue(int value, bool force = false)
        {
            if (force)
            {
                _tweener?.Kill();
                Value = value;
            }
            else
            {
                _tweener?.Kill();
                _tweener = DOTween.To(() => Value, x => Value = x, value, _animationDuration).SetEase(_animationEase).OnKill(() => _tweener = null);
            }
        }

        private void OnDestroy()
        {
            _tweener?.Kill();
        }
    }
}