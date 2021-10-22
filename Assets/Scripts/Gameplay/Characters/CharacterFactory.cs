using System.Linq;
using Characters;
using Content;
using Gameplay.Characters.Models;
using Gameplay.Equipments;
using Gameplay.Movement;
using Source;
using UnityEngine;

namespace Gameplay.Characters
{
    public static class CharacterFactory
    {
        public static BaseCharacterModel Get(CharacterType type, Gender gender, Rarity rarity)
        {
            switch (type)
            {
                case CharacterType.Hero:
                    return GetHero(gender, rarity);
                default:
                    return null;
            }
        }

        private static HeroModel GetHero(Gender gender, Rarity rarity)
        {
            return new HeroModel
            {
                CharacterType = CharacterType.Hero,
                Movement = new WaypointMovement(),
                Stats = new Stats(),
                Name = ContentProvider.Graphic.CharacterNames.Data.FirstOrDefault(x => x.Gender == gender)?.Info.GetRandom().Name,
                Gender = gender,
                Rarity = rarity,
                Inventory = new CharacterInventory(),
                HeroClassType = (HeroClassType)Random.Range(0, 6)
            };
        }
    }
}