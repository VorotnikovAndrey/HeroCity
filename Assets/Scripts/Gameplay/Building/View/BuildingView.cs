using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Building.View
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private string Id;
        [SerializeField] private List<GameObject> Stages = default;

        public void SetStages(int value)
        {
            foreach (GameObject element in Stages)
            {
                element.SetActive(element.transform.GetSiblingIndex() == value);
            }
        }
    }
}