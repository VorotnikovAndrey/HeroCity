using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Building.View
{
    public class BuildingView : MonoBehaviour
    {
        [HideInInspector] public string BuildingId;

        [SerializeField] public List<BuildingStateContainer> States = new List<BuildingStateContainer>();

        public void SetState(BuildingState buildingState, int stage)
        {
            foreach (var state in States)
            {
                bool valid = state.State == buildingState;
                state.Object.SetActive(valid);

                if (!valid)
                {
                    continue;
                }

                foreach (var element in state.Stages)
                {
                    element.SetActive(element.transform.GetSiblingIndex() == stage);
                }
            }
        }
    }
}