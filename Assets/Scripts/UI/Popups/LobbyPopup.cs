using Events;
using PopupSystem;
using Utils.PopupSystem;

namespace UI.Popups
{
    public class LobbyPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Lobby;

        public void LoadLocation(string location)
        {
            EventAggregator.SendEvent(new LoadLocationEvent
            {
                Location = location
            });

            Hide();
        }

        public void ExitGame()
        {
            EventAggregator.SendEvent(new ExitGameEvent());
            Hide();
        }
    }
}