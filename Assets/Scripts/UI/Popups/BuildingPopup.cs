using System;
using Events;
using Gameplay.Building;
using PopupSystem;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using Utils.PopupSystem;
using Zenject;

namespace UI.Popups
{
    public class BuildingPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Building;

        [SerializeField] private LocalEvents _events;

        private BuildingsManager _buildingsManager;
        private string _buildingId;

        protected override void OnAwake()
        {
            _buildingsManager = ProjectContext.Instance.Container.Resolve<BuildingsManager>();
        }

        protected override void OnShow(object args = null)
        {
            if (args != null)
            {
                _buildingId = args.ToString();
            }

            var model = _buildingsManager.GetBuildingModel(_buildingId);
            if (model == null)
            {
                Debug.LogError("Model is null".AddColorTag(Color.red));
                return;
            }

            _events.EmitTitleText?.Invoke($"{model.Id} {model.Stage.Value}");
            _events.EmitPriceText?.Invoke(_buildingsManager.GetUpgradePriceText(_buildingId, model.Stage.Value));
        }

        public void OnUpgradePressed()
        {
            EventAggregator.SendEvent(new BuildingViewUnSelectedEvent
            {
                ReturnToPrevPos = false
            });

            EventAggregator.SendEvent(new UpgradeBuildingEvent
            {
                BuildingId = _buildingId
            });

            Hide();
        }

        public void OnHidePressed()
        {
            EventAggregator.SendEvent(new BuildingViewUnSelectedEvent
            {
                ReturnToPrevPos = true
            });

            Hide();
        }

        [Serializable]
        public class LocalEvents
        {
            public UnityEvent<string> EmitTitleText;
            public UnityEvent<string> EmitPriceText;

        }
    }
}