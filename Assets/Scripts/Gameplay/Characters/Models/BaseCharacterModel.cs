using Characters;
using Gameplay.Movement;
using Utils.ObjectPool;

namespace Gameplay.Characters.Models
{
    public class BaseCharacterModel
    {
        public CharacterType CharacterType;
        public IView View;
        public string GraphicPresetId;
        public Stats Stats;
        public IMovable Movement;
    }
}