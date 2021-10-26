using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Craft
{
    public class ItemCraftAffixSlot : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _defaultIcon;

        public void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite != null ? sprite : _defaultIcon;
        }
    }
}