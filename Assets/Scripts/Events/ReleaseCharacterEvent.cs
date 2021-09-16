using Gameplay.Characters.Models;
using Utils.Events;

namespace Events
{
    public class ReleaseCharacterEvent : BaseEvent
    {
        public BaseCharacterModel Model;
    }
}