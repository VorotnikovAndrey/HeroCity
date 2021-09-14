using System;
using Economies;
using PopupSystem;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using Utils.PopupSystem;

namespace UI.Popups
{
    public class ImprovementPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Improvement;

        [SerializeField] private LocalEvents _events;

        private ImprovementData _improvementData;

        protected override void OnShow(object args = null)
        {
            _improvementData = args as ImprovementData;

            if (_improvementData == null)
            {
                Debug.LogError("ImprovementData is null".AddColorTag(Color.red));
                return;
            }

            _events.EmitTitle?.Invoke(_improvementData.Id);
            _events.EmitDescription?.Invoke(_improvementData.Description);
        }
    }

    [Serializable]
    public class LocalEvents
    {
        public UnityEvent<string> EmitTitle;
        public UnityEvent<string> EmitDescription;
    }
}