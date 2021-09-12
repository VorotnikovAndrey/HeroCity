using System.Linq;
using Content;
using Events;
using Gameplay.Building;
using Gameplay.Building.Models;
using Gameplay.Building.View;
using PopupSystem;
using TMPro;
using UnityEngine;
using Utils;
using Utils.PopupSystem;
using Zenject;

namespace UI.Popups
{
    public class BuildingPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Building;

        [SerializeField] private TextMeshProUGUI _idText = default;
        [Space] 
        [SerializeField] private GameObject _buildButton;
        [SerializeField] private GameObject _upgradeButton;

        private BuildingsManager _buildingsManager;
        private BuildingView _view;
        private BuildingModel _model;

        protected override void OnShow(object args = null)
        {
            if (args != null)
            {
                _view = args as BuildingView;
            }

            if (_view == null)
            {
                Debug.LogError("BuildingView is null".AddColorTag(Color.red));
                return;
            }

            _buildingsManager = ProjectContext.Instance.Container.Resolve<BuildingsManager>();
            _model = _buildingsManager.GetBuildingModel(_view.BuildingId);

            if (_model == null)
            {
                Debug.LogError("BuildingModel is null".AddColorTag(Color.red));
                return;
            }

            _idText.text = _model.Id;

            _buildButton.SetActive(_model.State == BuildingState.Inactive);
            _upgradeButton.SetActive(_model.State == BuildingState.Active);

            var data = ContentProvider.BuildingsEconomy.Data.FirstOrDefault(x => x.Id == _view.BuildingId);
            
            //_buildButton.GetComponentInChildren<TextMeshProUGUI>().text =
            //    $"Build {GameConstants.Resources.Coins} {data.Upgrades.FirstOrDefault(x => x.Stage == _model.Stage).Price.First().Value}";

            //_upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text =
            //    $"Upgrade {GameConstants.Resources.Coins} {data.Upgrades.FirstOrDefault(x => x.Stage == _model.Stage).Price.First().Value}";
        }

        public void OnExitPressed()
        {
            EventAggregator.SendEvent(new BuildingViewUnSelectedEvent
            {
                ReturnToPrevPos = true
            });

            Hide();
        }

        public void OnBuildPressed()
        {
            EventAggregator.SendEvent(new BuildingViewUnSelectedEvent
            {
                ReturnToPrevPos = false
            });

            EventAggregator.SendEvent(new BuildBuildingEvent
            {
                View = _view
            });

            Hide();
        }

        public void OnUpgradePressed()
        {
            EventAggregator.SendEvent(new BuildingViewUnSelectedEvent
            {
                ReturnToPrevPos = false
            });

            EventAggregator.SendEvent(new UpgradeBuildingEvent
            {
                View = _view
            });

            Hide();
        }
    }
}