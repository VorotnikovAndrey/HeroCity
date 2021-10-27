using System;
using System.Collections.Generic;
using System.Linq;
using Content;
using Economies;
using Events;
using Gameplay.Craft;
using Gameplay.Equipments;
using PopupSystem;
using ResourceSystem;
using TMPro;
using UI.Popups.Components;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.ObjectPool;
using Utils.PopupSystem;
using Zenject;

namespace UI.Popups
{
    public class ItemCraftPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.ItemCraft;

        [SerializeField] private TextMeshProUGUI _itemType;
        [SerializeField] private RectTransform _inventoryItemHolder = default;
        [SerializeField] private Vector2 _inventoryItemScale;
        [SerializeField] private Color _defaultItemNameColor;
        [SerializeField] private Button _buttonCraft;
        [SerializeField] private RectTransform[] _layoutGroups;
        [SerializeField] private ItemCraftAffixesBar _affixesBar;
        [SerializeField] private RectTransform _statsHolder;
        [SerializeField] private RectTransform _requiredResourcesHolder;
        [Space]
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _createTimerText;

        private List<StatsInfoElement> _statsInfoElementContainers = new List<StatsInfoElement>();
        private List<ResourceRequiredContainer> _resourceRequiredContainers = new List<ResourceRequiredContainer>();
        private GameResourceManager _gameResourceManager;
        private InventoryItem _inventoryItem;
        private CraftedItem _item;

        private void Awake()
        {
            _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();
        }

        protected override void OnShow(object args = null)
        {
            base.OnShow(args);

            _item = args as CraftedItem;

            if (_item == null)
            {
                Debug.LogError("Item is null".AddColorTag(Color.red));
                return;
            }

            _itemName.text = _item.Title;

            if (_item is EquipingItem equipingItem)
            {
                _itemType.text = equipingItem.EquipSlotType.ToString();
            }
            else
            {
                _itemType.text = "Item";
            }

            UpdateInventoryItem();
            UpdateItemNameColor();
            UpdateAffixes();
            UpdateCraftButtonState();
            UpdateRequiredResources();
            UpdateItemStats();

            EventAggregator.Add<ResourceModifiedEvent>(OnResourceModified);
        }

        protected override void OnShowLate()
        {
            RebuildLayouts();
        }

        protected override void OnHide()
        {
            base.OnHide();

            EventAggregator.Add<ResourceModifiedEvent>(OnResourceModified);
        }

        private void OnResourceModified(ResourceModifiedEvent sender)
        {
            UpdateCraftButtonState();
            UpdateRequiredResources();
        }

        private void UpdateItemStats()
        {
            ReleaseAllStatElements();

            if (_item is WeaponItem weaponItem)
            {
                var container = ViewGenerator.GetOrCreateItemView<StatsInfoElement>(GameConstants.View.ItemCraftStatsContainer);
                container.SetParent(_statsHolder);
                container.SetKey("Damage");
                container.SetValue($"{weaponItem.Damage.x}-{weaponItem.Damage.y}");

                _statsInfoElementContainers.Add(container);
            }

            foreach (var stat in _item.Stats.BaseStats)
            {
                var container = ViewGenerator.GetOrCreateItemView<StatsInfoElement>(GameConstants.View.ItemCraftStatsContainer);
                container.SetParent(_statsHolder);
                container.SetKey(stat.Key.ToString());
                container.SetValue(stat.Value.ToString(), true);

                _statsInfoElementContainers.Add(container);
            }
        }

        private void UpdateRequiredResources()
        {
            _requiredResourcesHolder.gameObject.SetActive(_item.Price.Count > 0);

            ReleaseAllRequiredResources();

            foreach (ResourcesData element in _item.Price)
            {
                ResourceRequiredContainer container = ViewGenerator.GetOrCreateItemView<ResourceRequiredContainer>(GameConstants.View.ResourceRequiredContainer);
                container.SetParent(_requiredResourcesHolder);
                container.SetResourcesData(element);

                _resourceRequiredContainers.Add(container);
            }

            RebuildLayouts();
        }

        private void UpdateCraftButtonState()
        {
            bool hasResources = true;

            if (_item.Price.Count > 0)
            {
                hasResources = _item.Price.All(y => _gameResourceManager.HasResource(y.Type, y.Value));
            }

            _createTimerText.text = TimeSpan.FromSeconds(_item.TimeCreationTick).ToString(@"hh\:mm\:ss");
            _buttonCraft.gameObject.SetActive(hasResources);
        }

        private void UpdateAffixes()
        {
            if (_item is WeaponItem weaponItem)
            {
                _affixesBar.SetAffixes(weaponItem.AffixesIds);
            }
        }

        private void UpdateInventoryItem()
        {
            if (_inventoryItem == null)
            {
                _inventoryItem = ViewGenerator.GetOrCreateItemView<InventoryItem>(GameConstants.View.InventoryItem);
                _inventoryItem.SetParent(_inventoryItemHolder);
                _inventoryItem.Transform.localPosition = Vector3.zero;
                _inventoryItem.RectTransform.sizeDelta = _inventoryItemScale;
            }

            _inventoryItem.SetItem(_item);
        }

        private void UpdateItemNameColor()
        {
            if (_item is EquipingItem equipingItem)
            {
                var data = ContentProvider.Graphic.SpriteBank.ItemsRarity.FirstOrDefault(x => x.Rarity == equipingItem.Rarity);
                _itemName.color = data?.ColorItemName ?? _defaultItemNameColor;
                return;
            }

            _itemName.text = string.Empty;
        }

        private void ReleaseAllRequiredResources()
        {
            foreach (var element in _resourceRequiredContainers)
            {
                element.ReleaseItemView();
            }

            _resourceRequiredContainers.Clear();
        }  
        
        private void ReleaseAllStatElements()
        {
            foreach (var element in _statsInfoElementContainers)
            {
                element.ReleaseItemView();
            }

            _statsInfoElementContainers.Clear();
        }

        private void RebuildLayouts()
        {
            foreach (var element in _layoutGroups)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(element);
            }
        }

        public void OnCraftButtonPressed()
        {

        }

#if UNITY_EDITOR
        [ContextMenu("Resize item")]
        public void ResizeItem()
        {
            for (int i = 0; i < _inventoryItemHolder.transform.childCount; i++)
            {
                _inventoryItemHolder.GetChild(i).GetComponent<RectTransform>().sizeDelta = _inventoryItemScale;
            }
        }
#endif
    }
}