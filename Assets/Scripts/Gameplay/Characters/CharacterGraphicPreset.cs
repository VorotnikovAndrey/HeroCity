using System;
using System.Collections.Generic;
using System.Linq;
using Source;
using UnityEngine;

namespace Gameplay.Characters
{
    [CreateAssetMenu(fileName = "CharacterGraphicPreset", menuName = "Characters/Graphic Preset")]
    public class CharacterGraphicPreset : ScriptableObject
    {
        public List<CharacterGraphicType> Data = new List<CharacterGraphicType>();

        public CharacterGraphicPresetPair Get(string id, CharacterType type)
        {
            var collection = Data.FirstOrDefault(x => x.Type == type);
            var element = collection?.Value.FirstOrDefault(x => x.Id == id);
            return element;
        }

        public CharacterGraphicPresetPair GetRandom(CharacterType type)
        {
            var collection = Data.FirstOrDefault(x => x.Type == type);
            return collection?.Value.GetRandom();
        }
    }

    [Serializable]
    public class CharacterGraphicType
    {
        public CharacterType Type;
        public List<CharacterGraphicPresetPair> Value;
    }

    [Serializable]
    public class CharacterGraphicPresetPair
    {
        public string Id;
        public CharacterType Type;
        public GameObject Object;
    }
}