using Gameplay.Characters.AI;
using Gameplay.Movement;
using Newtonsoft.Json;
using UserSystem;
using Utils.ObjectPool;

namespace Gameplay.Characters.Models
{
    public class BaseCharacterModel
    {
        [JsonProperty] public CharacterSaveData SaveData = new CharacterSaveData();

        [JsonProperty] public string GraphicPresetId;
        [JsonProperty] public Stats Stats;
        [JsonProperty] public CharacterType CharacterType;

        [JsonIgnore] public IView View;
        [JsonIgnore] public IMovable Movement;
        [JsonIgnore] public IAIController AIController;
    }
}