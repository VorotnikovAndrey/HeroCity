using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Characters
{
    [CreateAssetMenu(fileName = "CharacterNames", menuName = "Characters/Character Names")]
    public class CharacterNames : ScriptableObject
    {
        public List<CharacterInfo> Data = new List<CharacterInfo>();

        [Serializable]
        public class CharacterInfo
        {
            public Gender Gender;
            public List<CharacterName> Info;
        }

        [Serializable]
        public class CharacterName
        {
            public string Name;
            [TextArea(0, 3)] public string Description;
        }
    }
}