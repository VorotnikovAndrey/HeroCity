using System;
using System.Collections.Generic;
using Gameplay.Equipments;
using OneLine;
using UnityEngine;

namespace Utils
{

    [CreateAssetMenu(fileName = "SpriteBankData", menuName = "SO/SpriteBank")]
    public class SpriteBank : ScriptableObject
    {
        [OneLine] public List<SpriteBankElement> Data = new List<SpriteBankElement>();
        [Space]
        [OneLine] public List<SpriteBankElement> Items = new List<SpriteBankElement>();
        [Space]
        [OneLine] public List<SpriteBankRarityElement> ItemsRarity = new List<SpriteBankRarityElement>();
    }

    [Serializable]
    public class SpriteBankElement
    {
        public string Id;
        public Sprite Sprite;
    }

    [Serializable]
    public class SpriteBankRarityElement
    {
        public Rarity Rarity;
        public Sprite Sprite;
    }
}