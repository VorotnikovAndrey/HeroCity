using System;
using Defong.Events;

namespace Utils.Events
{
    /// <summary>
    /// Used in the <see cref="EventAggregator"/> to open a popup using <see cref="PopupManager{PopupType}"/>
    /// </summary>
    public class PopupClosedEvent<TPopupType> : BaseEvent where TPopupType : Enum
    {
        /// <summary>
        /// Popup type that has been closed
        /// </summary>
        public TPopupType PopupType;

        public PopupClosedEvent(TPopupType type)
        {
            PopupType = type;
        }
    }
}