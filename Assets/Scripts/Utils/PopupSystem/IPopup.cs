using System;

namespace Defong.PopupSystem
{
    public interface IPopup<PopupType> where PopupType : Enum
    {
        PopupType Type { get; }
        event Action<PopupType> OnClosePopup;
        event Action<PopupType> OnShowPopup;
        bool IsBackButtonAvailable { get; }
        
        int ChildIndex { get; }
        bool IsShowed { get; }
        void Show(object args = null);
        void ShowWithTimer(float time);
        void ShowWithTimer(float time, object args);
        void Hide();
        void HandleCloseButton();
    }
}
