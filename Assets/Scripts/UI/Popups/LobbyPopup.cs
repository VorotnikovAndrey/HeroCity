using Events;
using PopupSystem;
using Utils.PopupSystem;

namespace UI.Popups
{
    public class LobbyPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Lobby;

        public void OnStartLevelButtonPressed()
        {
            EventAggregator.SendEvent(new StartLevelButtonPressedEvent());
            Hide();
        }

        public void OnExitLobbyButtonPressed()
        {
            EventAggregator.SendEvent(new ExitLobbyButtonPressedEvent());
            Hide();
        }
    }
}