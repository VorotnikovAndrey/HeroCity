using System;

namespace Utils.Events
{
    /// <summary>
    /// Used in the <see cref="EventAggregator"/> to open a popup using <see cref="PopupManager"/>
    /// </summary>
    public class ShowPopupEvent<TPopupType> : BaseEvent where TPopupType : Enum
    {
        /// <summary>
        /// Popup type to open
        /// </summary>
        public TPopupType PopupType;
        
        /// <summary>
        /// Data to populate Popup with.
        /// If Data is null Show() method will be called, otherwise Show(obj args) will be called/
        /// </summary>
        public object Data;
    }
}