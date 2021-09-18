using Gameplay.Characters.Models;

namespace Gameplay.Characters.AI
{
    public interface IAIController
    {
        public void Initialize(BaseCharacterModel model);
        public void DeInitialize();
        public void Start();
        public void Stop();
    }
}