using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Economies
{
    [CreateAssetMenu(fileName = "ImprovementEconomyData", menuName = "Economy/Improvement Economy")]
    public class ImprovementEconomy : EconomyFile
    {
        public List<ImprovementData> Data = new List<ImprovementData>();

        public ImprovementData Get(string id)
        {
            return Data.FirstOrDefault(x => x.Id == id);
        }
    }

    [Serializable]
    public class ImprovementData
    {
        public string Id;
        public string SpriteBankId;
        [TextArea(3, 10)] public string Description;
    }
}