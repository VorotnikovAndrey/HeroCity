﻿using Gameplay.Characters.Models;
using Utils.Events;

namespace Events
{
    public class CharacterLeavedFromLocationEvent : BaseEvent
    {
        public BaseCharacterModel Model;
    }
}