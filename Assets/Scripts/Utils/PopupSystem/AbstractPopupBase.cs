using System;
using UnityEngine;
using Utils.Events;
using Zenject;

namespace Utils.PopupSystem
{
    public abstract class AbstractPopupBase<PopupType> : MonoBehaviour, IPopup<PopupType> where PopupType : Enum
    {
        [SerializeField] protected GameObject _body;

        public bool IsShowed => _body.activeSelf;

        public abstract PopupType Type { get; }

        public event Action<PopupType> OnShowPopup;
        public event Action<PopupType> OnClosePopup;

        [Inject]
        protected EventAggregator EventAggregator;

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
            transform.localPosition = Vector3.zero;
        }

        public void Show(object args = null)
        {
            OnShow(args);
            _body.SetActive(true);
            OnShowLate();

            OnShowPopup?.Invoke(Type);
        }

        public void Hide()
        {
            OnHide();
            _body.SetActive(false);

            OnClosePopup?.Invoke(Type);
        }

        protected virtual void OnShow(object args = null) { }
        protected virtual void OnShowLate() { }
        protected virtual void OnHide() { }
        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
    }
}
