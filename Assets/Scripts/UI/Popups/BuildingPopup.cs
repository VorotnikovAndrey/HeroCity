using Events;
using PopupSystem;
using Utils.PopupSystem;

namespace UI.Popups
{
    public class BuildingPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Building;

        protected override void OnHide()
        {
            EventAggregator.SendEvent(new BuildingViewExitEvent());
        }
    }
}