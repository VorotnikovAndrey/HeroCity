using System.Collections.Generic;
using UI.Popups.Components;
using UnityEngine;
using Utils;

namespace Gameplay.Building.View
{
    public class BuildingView : MonoBehaviour
    {
        [HideInInspector] public string BuildingId;

        public List<BuildingStateContainer> States = new List<BuildingStateContainer>();
        public UpgradeBar UpgradeBar;
        public BoxCollider Collider;

        public BuildingStateContainer ActiveContainer { get; private set; }
        public BuildingStageElement ActiveStageElement { get; private set; }

        public void SetState(BuildingState buildingState)
        {
            Debug.Log($"{BuildingId.AddColorTag(Color.yellow)} set state {buildingState.AddColorTag(Color.yellow)}".AddColorTag(Color.red));

            foreach (BuildingStateContainer state in States)
            {
                bool valid = state.State == buildingState;
                state.Object.SetActive(valid);

                if (valid)
                {
                    ActiveContainer = state;
                }
            }

            switch (buildingState)
            {
                case BuildingState.Inactive:
                case BuildingState.Active:
                    Collider.enabled = true;
                    break;
                case BuildingState.NotAvailable:
                case BuildingState.Upgrade:
                    Collider.enabled = false;
                    break;
            }
        }

        public void SetStage(int stage)
        {
            Debug.Log($"{BuildingId.AddColorTag(Color.yellow)} set stage {stage.AddColorTag(Color.yellow)}".AddColorTag(Color.red));

            foreach (var element in ActiveContainer.Stages)
            {
                bool value = element.Stage == stage;
                element.Object.SetActive(value);

                if (value)
                {
                    ActiveStageElement = element;
                }
            }
        }
    }
}