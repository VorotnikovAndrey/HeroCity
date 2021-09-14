using System;
using System.Linq;
using Content;
using Gameplay.Building;
using Gameplay.Building.Models;
using Polyglot;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserSystem;
using Utils;
using Zenject;

namespace UI.Popups.Components
{
    public class BuildingUpgradePage : MonoBehaviour
    {
        public const string BuildKey = "BUILD";
        public const string UpgradeKey = "UPGRADE";

        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _buyButtonText;
        [SerializeField] private LocalEvents _events;

        private GameResourceManager _gameResourceManager;
        private BuildingsManager _buildingsManager;
        private BuildingModel _buildingModel;

        public void Initialize(BuildingModel buildingModel)
        {
            _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();
            _buildingsManager = ProjectContext.Instance.Container.Resolve<BuildingsManager>();
            _buildingModel = buildingModel;

            var economyData = ContentProvider.BuildingsEconomy.Data.FirstOrDefault(x => x.Id == buildingModel.Id);
            if (economyData == null)
            {
                Debug.LogError("Economy data is null".AddColorTag(Color.red));
                return;
            }

            bool hasResources = economyData.Upgrades[_buildingModel.Stage].Price.All(y => _gameResourceManager.HasResource(y.Type, y.Value));
            
            _buyButton.interactable = hasResources;
            _buyButtonText.text = _buildingModel.State.Value == BuildingState.Inactive ? Localization.Get(BuildKey) : Localization.Get(UpgradeKey);

            _events.EmitPriceText?.Invoke(_gameResourceManager.GetPriceText(economyData.Upgrades[_buildingModel.Stage].Price));
        }

        [Serializable]
        public class LocalEvents
        {
            public UnityEvent<string> EmitPriceText;
        }
    }
}