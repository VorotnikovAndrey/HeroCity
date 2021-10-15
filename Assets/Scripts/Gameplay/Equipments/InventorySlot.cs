using System;
using UnityEngine;

namespace Gameplay.Equipments
{
    public class InventorySlot : MonoBehaviour
    {
        public ItemSlotType SlotType = ItemSlotType.Head;
        public string Id;

        private RectTransform _rectTransform;
        private InventoryItem _inventoryItem;

        private void OnValidate()
        {
            gameObject.name = $"Slot [{SlotType}]";
            Id ??= Guid.NewGuid().ToString();
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Snap(InventoryItem inventoryItem)
        {
            _inventoryItem = inventoryItem;
            _inventoryItem.SetParent(transform);
            _inventoryItem.RectTransform.localPosition = Vector3.zero;
            _inventoryItem.RectTransform.sizeDelta = _rectTransform.sizeDelta;
        }

        public void Drop()
        {
            _inventoryItem = null;
        }

        public void OnButtonClick()
        {
            // TODO: Open select items
        }
    }
}