using PopupSystem;
using UnityEngine;
using UnityEngine.UI;
using Utils.PopupSystem;

namespace UI.Popups
{
    public class DefaultPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Default;

        [SerializeField] private Image _icon = default;

        protected override void OnShow(object args = null)
        {
            base.OnShow(args);
        }
    }
}