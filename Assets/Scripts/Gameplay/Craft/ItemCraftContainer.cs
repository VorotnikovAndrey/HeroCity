using System;
using System.Collections.Generic;
using System.Linq;
using Content;
using DG.Tweening;
using Events;
using Gameplay.Building.Models;
using Gameplay.Equipments;
using PopupSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Events;
using Utils.ObjectPool;

namespace Gameplay.Craft
{
    public class ItemCraftContainer : AbstractBaseViewUI
    {
        [SerializeField] private RectTransform _mainRect;
        [SerializeField] private RectTransform _inventoryItemHolder;
        [SerializeField] private Vector2 _inventoryItemScale;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private ItemCraftAffixesBar _affixesBar;
        [Space]
        [SerializeField] private GameObject _selectionButton;
        [SerializeField] private GameObject _progressHolder;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private GameObject _claimHolder;

        private ProductionBuildingModel _model;
        private InventoryItem _inventoryItem;
        private Item _item;
        private Tweener _tweener;

        public override void Initialize(object data)
        {
            base.Initialize(data);

            _model = data as ProductionBuildingModel;

            if (_model == null)
            {
                Debug.LogError("Model is null".AddColorTag(Color.red));
                return;
            }

            EventAggregator.Add<BeginProductionEvent>(OnBeginProductionEvent);
            EventAggregator.Add<EndProductionEvent>(OnEndProductionEvent);
        }

        public override void Deinitialize()
        {
            base.Deinitialize();

            EventAggregator.Remove<BeginProductionEvent>(OnBeginProductionEvent);
            EventAggregator.Remove<EndProductionEvent>(OnEndProductionEvent);

            _model = null;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (true)
            {
                UpdateState();
            }
        }

        public void SetItem(Item item)
        {
            _item = item;

            _title.text = _item.Title;
            _description.text = TryOverrideDescription();

            UpdateTitleColor();
            UpdateInventoryItem();
            UpdateAffixes();
            UpdateState();
        }

        private void UpdateState()
        {
            var state = ItemCraftContainerState.Default;
            ProductionData data = null;

            foreach (var element in _model.ProductionData)
            {
                if (element.ProductionId != _item.Id)
                {
                    continue;
                }

                data = element;
                state = data.Finished ? ItemCraftContainerState.Completed : ItemCraftContainerState.Crafting;
            }

            data?.TimeLeft.RemoveListener(UpdateProgressText);

            switch (state)
            {
                case ItemCraftContainerState.Default:
                    _selectionButton.SetActive(true);
                    _progressHolder.SetActive(false);
                    _claimHolder.SetActive(false);

                    break;
                case ItemCraftContainerState.Crafting:

                    if (data == null)
                    {
                        break;
                    }

                    data.TimeLeft.AddListener(UpdateProgressText);

                    UpdateProgressText(data.ProductionEndUnixTime - DateTimeUtils.GetCurrentTime());

                    var totalTime = data.ProductionEndUnixTime - data.ProductionStartUnixTime;
                    var timeLeft = data.ProductionEndUnixTime - DateTimeUtils.GetCurrentTime();
                    var progress = 1 - (float)timeLeft / totalTime;

                    _progressSlider.value = progress;

                    _tweener?.Kill();
                    _tweener = DOTween.To(() => _progressSlider.value, x => _progressSlider.value = x, 1f, timeLeft)
                        .SetEase(Ease.Linear).OnKill(() =>
                        {
                            _tweener = null;
                        });

                    _selectionButton.SetActive(true);
                    _progressHolder.SetActive(true);
                    _claimHolder.SetActive(false);

                    break;
                case ItemCraftContainerState.Completed:
                    _selectionButton.SetActive(false);
                    _progressHolder.SetActive(false);
                    _claimHolder.SetActive(true);

                    break;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_mainRect);
        }

        private string TryOverrideDescription()
        {
            if (_item is WeaponItem weaponItem)
            {
                return $"<sprite=4> {weaponItem.Damage.x} - {weaponItem.Damage.y}";
            }

            return _item.Description;
        }

        private void UpdateTitleColor()
        {
            var data = ContentProvider.Graphic.SpriteBank.ItemsRarity.FirstOrDefault(x => x.Rarity == _item.Rarity);
            
            _title.color = data?.ColorItemName ?? Color.white;
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

        public void OnButtonSelectPressed()
        {
            EventAggregator.SendEvent(new ShowPopupEvent<PopupType>
            {
                PopupType = PopupType.ItemCraft,
                Data = new List<object>
                {
                    _model,
                    _item
                }
            });
        }

        public void OnButtonClaimPressed()
        {
            ProductionData data = null;

            foreach (var element in _model.ProductionData)
            {
                if (element.ProductionId != _item.Id)
                {
                    continue;
                }

                data = element;
                break;
            }

            if (data == null)
            {
                Debug.LogError("Data is null".AddColorTag(Color.red));
                return;
            }

            EventAggregator.SendEvent(new ClaimProductionEvent
            {
                Data = data
            });

            UpdateState();
        }

        private void UpdateProgressText(long unixTime)
        {
            _progressText.text = DateTimeUtils.GetTimerText(DateTimeUtils.UnixTimeToDateTime(unixTime));
        }

        private void OnBeginProductionEvent(BeginProductionEvent sender)
        {
            if (sender.Data.ProductionId == _item.Id)
            {
                UpdateState();
            }
        }

        private void OnEndProductionEvent(EndProductionEvent sender)
        {
            if (sender.Data.ProductionId == _item.Id)
            {
                UpdateState();
            }
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