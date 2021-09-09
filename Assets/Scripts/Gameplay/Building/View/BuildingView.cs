using System.Collections.Generic;
using CameraSystem;
using UnityEngine;

namespace Gameplay.Building.View
{
    public class BuildingView : MonoBehaviour
    {
        [HideInInspector] public string BuildingId;

        [SerializeField] public List<BuildingStateContainer> States = new List<BuildingStateContainer>();

        public CameraOffsetParams CameraOffset { get; private set; }
        public BuildingStateContainer ActiveContainer { get; private set; }

        public void SetState(BuildingState buildingState)
        {
            foreach (BuildingStateContainer state in States)
            {
                bool valid = state.State == buildingState;
                state.Object.SetActive(valid);

                if (valid)
                {
                    ActiveContainer = state;
                }
            }
        }

        public void SetStage(int stage)
        {
            foreach (var element in ActiveContainer.Stages)
            {
                bool value = element.transform.GetSiblingIndex() == stage;
                element.SetActive(value);

                if (value)
                {
                    CameraOffset = element.GetComponent<CameraOffsetParams>();
                }
            }
        }
    }
}