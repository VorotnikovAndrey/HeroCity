using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Building.View
{
    public class BuildingView : MonoBehaviour
    {
        [HideInInspector] public string BuildingId;

        [SerializeField] public List<GameObject> Stages = default;

        public void SetStages(int value)
        {
            foreach (GameObject element in Stages)
            {
                element.SetActive(element.transform.GetSiblingIndex() == value);
            }
        }
    }
}