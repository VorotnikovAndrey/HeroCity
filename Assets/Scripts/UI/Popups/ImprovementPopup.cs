using Economies;
using PopupSystem;
using TMPro;
using UnityEngine;
using Utils;
using Utils.PopupSystem;

namespace UI.Popups
{
    public class ImprovementPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Improvement;

        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;

        private ImprovementData _improvementData;

        protected override void OnShow(object args = null)
        {
            _improvementData = args as ImprovementData;

            if (_improvementData == null)
            {
                Debug.LogError("ImprovementData is null".AddColorTag(Color.red));
                return;
            }

            _title.text = _improvementData.Id;
            _description.text = _improvementData.Description;
        }
    }
}