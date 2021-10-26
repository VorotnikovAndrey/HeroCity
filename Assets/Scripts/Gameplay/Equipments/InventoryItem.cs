using System.Linq;
using Content;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.ObjectPool;

namespace Gameplay.Equipments
{
    public class InventoryItem : AbstractBaseViewUI
    {
        [SerializeField] private Image _rarityBackground;
        [SerializeField] private Image _rarityBorders;
        [SerializeField] private Image _icon;

        private Item _item;

        public override void Initialize(object data)
        {
            base.Initialize(data);

            SetItem(data as Item);
        }

        public void SetItem(Item item)
        {
            if (item == null)
            {
                Debug.LogError("Item is null".AddColorTag(Color.red));
                return;
            }

            _item = item;

            UpdateRarity();
            UpdateIcon();
        }

        private void UpdateRarity()
        {
            SpriteBankRarityElement data = ContentProvider.Graphic.SpriteBank.ItemsRarity.FirstOrDefault(x => x.Rarity == _item.Rarity);

            if (data == null)
            {
                Debug.LogError("Data is null".AddColorTag(Color.red));
                return;
            }

            _rarityBorders.sprite = data.Sprite;
            _rarityBackground.color = data.ColorBackground;
        }

        private void UpdateIcon()
        {
            if (_item is EquipingItem equipingItem)
            {
                var data = ContentProvider.Graphic.SpriteBank.Items.FirstOrDefault(x => x.SlotType == equipingItem.EquipSlotType);

                if (data != null)
                {
                    var spriteData = data.Data.FirstOrDefault(x => x.Id == equipingItem.IconId);

                    if (spriteData != null)
                    {
                        _icon.sprite = spriteData.Sprite;
                    }
                    else
                    {
                        Debug.LogError("Icon is not found");
                    }
                }
            }
        }

        public override void Deinitialize()
        {
            _item = null;

            base.Deinitialize();
        }

        public void OnButtonClick()
        {
            // TODO: Show item info popup
            Debug.Log($"Item {_item.Title.AddColorTag(Color.yellow)}".AddColorTag(Color.red));
        }

        public void SetRarityFromEditor(Color backgroundColor, Sprite bordersSprite)
        {
#if UNITY_EDITOR
            _rarityBorders.sprite = bordersSprite;
            _rarityBackground.color = backgroundColor;
#endif
        }
    }
}