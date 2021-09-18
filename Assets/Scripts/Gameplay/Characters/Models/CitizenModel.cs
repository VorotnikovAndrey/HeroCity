using Newtonsoft.Json;

namespace Gameplay.Characters.Models
{
    public class CitizenModel : BaseCharacterModel
    {
        [JsonProperty] public string PersonalAIContainerName;
    }
}