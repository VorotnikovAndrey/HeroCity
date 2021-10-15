using Gameplay.Characters.Models;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Popups.Components
{
    public class CharacterPopupPageElement : MonoBehaviour
    {
        public int Index;
        public GameObject Object;

        [SerializeField] public TextMeshProUGUI _characterName;
        [SerializeField] public TextMeshProUGUI _characterDescription;

        protected CharacterRawModelHolder _rawModelHolder;
        protected BaseCharacterModel _model;

        private void OnValidate()
        {
            if (Object == null)
            {
                Object = gameObject;
            }
        }

        private void Awake()
        {
            _rawModelHolder = ProjectContext.Instance.Container.Resolve<CharacterRawModelHolder>();
        }

        public virtual void SetModel(BaseCharacterModel model)
        {
            _model = model;

            _characterName.text = model.Name;
            _characterDescription.text = $"{model.CharacterType} - level {model.Stats.Level}";
        }

        public virtual void Show()
        {
            Object.SetActive(true);
        }

        public virtual void Hide()
        {
            Object.SetActive(false);
        }

        public void DropData()
        {
            _model = null;
        }
    }
}