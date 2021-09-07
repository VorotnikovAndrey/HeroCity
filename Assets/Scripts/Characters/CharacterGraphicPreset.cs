using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "CharacterGraphicPreset", menuName = "Characters/Graphic Preset")]
    public class CharacterGraphicPreset : ScriptableObject
    {
        public List<CharacterGraphicPresetPair> Data = new List<CharacterGraphicPresetPair>();
    }

    [Serializable]
    public class CharacterGraphicPresetPair
    {
        public string Id;
        public GameObject Object;
    }
}