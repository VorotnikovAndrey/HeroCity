using Events;
using PopupSystem;
using Utils.PopupSystem;

namespace UI.Popups
{
    public class HudPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Hud;

        public void OnExitLevelButtonPressed()
        {
            EventAggregator.SendEvent(new ExitLevelButtonPressedEvent());
            Hide();
        }
    }
}