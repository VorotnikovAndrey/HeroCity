using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Craft
{
    public class ItemLevelBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _layoutGroupTransform;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            SetText(100);
        }

        public void SetText(int value)
        {
            _text.text = value.ToString();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroupTransform);
        }
    }
}