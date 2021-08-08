using System;

namespace Defong.PopupSystem
{
    public interface IPopupManager<PopupType> where PopupType : Enum
    {
        IPopup<PopupType> GetPopupByType(PopupType type);
        void ShowPopup(PopupType type);
        void ShowPopup(PopupType type, float timer);
        void ShowPopup(PopupType type, object args);
        void ShowPopup(PopupType type, float timer, object args);
        void HidePopupByType(PopupType type);
    }
}
