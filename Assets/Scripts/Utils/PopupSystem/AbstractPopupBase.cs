using System;
using System.Collections;
using System.Collections.Generic;
using Defong.PopupSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Events;
using Zenject;

namespace Utils.PopupSystem
{
    /// <summary>
    /// Used in the <see cref="AbstractPopupBase{PopupType}"/> and <see cref="PopupManager{PopupType}"/> to differentiate popups
    /// Base class for popups
    /// </summary>
    public abstract class AbstractPopupBase<PopupType> : MonoBehaviour, IPopup<PopupType> where PopupType : Enum
    {
        [SerializeField] protected GameObject _panel;
        [SerializeField] protected Button _darkBackground;

        public int ChildIndex => transform.GetSiblingIndex();
        public bool IsShowed => _panel.activeSelf;

        public abstract PopupType Type { get; }
        public event Action<PopupType> OnClosePopup;
        public event Action<PopupType> OnShowPopup;
        
        protected virtual bool IsTopMost { get; }
        public virtual bool IsBackButtonAvailable { get; }
        
        private readonly Dictionary<Button, UnityAction> _buttonClickCallbacks = new Dictionary<Button, UnityAction>();
        
        [Inject]
        protected EventAggregator EventAggregator;
        
        private void Awake()
        {
            OnAwake();
            DisableView();
        }

        private void Start()
        {
            OnStart();
            transform.localPosition = Vector3.zero;
        }

        public void Show(object args = null)
        {
            MapButtonClickCallbacks();
            Subscribe();
            OnShow(args);
            EnablePanel();
            OnShowLate();
            OnShowPopup?.Invoke(Type);
        }
        
        private void EnablePanel()
        {
            if (IsTopMost)
                _panel.transform.parent.SetAsLastSibling();

            _panel.SetActive(true);
            if (_darkBackground != null)
            {
                _darkBackground.gameObject.SetActive(true);
            }
        }

        public void ShowWithTimer(float time)
        {
            Show();
            StartCoroutine(HideAfterTime(time));
        }

        public void ShowWithTimer(float time, object args)
        {
            Show(args);
            StartCoroutine(HideAfterTime(time));
        }

        public void Hide()
        {
            Unsubscribe();
            OnHide();

            DisableView();

            OnClosePopup?.Invoke(Type);
        }

        private void DisableView()
        {
            _panel.SetActive(false);
            if (_darkBackground != null)
            {
                _darkBackground.gameObject.SetActive(false);
            }
        }

        protected IEnumerator HideAfterTime(float timer)
        {
            yield return new WaitForSeconds(timer);
            Hide();
        }

        protected virtual void OnShow(object args = null) { }
        protected virtual void OnShowLate() { }
        protected virtual void OnHide() { }
        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void MapButtonClickCallbacks() { }

        public virtual void HandleCloseButton()
        {
            Hide();
        }

        /// <summary>
        /// Helper method to map Button.onclick event to its callback.
        /// </summary>
        /// <param name="button">Button</param>
        /// <param name="callback">on click callback</param>
        protected void MapButton(Button button, UnityAction callback)
        {
            if (_buttonClickCallbacks.ContainsKey(button))
            {
                Debug.LogError("You try twice subscribe on button");
            }
            else
            {
                _buttonClickCallbacks.Add(button, callback);
            }
        }

        /// <summary>
        /// Method to subscribe mapped buttons to their callbacks. Subscribe to other events here 
        /// </summary>
        protected virtual void Subscribe()
        {
            foreach (var kvp in _buttonClickCallbacks)
            {
                kvp.Key.onClick.AddListener(kvp.Value);
            }
        }

        /// <summary>
        /// Method to unsubscribe mapped buttons from their callbacks. Unsubscribe from other events here 
        /// </summary>
        protected virtual void Unsubscribe()
        {
            foreach (var kvp in _buttonClickCallbacks)
            {
                kvp.Key.onClick.RemoveListener(kvp.Value);
            }
            _buttonClickCallbacks.Clear();
        }
    }
}
