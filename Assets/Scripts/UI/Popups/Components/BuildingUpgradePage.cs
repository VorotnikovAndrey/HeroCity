using System.Collections.Generic;
using System.Linq;
using Events;
using Gameplay.Building;
using Gameplay.Building.Models;
using Gameplay.Improvements;
using Polyglot;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserSystem;
using Utils.ObjectPool;
using Zenject;

namespace UI.Popups.Components
{
    public class BuildingUpgradePage : BuildingPopupPageElement
    {
        public const string ImprovementButtonPath = "Popups/Components/ImprovementButton";
        public const string DependencyButtonPath = "Popups/Components/DependencyButton";
        public const string BuildKey = "BUILD";
        public const string UpgradeKey = "UPGRADE";

        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _buyButtonText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _improvementsHolder;
        [SerializeField] private Transform _dependenciesHolder;
        [SerializeField] private GameObject _dependencyBar;

        private GameResourceManager _gameResourceManager;
        private UserManager _userManager;

        private List<ImprovementButton> _improvementButtons = new List<ImprovementButton>();
        private List<ImprovementButton> _dependencyButtons = new List<ImprovementButton>();

        private void Awake()
        {
            _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
        }

        public override void Initialize(BuildingModel buildingModel)
        {
            base.Initialize(buildingModel);

            if (buildingModel.Stage >= _economyData.Upgrades.Count)
            {
                return;
            }

            UpdateState();

            CreateImprovements();
            CreateDependencies();

            _backButton.gameObject.SetActive(buildingModel.Stage > 0);

            EventAggregator.Add<ResourceModifiedEvent>(OnResourceModified);
            EventAggregator.Add<ImprovementReceivedEvent>(OnImprovementReceived);
        }

        private void OnImprovementReceived(ImprovementReceivedEvent sender)
        {
            UpdateState();
        }

        private void OnResourceModified(ResourceModifiedEvent sender)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            var data = _economyData.Upgrades[_buildingModel.Stage];

            bool hasResources = data.Price.All(y => _gameResourceManager.HasResource(y.Type, y.Value));
            bool hasImprovements = data.ImprovementDependencies.All(dependency => _userManager.CurrentUser.Improvement.Contains(dependency));

            _buyButton.interactable = hasResources;
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
        }

        private void CreateImprovements()
        {
            ReleaseAllImprovementButtons();

            foreach (var improvementId in _economyData.Upgrades[_buildingModel.Stage].ImprovementOpen)
            {
                var improvementButton = ViewGenerator.GetOrCreateItemView<ImprovementButton>(ImprovementButtonPath);
                improvementButton.Initialize(improvementId);
                improvementButton.Transform.SetParent(_improvementsHolder);

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
                improvementButton.Transform.SetParent(_dependenciesHolder);

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

        public override void DeInitialize()
        {
            base.DeInitialize();

            ReleaseAllImprovementButtons();
            ReleaseAllDependencyButtons();

            EventAggregator.Remove<ResourceModifiedEvent>(OnResourceModified);
            EventAggregator.Remove<ImprovementReceivedEvent>(OnImprovementReceived);
        }
    }
}