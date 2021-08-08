using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    [CreateAssetMenu(menuName = "Language/LanguageAsset")]
    public class LanguageAsset : ScriptableObject 
    {
        [System.Serializable]
        public struct LanguageData
        {
            public string key;
            public string value;
        }
    
        public List<LanguageData> Data = new List<LanguageData>();
    }
}
