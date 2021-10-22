using System.Linq;
using Content;
using Gameplay.Characters.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace UI.Popups.Components
{
    public class CharacterPopupPageElement : MonoBehaviour
    {
        public int Index;
        public GameObject Object;

        [SerializeField] public TextMeshProUGUI _characterName;
        [SerializeField] public Image _characterClassImage;

        protected CharacterRawModelHolder _rawModelHolder;
        protected HeroModel _model;

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
            _model = model as HeroModel;

            if (_model == null)
            {
                Debug.LogError("Model is null".AddColorTag(Color.red));
                return;
            }

            UpdateName();
            UpdateClassIcon();
        }

        private void UpdateName()
        {
            _characterName.text = _model.Name;
        }

        private void UpdateClassIcon()
        {
            if (_characterClassImage == null)
            {
                return;
            }

            _characterClassImage.sprite = ContentProvider.Graphic.SpriteBank.HeroClassIcons
                .FirstOrDefault(x => x.Class == _model.HeroClassType)?.Sprite;
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