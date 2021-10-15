using System.Linq;
using Content;
using Gameplay.Characters.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Components
{
    public class MainCharacterPopupPageElement : CharacterPopupPageElement
    {
        [SerializeField] public Image _rarity;

        public override void Show()
        {
            base.Show();

            _rawModelHolder.ResetRotation();
        }

        public override void SetModel(BaseCharacterModel model)
        {
            base.SetModel(model);

            _rarity.sprite = ContentProvider.Graphic.SpriteBank.ItemsRarity.FirstOrDefault(x => x.Rarity == model.Rarity)?.Sprite;
        }
    }
}