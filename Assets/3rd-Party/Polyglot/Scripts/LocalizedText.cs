using TMPro;
using UnityEngine;

namespace Polyglot
{
    [AddComponentMenu("UI/Localized Text", 11)]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : LocalizedTextComponent<TextMeshProUGUI>
    {
        [SerializeField] private bool _toUpper = false;

        public void OnValidate()
        {
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();
            }
        }

        protected override void SetText(TextMeshProUGUI text, string value)
        {
            if (text == null)
            {
                Debug.LogWarning("Missing Text Component on " + gameObject, gameObject);
                return;
            }

            text.text = _toUpper ? value.ToUpper() : value;
        }

        protected override void UpdateAlignment(TextMeshProUGUI text, LanguageDirection direction)
        {
            if (IsOppositeDirection(text.alignment, direction))
            {
                switch (text.alignment)
                {
                    case TextAlignmentOptions.TopLeft:
                        text.alignment = TextAlignmentOptions.TopLeft;
                        break;
                    case TextAlignmentOptions.TopRight:
                        text.alignment = TextAlignmentOptions.TopRight;
                        break;
                    case TextAlignmentOptions.MidlineLeft:
                        text.alignment = TextAlignmentOptions.MidlineRight;
                        break;
                    case TextAlignmentOptions.MidlineRight:
                        text.alignment = TextAlignmentOptions.MidlineLeft;
                        break;
                    case TextAlignmentOptions.BottomLeft:
                        text.alignment = TextAlignmentOptions.BottomRight;
                        break;
                    case TextAlignmentOptions.BottomRight:
                        text.alignment = TextAlignmentOptions.BottomLeft;
                        break;
                }
            }
        }

        private bool IsOppositeDirection(TextAlignmentOptions alignment, LanguageDirection direction)
        {
            return direction == LanguageDirection.LeftToRight && IsAlignmentRight(alignment) || direction == LanguageDirection.RightToLeft && IsAlignmentLeft(alignment);
        }

        private bool IsAlignmentRight(TextAlignmentOptions alignment)
        {
            return alignment == TextAlignmentOptions.BottomRight || alignment == TextAlignmentOptions.MidlineRight || alignment == TextAlignmentOptions.TopRight;
        }

        private bool IsAlignmentLeft(TextAlignmentOptions alignment)
        {
            return alignment == TextAlignmentOptions.BottomLeft || alignment == TextAlignmentOptions.MidlineLeft || alignment == TextAlignmentOptions.TopLeft;
        }
    }
}