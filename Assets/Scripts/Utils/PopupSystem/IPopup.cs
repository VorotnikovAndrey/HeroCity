using System;

namespace Utils.PopupSystem
{
    public interface IPopup<out PopupType> where PopupType : Enum
    {
        PopupType Type { get; }

        event Action<PopupType> OnClosePopup;
        event Action<PopupType> OnShowPopup;

        bool IsShowed { get; }
        void Show(object args = null);
        void Hide();
    }
}
