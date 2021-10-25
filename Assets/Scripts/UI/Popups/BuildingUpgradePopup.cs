using System;
using System.Collections.Generic;
using System.Linq;
using Content;
using Economies;
using Events;
using Gameplay.Building;
using Gameplay.Building.Models;
using Gameplay.Improvements;
using Polyglot;
using PopupSystem;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserSystem;
using Utils;
using Utils.ObjectPool;
using Utils.PopupSystem;
using Zenject;

namespace UI.Popups
{
    public class BuildingUpgradePopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.BuildingUpgrade;

        public const string ImprovementButtonPath = "Popups/Components/ImprovementButton";
        public const string DependencyButtonPath = "Popups/Components/DependencyButton";
        public const string BuildKey = "BUILD";
        public const string UpgradeKey = "UPGRADE";

        [SerializeField] private ButtonInteractableTextColorHelper _buyButton;
        [SerializeField] private TextMeshProUGUI _buyButtonText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _improvementsHolder;
        [SerializeField] private Transform _dependenciesHolder;
        [SerializeField] private GameObject _dependencyBar;
        [SerializeField] private RectTransform[] _layoutGroups;
        [Space]
        [SerializeField] private LocalEvents _events;

        private BuildingsManager _buildingsManager;
        private BuildingModel _buildingModel;
        private BuildingData _economyData;
        private GameResourceManager _gameResourceManager;
        private UserManager _userManager;

        private string _buildingId;

        private List<ImprovementButton> _improvementButtons = new List<ImprovementButton>();
        private List<ImprovementButton> _dependencyButtons = new List<ImprovementButton>();

        protected override void OnAwake()
        {
            _buildingsManager = ProjectContext.Instance.Container.Resolve<BuildingsManager>();
            _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
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

            _economyData = ContentProvider.Economies.BuildingsEconomy.Data.FirstOrDefault(x => x.Id == _buildingId);

            if (_economyData == null)
            {
                Debug.LogError("EconomyData is null".AddColorTag(Color.red));
                return;
            }

            UpdateState();
            CreateImprovements();
            CreateDependencies();

            EventAggregator.Add<ResourceModifiedEvent>(OnResourceModified);
            EventAggregator.Add<ImprovementReceivedEvent>(OnImprovementReceived);

            _events.EmitTitleText?.Invoke($"{_buildingModel.Id}");
        }

        protected override void OnShowLate()
        {
            RebuildLayouts();
        }

        protected override void OnHide()
        {
            ReleaseAllImprovementButtons();
            ReleaseAllDependencyButtons();

            EventAggregator.Remove<ResourceModifiedEvent>(OnResourceModified);
            EventAggregator.Remove<ImprovementReceivedEvent>(OnImprovementReceived);

            EventAggregator.SendEvent(new BuildingViewUnSelectedEvent
            {
                ReturnToPrevPos = true
            });
        }

        private void OnImprovementReceived(ImprovementReceivedEvent sender)
        {
            UpdateState();
        }

        private void OnResourceModified(ResourceModifiedEvent sender)
        {
            UpdateState();
        }

        public void OnBuyPressed()
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

        private void UpdateState()
        {
            var data = _economyData.Upgrades[_buildingModel.Stage];

            bool hasResources = data.Price.All(y => _gameResourceManager.HasResource(y.Type, y.Value));
            bool hasImprovements = data.ImprovementDependencies.All(dependency => _userManager.CurrentUser.Improvement.Contains(dependency));

            _buyButton.SetState(hasResources);
            _buyButtonText.text = _buildingModel.State.Value == BuildingState.Inactive ? Localization.Get(BuildKey) : Localization.Get(UpgradeKey);
            _priceText.text = _gameResourceManager.GetPriceText(_economyData.Upgrades[_buildingModel.Stage].Price);

            if (hasImprovements)
            {
                _buyButton.gameObject.SetActive(true);
                _dependencyBar.SetActive(false);
            }
            else
            {
                _dependencyBar.SetActive(true);
                _buyButton.gameObject.SetActive(false);
            }

            RebuildLayouts();
        }

        private void CreateImprovements()
        {
            ReleaseAllImprovementButtons();

            foreach (var improvementId in _economyData.Upgrades[_buildingModel.Stage].ImprovementOpen)
            {
                var improvementButton = ViewGenerator.GetOrCreateItemView<ImprovementButton>(ImprovementButtonPath);
                improvementButton.Initialize(improvementId);
                improvementButton.SetParent(_improvementsHolder);

                _improvementButtons.Add(improvementButton);
            }
        }

        private void CreateDependencies()
        {
            ReleaseAllDependencyButtons();

            foreach (var improvementId in _economyData.Upgrades[_buildingModel.Stage].ImprovementDependencies)
            {
                var improvementButton = ViewGenerator.GetOrCreateItemView<ImprovementButton>(DependencyButtonPath);
                improvementButton.Initialize(improvementId);
                improvementButton.SetParent(_dependenciesHolder);

                _dependencyButtons.Add(improvementButton);
            }
        }

        private void ReleaseAllImprovementButtons()
        {
            foreach (var improvementButton in _improvementButtons)
            {
                improvementButton.ReleaseItemView();
            }

            _improvementButtons.Clear();
        }

        private void ReleaseAllDependencyButtons()
        {
            foreach (var dependencyButton in _dependencyButtons)
            {
                dependencyButton.ReleaseItemView();
            }

            _dependencyButtons.Clear();
        }

        private void RebuildLayouts()
        {
            foreach (var element in _layoutGroups)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(element);
            }
        }

        [Serializable]
        public class LocalEvents
        {
            public UnityEvent<string> EmitTitleText;
        }
    }
}