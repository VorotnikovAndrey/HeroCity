using Newtonsoft.Json;
using UnityEngine;

namespace Economies
{
    public abstract class EconomyFile : ScriptableObject
    {
        [JsonIgnore] public EconomyFilesContainer WebAssets;
    }
}