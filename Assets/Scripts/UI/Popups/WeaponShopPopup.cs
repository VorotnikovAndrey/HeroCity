using System.Collections.Generic;
using System.Linq;
using Content;
using Events;
using Gameplay.Craft;
using PopupSystem;
using UnityEngine;
using Utils;
using Utils.ObjectPool;
using Utils.PopupSystem;

namespace UI.Popups
{
    public class WeaponShopPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.WeaponShop;

        [SerializeField] private List<WeaponCraftTypeButton> _buttons;

        private readonly List<ItemCraftContainer> _containers = new List<ItemCraftContainer>();

        private void OnValidate()
        {
            _buttons = GetComponentsInChildren<WeaponCraftTypeButton>().ToList();
        }

        protected override void OnStart()
        {
            var itemsData = ContentProvider.Economies.WeaponShopEconomy.Data.OrderBy(x => x.Index);

            foreach (var element in itemsData)
            {
                var view = ViewGenerator.GetOrCreateItemView<ItemCraftContainer>(GameConstants.View.ItemCraftContainer.WeaponCraftContainer);
                view.SetItem(element.Item);

                var holder = _buttons.FirstOrDefault(x => x.Types.Contains(element.Item.WeaponType));
                if (holder != null)
                {
                    holder.AddItem(view);
                }
                else
                {
                    Debug.LogError("Holder is not found".AddColorTag(Color.red));
                }

                _containers.Add(view);
            }

            ShowTypes(_buttons[0].Types);
        }

        protected override void OnShow(object args = null)
        {

        }

        public void ShowTypes(List<WeaponType> types)
        {
            foreach (var button in _buttons)
            {
                button.SetState(button.Types == types);
            }
        }

        protected override void OnHide()
        {
            EventAggregator.SendEvent(new BuildingViewUnSelectedEvent
            {
                ReturnToPrevPos = false
            });
        }
    }
}