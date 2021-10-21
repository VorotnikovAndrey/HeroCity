using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Craft
{
    public class ItemCraftAffixSlot : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void SetIcon(Sprite sprite)
        {
            _icon.enabled = sprite != null;
            _icon.sprite = sprite;
        }
    }
}