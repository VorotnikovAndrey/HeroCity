using Gameplay.Characters.AI;
using Gameplay.Equipments;
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
        [JsonProperty] public string Name;
        [JsonProperty] public Gender Gender;
        [JsonProperty] public Stats Stats;
        [JsonProperty] public CharacterInventory Inventory;
        [JsonProperty] public CharacterType CharacterType;
        [JsonProperty] public Rarity Rarity;

        [JsonIgnore] public IView View;
        [JsonIgnore] public IMovable Movement;
        [JsonIgnore] public IAIController AIController;
    }
}