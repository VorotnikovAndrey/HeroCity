using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonInteractableTextColorHelper : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private Button _button;

        [SerializeField] private Color _interactableColor;
        [SerializeField] private Color _nonInteractableColor;
        [Space] 
        [SerializeField] private TextMeshProUGUI _text;

        private void OnValidate()
        {
            _button = GetComponent<Button>();

            SetState(_button.interactable);
        }

        public void SetState(bool value)
        {
            _button.interactable = value;
            _text.color = value ? _interactableColor : _nonInteractableColor;
        }
    }
}