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
        [SerializeField] public LocalEvents _events;

        public override void Show()
        {
            base.Show();

            _rawModelHolder.ResetRotation();
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