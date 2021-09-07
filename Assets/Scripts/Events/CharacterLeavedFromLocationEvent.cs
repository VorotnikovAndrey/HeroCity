using Characters.Models;
using Gameplay;
using Utils.Events;

namespace Events
{
    public class CharacterLeavedFromLocationEvent : BaseEvent
    {
        public BaseCharacterModel Model;
    }
}