using System;
using System.Collections.Generic;
using Gameplay.Equipments;
using UnityEngine;

namespace Economies
{
    [CreateAssetMenu(fileName = "WeaponShopEconomyData", menuName = "Economy/WeaponShop Economy")]
    public class WeaponShopEconomy : EconomyFile
    {
        public List<WeaponShopData> Data = new List<WeaponShopData>();
    }

    [Serializable]
    public class WeaponShopData
    {
        public int Index;
        public WeaponItem Item;
    }
}