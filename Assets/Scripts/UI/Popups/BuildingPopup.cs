using System;
using System.Linq;
using Content;
using Economies;
using Events;
using Gameplay.Building;
using Gameplay.Building.Models;
using PopupSystem;
using UI.Popups.Components;
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

        [SerializeField] private BuildingUpgradePage _upgradePage;
        [SerializeField] private LocalEvents _events;

        private BuildingsManager _buildingsManager;
        private BuildingModel _buildingModel;
        private BuildingData _buildingData;
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

            _buildingData = ContentProvider.BuildingsEconomy.Data.FirstOrDefault(x => x.Id == _buildingId);
            _buildingModel = _buildingsManager.GetBuildingModel(_buildingId);
            if (_buildingModel == null)
            {
                Debug.LogError("Model is null".AddColorTag(Color.red));
                return;
            }

            _events.EmitTitleText?.Invoke($"{_buildingModel.Id}");


            OpenUpgradePage();
        }

        private void OpenUpgradePage()
        {
            if (_buildingModel.Stage < _buildingData.Upgrades.Count)
            {
                _upgradePage.gameObject.SetActive(true);
                _upgradePage.Initialize(_buildingModel);
            }
            else
            {
                _upgradePage.gameObject.SetActive(false);
            }
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
        }
    }
}