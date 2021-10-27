using System.Collections.Generic;
using System.Linq;
using Content;
using Economies;
using Events;
using Gameplay.Building.Models;
using Gameplay.Equipments;
using ResourceSystem;
using UnityEngine;
using UserSystem;
using Utils;
using Zenject;

namespace Gameplay.Craft
{
    public class ProductionManager : EventBehavior
    {
        private UserManager _userManager;
        private GameResourceManager _gameResourceManager;
        private TimeTicker _timeTicker;
        private Dictionary<string, ProductionBuildingModel> _productionBuildingModels;

        public void Initialize()
        {
            ProjectContext.Instance.Container.BindInstance(this);

            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();
            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();

            _productionBuildingModels = new Dictionary<string, ProductionBuildingModel>();
            foreach (var model in _userManager.CurrentUser.CurrentLocation.Buildings.Values)
            {
                if (model is ProductionBuildingModel productionBuildingModel)
                {
                    _productionBuildingModels.Add(productionBuildingModel.Id, productionBuildingModel);

                    foreach (var element in productionBuildingModel.ProductionData)
                    {
                        Debug.LogError(element.ProductionId.AddColorTag(Color.red));
                    }
                }
            }

            EventAggregator.Add<BeginProductionEvent>(OnBeginProductionEvent);
            EventAggregator.Add<EndProductionEvent>(OnEndProductionEvent);
            EventAggregator.Add<ClaimProductionEvent>(OnClaimProductionEvent);

            _timeTicker.OnSecondTick += UpdateProduction;
        }

        public void DeInitialize()
        {
            ProjectContext.Instance.Container.Unbind<ProductionManager>();

            _productionBuildingModels.Clear();
            _userManager = null;

            EventAggregator.Remove<BeginProductionEvent>(OnBeginProductionEvent);
            EventAggregator.Remove<EndProductionEvent>(OnEndProductionEvent);
            EventAggregator.Remove<ClaimProductionEvent>(OnClaimProductionEvent);

            _timeTicker.OnSecondTick -= UpdateProduction;
        }

        private void UpdateProduction()
        {
            var currentTime = DateTimeUtils.GetCurrentTime();

            foreach (ProductionBuildingModel model in _productionBuildingModels.Values)
            {
                foreach (ProductionData data in model.ProductionData)
                {
                    data.TimeLeft.Value = data.ProductionEndUnixTime - currentTime;

                    if (data.ProductionEndUnixTime > currentTime || data.Finished)
                    {
                        continue;
                    }

                    data.Finished = true;

                    EventAggregator.SendEvent(new EndProductionEvent
                    {
                        Data = data
                    });
                }
            }
        }

        private void OnBeginProductionEvent(BeginProductionEvent sender)
        {
            if (sender.Data == null)
            {
                Debug.LogError("Data is null".AddColorTag(Color.red));
                return;
            }

            _productionBuildingModels[sender.Data.ProductionBuildingId].ProductionData.Add(sender.Data);

            Debug.Log($"Production begined {sender.Data.ProductionId.AddColorTag(Color.yellow)}".AddColorTag(Color.cyan));

            _gameResourceManager.RemoveResourceValue(GetPrice(sender.Data));
        }

        private void OnEndProductionEvent(EndProductionEvent sender)
        {
            if (sender.Data == null)
            {
                Debug.LogError("Data is null".AddColorTag(Color.red));
                return;
            }

            Debug.Log($"Production ended {sender.Data.ProductionId.AddColorTag(Color.yellow)}".AddColorTag(Color.cyan));
        }

        private void OnClaimProductionEvent(ClaimProductionEvent sender)
        {
            if (sender.Data == null)
            {
                Debug.LogError("Data is null".AddColorTag(Color.red));
                return;
            }

            _productionBuildingModels[sender.Data.ProductionBuildingId].ProductionData.Remove(sender.Data);

            Debug.Log($"Production claimed {sender.Data.ProductionId.AddColorTag(Color.yellow)}".AddColorTag(Color.cyan));

            _userManager.Save();
        }

        public ProductionType GetProductionType(ProductionItem item)
        {
            if (item is WeaponItem)
            {
                return ProductionType.Weapon;
            }

            return ProductionType.None;
        }

        private List<ResourcesData> GetPrice(ProductionData data)
        {
            Item item = null;

            switch (data.ProductionType)
            {
                case ProductionType.Weapon:
                    item = ContentProvider.Economies.WeaponShopEconomy.Data.FirstOrDefault(x => x.Item.Id == data.ProductionId)?.Item;
                    break;
                case ProductionType.Armor:
                    // TODO: Add armor economy
                    break;
            }

            return item?.Price;
        }
    }
}