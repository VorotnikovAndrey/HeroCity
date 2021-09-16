using Gameplay.Characters.Models;
using Utils.Events;

namespace Events
{
    public class CreateCharacterEvent : BaseEvent
    {
        public BaseCharacterModel Model;
    }
}