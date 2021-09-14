using System;
using System.Collections.Generic;
using Content;
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

        [SerializeField] private List<BuildingPopupPageElement> _pages;
        [SerializeField] private LocalEvents _events;

        private BuildingsManager _buildingsManager;
        private BuildingModel _buildingModel;
        private string _buildingId;

        protected override void OnAwake()
        {
            _buildingsManager = ProjectContext.Instance.Container.Resolve<BuildingsManager>();
        }

        protected override void OnShow(object args = null)
        {
            _buildingId = args?.ToString();

            _buildingModel = _buildingsManager.GetBuildingModel(_buildingId);
            if (_buildingModel == null)
            {
                Debug.LogError("Model is null".AddColorTag(Color.red));
                return;
            }

            _events.EmitTitleText?.Invoke($"{_buildingModel.Id}");

            foreach (var page in _pages)
            {
                page.Initialize(_buildingModel);
            }

            SwitchPage(_buildingModel.State.Value == BuildingState.Inactive ? 1 : 0);
        }

        protected override void OnHide()
        {
            foreach (var page in _pages)
            {
                page.DeInitialize();
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

        private void SwitchPage(int index)
        {
            foreach (BuildingPopupPageElement page in _pages)
            {
                if (Check(index))
                {
                    page.Object.SetActive(page.Index == index);
                }
            }
        }

        private bool Check(int index)
        {
            switch (index)
            {
                case 1: return _buildingModel.Stage < ContentProvider.Economies.BuildingsEconomy.Get(_buildingId).Upgrades.Count;
            }

            return true;
        }

        [Serializable]
        public class LocalEvents
        {
            public UnityEvent<string> EmitTitleText;
        }
    }
}