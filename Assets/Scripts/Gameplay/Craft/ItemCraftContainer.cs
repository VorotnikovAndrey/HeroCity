using System;
using System.Linq;
using Content;
using Gameplay.Equipments;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.ObjectPool;

namespace Gameplay.Craft
{
    public class ItemCraftContainer : AbstractBaseViewUI
    {
        [SerializeField] private RectTransform _inventoryItemHolder;
        [SerializeField] private Vector2 _inventoryItemScale;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private ItemLevelBar _itemLevel;
        [SerializeField] private ItemCraftAffixesBar _affixesBar;
        [SerializeField] private Button _selectButton;
        [Space]
        [SerializeField] private GameObject _progressHolder;
        [SerializeField] private Image _progressFillImage;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private float _defaultY;
        [SerializeField] private float _progressY;

        private ItemCraftContainerState _state;
        private InventoryItem _inventoryItem;
        private Item _item;

        public void SetItem(Item item)
        {
            _item = item;

            _title.text = _item.Title;
            _description.text = _item.Description;

            UpdateInventoryItem();
            UpdateAffixes();
            UpdateItemLevel();
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

        private void UpdateAffixes()
        {
            if (_item is WeaponItem weaponItem)
            {
                _affixesBar.SetAffixes(weaponItem.AffixesIds);
            } 
        }

        private void UpdateItemLevel()
        {
            // TODO:
        }

        public void SetState(ItemCraftContainerState state)
        {
            switch (state)
            {
                case ItemCraftContainerState.Default:
                    RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, _defaultY);
                    _progressHolder.SetActive(false);
                    break;
                case ItemCraftContainerState.Crafting:
                    RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, _progressY);
                    _progressHolder.SetActive(true);
                    break;
                case ItemCraftContainerState.Completed:
                    RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, _progressY);
                    _progressHolder.SetActive(true);
                    break;
                default:
                    return;
            }
        }

        public void OnButtonSelectPressed()
        {

        }
    }
}