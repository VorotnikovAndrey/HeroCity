using System;
using System.Collections.Generic;
using System.Linq;
using OneLine;
using UnityEngine;

namespace Utils
{

    [CreateAssetMenu(fileName = "SpriteBankData", menuName = "SO/SpriteBank")]
    public class SpriteBank : ScriptableObject
    {
        [OneLine] public List<SpriteBankElement> Data = new List<SpriteBankElement>();

        public bool HasId(string id)
        {
            return Data.Any(x => x.Id == id);
        }

        public Sprite GetSprite(string id)
        {
            return Data.FirstOrDefault(x => x.Id == id)?.Sprite;
        }
    }

    [Serializable]
    public class SpriteBankElement
    {
        public string Id;
        public Sprite Sprite;
    }
}