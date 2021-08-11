using Events;
using Gameplay.Building.View;
using PopupSystem;
using TMPro;
using UnityEngine;
using Utils.PopupSystem;

namespace UI.Popups
{
    public class BuildingPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Building;

        [SerializeField] private TextMeshProUGUI _idText = default;

        private BuildingView View;

        protected override void OnShow(object args = null)
        {
            if (args != null)
            {
                View = args as BuildingView;
            }

            _idText.text = View?.BuildingId;
        }

        protected override void OnHide()
        {
            EventAggregator.SendEvent(new BuildingViewExitEvent());
        }
    }
}