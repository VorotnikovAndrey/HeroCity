using System;
using System.Collections.Generic;
using CameraSystem;
using Content;
using UI.Popups.Components;
using UnityEngine;
using Zenject;

namespace Gameplay.Building.View
{
    public class BuildingView : MonoBehaviour
    {
        [HideInInspector] public string BuildingId;

        public List<BuildingStateContainer> States = new List<BuildingStateContainer>();
        public UpgradeBar UpgradeBar;
        public BoxCollider Collider;

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