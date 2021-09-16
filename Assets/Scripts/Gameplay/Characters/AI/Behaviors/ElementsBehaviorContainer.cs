using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Characters.AI.Behaviors
{
    [CreateAssetMenu(fileName = "ElementsBehaviorContainer", menuName = "Characters/AI/ElementsBehaviorContainer")]
    public class ElementsBehaviorContainer : ScriptableObject
    {
        public List<ElementBehavior> Elements = new List<ElementBehavior>();
    }
}