using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Building;
using Gameplay.Building.Models;
using Gameplay.Improvements;
using Polyglot;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.ObjectPool;
using Zenject;

namespace UI.Popups.Components
{
    public class BuildingUpgradePage : BuildingPopupPageElement
    {
        public const string ImprovementButtonPath = "Components/ImprovementButton";
        public const string BuildKey = "BUILD";
        public const string UpgradeKey = "UPGRADE";

        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _buyButtonText;
        [SerializeField] private Transform _improvementsHolder;
        [SerializeField] private LocalEvents _events;

        private GameResourceManager _gameResourceManager;
        private List<ImprovementButton> _improvementButtons;

        public override void Initialize(BuildingModel buildingModel)
        {
            base.Initialize(buildingModel);

            if (buildingModel.Stage >= _economyData.Upgrades.Count)
            {
                return;
            }

            _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();

            bool hasResources = _economyData.Upgrades[_buildingModel.Stage].Price.All(y => _gameResourceManager.HasResource(y.Type, y.Value));
            
            _buyButton.interactable = hasResources;
            _buyButtonText.text = _buildingModel.State.Value == BuildingState.Inactive ? Localization.Get(BuildKey) : Localization.Get(UpgradeKey);
            _backButton.gameObject.SetActive(buildingModel.Stage > 0);
            
            _events.EmitPriceText?.Invoke(_gameResourceManager.GetPriceText(_economyData.Upgrades[_buildingModel.Stage].Price));

            _improvementButtons = new List<ImprovementButton>();

            foreach (var improvementId in _economyData.Upgrades[_buildingModel.Stage].ImprovementOpen)
            {
                var improvementButton = ViewGenerator.GetOrCreateItemView<ImprovementButton>(ImprovementButtonPath);
                improvementButton.Initialize(improvementId);
                improvementButton.Transform.SetParent(_improvementsHolder);

                _improvementButtons.Add(improvementButton);
            }
        }

        public override void DeInitialize()
        {
            base.DeInitialize();

            if (_improvementButtons == null)
            {
                return;
            }

            foreach (var improvementButton in _improvementButtons)
            {
                improvementButton.ReleaseItemView();
            }

            _improvementButtons = null;
        }

        [Serializable]
        public class LocalEvents
        {
            public UnityEvent<string> EmitPriceText;
            public UnityEvent EmitBreak;
        }
    }
}