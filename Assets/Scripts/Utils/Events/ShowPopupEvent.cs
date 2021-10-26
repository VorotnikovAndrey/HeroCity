using System;

namespace Utils.Events
{
    public class ShowPopupEvent<TPopupType> : BaseEvent where TPopupType : Enum
    {
        public TPopupType PopupType;
        public object Data;
    }
}