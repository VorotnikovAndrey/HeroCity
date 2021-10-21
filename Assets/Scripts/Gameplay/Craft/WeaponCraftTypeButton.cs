using System;
using System.Collections.Generic;
using System.Linq;
using UI.Popups;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Craft
{
    public class WeaponCraftTypeButton : MonoBehaviour
    {
        [SerializeField] private WeaponShopPopup _weaponShopPopup;
        [SerializeField] private Image _colorImage;
        [SerializeField] private Image _focusImage;
        [Space]
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        [Space]
        [SerializeField] private List<WeaponType> _types;
        [Space] 
        [SerializeField] private WeaponCraftContent _content;

        public List<WeaponType> Types => _types;

        private void OnValidate()
        {
            string types = _types.Aggregate(string.Empty, (current, type) => current + $" [{type}]");

            gameObject.name = "Button" + types;
            
            if (_content != null)
            {
                _content.gameObject.name = "Content for" + types;
            }
        }

        public void AddItem(ItemCraftContainer item)
        {
            item.Transform.SetParent(_content.Content);
        }

        public void OnButtonPressed()
        {
            _weaponShopPopup.ShowTypes(_types);
        }

        public void SetState(bool state)
        {
            _focusImage.enabled = state;
            _colorImage.color = state ? _selectedColor : _unselectedColor;
            _content.gameObject.SetActive(state);
        }
    }
}