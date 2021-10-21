using System.Collections.Generic;
using System.Linq;
using Gameplay.Equipments;
using UnityEngine;
using Utils.ObjectPool;
using Zenject;

namespace UI.Popups.Components
{
    public class EquipmentCharacterPopupPageElement : CharacterPopupPageElement
    {
        [SerializeField] private List<InventorySlot> _slots = new List<InventorySlot>();

        private List<InventoryItem> _inventoryItems = new List<InventoryItem>();

        private void OnValidate()
        {
            _slots = transform.GetComponentsInChildren<InventorySlot>(true).ToList();
        }

        public override void Show()
        {
            base.Show();

            _rawModelHolder.ResetRotation();

            LoadEquip();
        }

        public override void Hide()
        {
            foreach (var slot in _slots)
            {
                slot.Drop();
            }

            foreach (var inventoryItem in _inventoryItems)
            {
                inventoryItem.ReleaseItemView();
            }

            _inventoryItems.Clear();

            base.Hide();
        }

        public void EmitVelocity(Vector3 velocity)
        {
            _rawModelHolder.ApplyVelocity(velocity);
        }

        private void LoadEquip()
        {
            foreach (var item in _model.Inventory.Items)
            {
                if (!(item is EquipingItem equipingItem))
                {
                    continue;
                }

                var inventoryItem = ViewGenerator.GetOrCreateItemView<InventoryItem>(GameConstants.View.InventoryItem);
                inventoryItem.Initialize(item);

                _slots.FirstOrDefault(x => x._equipSlotType == equipingItem.EquipSlotType)?.Snap(inventoryItem);

                _inventoryItems.Add(inventoryItem);
            }
        }
    }
}