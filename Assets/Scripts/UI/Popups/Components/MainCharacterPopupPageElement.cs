using System;
using System.Linq;
using Content;
using Gameplay.Characters.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Popups.Components
{
    public class MainCharacterPopupPageElement : CharacterPopupPageElement
    {
        [SerializeField] public Image _rarity;
        [SerializeField] public LocalEvents _events;

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

        public void OnPressPrevButton()
        {
            _events.EmitSwitchCharacter.Invoke(-1);
        }

        public void OnPressNextButton()
        {
            _events.EmitSwitchCharacter.Invoke(1);
        }

        [Serializable]
        public class LocalEvents
        {
            public UnityEvent<int> EmitSwitchCharacter;
        }
    }
}