using System;
using System.Collections.Generic;
using Characters;
using Gameplay.Characters;
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
        [OneLine] public List<SpriteBankRarityElement> ItemsRarity = new List<SpriteBankRarityElement>();
        [Space]
        public List<SpriteBankEquipElement> Items = new List<SpriteBankEquipElement>();
        [Space]
        [OneLine] public List<SpriteBankElement> AffixesIcons = new List<SpriteBankElement>();
        [Space]
        [OneLine] public List<SpriteBankHeroClassTypeElement> HeroClassIcons = new List<SpriteBankHeroClassTypeElement>();
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
        public Color Color;
        public Sprite Sprite;
    }

    [Serializable]
    public class SpriteBankEquipElement
    {
        public EquipSlotType SlotType;
        public List<SpriteBankElement> Data;
    }

    [Serializable]
    public class SpriteBankHeroClassTypeElement
    {
        public HeroClassType Class;
        public Sprite Sprite;
    }
}