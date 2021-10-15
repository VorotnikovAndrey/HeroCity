using System.Linq;
using Content;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.ObjectPool;

namespace Gameplay.Equipments
{
    public class InventoryItem : AbstractBaseViewUI
    {
        [SerializeField] private Image _rarity;
        [SerializeField] private Image _icon;

        private Item _item;

        public override void Initialize(object data)
        {
            base.Initialize(data);

            _item = data as Item;

            if (_item == null)
            {
                Debug.LogError("Item is null".AddColorTag(Color.red));
                return;
            }

            var bank = ContentProvider.Graphic.SpriteBank;

            _rarity.sprite = bank.ItemsRarity.FirstOrDefault(x => x.Rarity == _item.Rarity)?.Sprite;
            _icon.sprite = bank.Items.FirstOrDefault(x => x.Id == _item.IconId)?.Sprite;
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
    }
}