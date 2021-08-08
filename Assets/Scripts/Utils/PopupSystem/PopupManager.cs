using System;
using System.Collections.Generic;
using System.Linq;
using Defong.PopupSystem;
using Gameplay.Level;
using UI;
using UnityEngine;
using Utils.Events;
using Zenject;
using Object = UnityEngine.Object;

namespace Utils.PopupSystem
{
    public class PopupManager<PopupType> : IPopupManager<PopupType> where PopupType : Enum
    {
        private readonly Dictionary<PopupType, IPopup<PopupType>> _popups = new Dictionary<PopupType, IPopup<PopupType>>();
        private readonly List<IPopup<PopupType>> _popupHistory = new List<IPopup<PopupType>>();
        private MainCanvas _canvas;

        private readonly EventAggregator _eventAggregator;

        public PopupManager()
        {
            _eventAggregator = ProjectContext.Instance.Container.Resolve<EventAggregator>();
            _eventAggregator.Add<ShowPopupEvent<PopupType>>(OnShowPopupEvent);
        }

        ~PopupManager()
        {
            _eventAggregator.Remove<ShowPopupEvent<PopupType>>(OnShowPopupEvent);
            _canvas.OnBackButtonPress -= OnBackButtonPress;
            foreach (var popup in _popups)
            {
                popup.Value.OnClosePopup -= RemovePopupFromHistory;
                popup.Value.OnShowPopup -= AddPopupToHistory;
            }
        }

        public void SetMainCanvas(MainCanvas canvas)
        {
            _canvas = canvas;
            _canvas.OnBackButtonPress += OnBackButtonPress;
        }

        private void OnShowPopupEvent(ShowPopupEvent<PopupType> data)
        {
            if (data.Time == 0)
            {
                if (data.Data == null)
                    ShowPopup(data.PopupType);
                else
                    ShowPopup(data.PopupType, data.Data);  
            }
            else
            {
                if (data.Data == null)
                    ShowPopup(data.PopupType, data.Time);
                else
                    ShowPopup(data.PopupType, data.Time, data.Data);
            }
            
        }

        public T GetPopupByType<T>(PopupType type)
            where T : class, IPopup<PopupType>
        {
            return GetPopupByType(type) as T;
        }

        public IPopup<PopupType> GetPopupByType(PopupType type)
        {
            LoadPopup(type);
            return _popups[type];
        }

        private void LoadPopup(PopupType type)
        {
            if (!_popups.ContainsKey(type))
            {
                var popup = Resources.Load<AbstractPopupBase<PopupType>>(string.Format(GameConstants.Base.PopupFormat, type.ToString()));
                if (popup != null)
                {
                    var instance = Object.Instantiate(popup, _canvas.transform);
                    ProjectContext.Instance.Container.Inject(instance);
                    _popups.Add(type, instance);
                    instance.OnClosePopup += RemovePopupFromHistory;
                    instance.OnShowPopup += AddPopupToHistory;
                }
            }
        }

        public void ShowPopup(PopupType type)
        {
            LoadPopup(type);
            if (_popups.ContainsKey(type))
            {
                _popups[type].Show();

                if (!_popupHistory.Contains(_popups[type]))
                {
                    _popupHistory.Add(_popups[type]);
                }
            }
        }

        public void ShowPopup(PopupType type, float timer)
        {
            LoadPopup(type);
            if (_popups.ContainsKey(type))
            {
                _popups[type].ShowWithTimer(timer);
            }
        }

        private void AddPopupToHistory(PopupType type)
        {
            if (!_popupHistory.Contains(_popups[type]))
            {
                _popupHistory.Add(_popups[type]);
            }
        }
     
        private void RemovePopupFromHistory(PopupType type)
        {
            if (_popupHistory.Contains(_popups[type]))
            {
                _popupHistory.Remove(_popups[type]);
                
                _eventAggregator.SendEvent(new PopupClosedEvent<PopupType>(type));
            }
        }

        public void ShowPopup(PopupType type, object args)
        {
            LoadPopup(type);
            if (_popups.ContainsKey(type))
            {
                _popups[type].Show(args);
            }
        }

        public void ShowPopup(PopupType type, float timer, object args)
        {
            LoadPopup(type);
            if (_popups.ContainsKey(type))
            {
                _popups[type].ShowWithTimer(timer, args);
            }
        }

        public void HidePopupByType(PopupType type)
        {
            if (_popups.ContainsKey(type))
            {
                _popups[type].Hide();
            }
        }

        public void CloseLastOpenedPopup()
        {
            var lastShown = _popupHistory.LastOrDefault();
            lastShown?.HandleCloseButton();
        }

        public bool IsPopupShowed(PopupType popupType)
        {
            return _popups.ContainsKey(popupType) && _popups[popupType].IsShowed;
        }

        public bool IsAnyPopupShowed()
        {
            return _popups.Any(x => x.Value.IsShowed);
        }

        private void OnBackButtonPress()
        {
            if (_popupHistory.Count > 0)
            {
                var lastPopup = _popupHistory[_popupHistory.Count - 1];
                if (lastPopup.IsBackButtonAvailable)
                {
                    lastPopup.HandleCloseButton();
                }
            }
        }
    }
}