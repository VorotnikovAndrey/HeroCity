using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Characters.AI.Behaviors
{
    [CreateAssetMenu(fileName = "BehaviorsData", menuName = "Characters/AI/BehaviorsData")]
    public class BehaviorsData : ScriptableObject
    {
        public List<ElementsBehaviorContainer> Containers = new List<ElementsBehaviorContainer>();
    }
}