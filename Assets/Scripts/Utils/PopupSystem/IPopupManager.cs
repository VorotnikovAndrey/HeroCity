using System;

namespace Utils.PopupSystem
{
    public interface IPopupManager<PopupType> where PopupType : Enum
    {
        IPopup<PopupType> GetPopupByType(PopupType type);
        void ShowPopup(PopupType type);
        void ShowPopup(PopupType type, object args);
        void HidePopupByType(PopupType type);
    }
}
