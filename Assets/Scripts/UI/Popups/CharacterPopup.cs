using CameraSystem;
using Events;
using PopupSystem;
using Utils.PopupSystem;
using Zenject;

namespace UI.Popups
{
    public class CharacterPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Character;

        protected override void OnHide()
        {
            EventAggregator.SendEvent(new CharacterViewUnSelectedEvent
            {
                ReturnToPrevPos = true
            });
        }
    }
}