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
        public List<CharacterGraphicPresetPair> Data = new List<CharacterGraphicPresetPair>();

        public GameObject Get(string id)
        {
            return Data.FirstOrDefault(x => x.Id == id)?.Object;
        }

        public string GetRandom()
        {
            return Data.GetRandom().Id;
        }
    }

    [Serializable]
    public class CharacterGraphicPresetPair
    {
        public string Id;
        public GameObject Object;
    }
}