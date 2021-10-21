using System.Collections.Generic;
using PopupSystem;
using UI.Popups.Components;
using UnityEngine;

namespace Gameplay.Building.View
{
    public class BuildingView : MonoBehaviour
    {
        [HideInInspector] public string BuildingId;

        public PopupType ShowablePopup;
        [Space]
        public List<BuildingStateContainer> States = new List<BuildingStateContainer>();
        public UpgradeBar UpgradeBar;
        public BoxCollider Collider;

        public BuildingStateContainer ActiveContainer { get; private set; }
        public BuildingStageElement ActiveStageElement { get; private set; }

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