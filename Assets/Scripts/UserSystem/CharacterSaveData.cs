using System.Collections.Generic;
using Newtonsoft.Json;

namespace UserSystem
{
    public class CharacterSaveData
    {
        [JsonProperty] public List<float> LastPosition;
    }
}