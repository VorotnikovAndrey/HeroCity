using Gameplay.Characters;
using UnityEngine;
using Utils.Events;

namespace Events
{
    public class CharacterViewSelectedEvent : BaseEvent
    {
        public BaseCharacterView View;
        public Vector3 TapPosition;
    }
}