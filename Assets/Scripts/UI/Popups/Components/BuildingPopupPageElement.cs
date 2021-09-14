using System.Linq;
using Content;
using Economies;
using Gameplay.Building;
using Gameplay.Building.Models;
using UnityEngine;
using Zenject;

namespace UI.Popups.Components
{
    public class BuildingPopupPageElement : MonoBehaviour
    {
        public int Index;
        public GameObject Object;

        protected BuildingsManager _buildingsManager;
        protected BuildingModel _buildingModel;
        protected BuildingData _economyData;

        protected virtual void OnValidate()
        {
            if (Object == null)
            {
                Object = gameObject;
            }
        }

        public virtual void Initialize(BuildingModel buildingModel)
        {
            _buildingsManager = ProjectContext.Instance.Container.Resolve<BuildingsManager>();
            _buildingModel = buildingModel;
            _economyData = ContentProvider.Economies.BuildingsEconomy.Data.FirstOrDefault(x => x.Id == buildingModel.Id);
        }

        public virtual void DeInitialize()
        {
            _buildingModel = null;
        }
    }
}