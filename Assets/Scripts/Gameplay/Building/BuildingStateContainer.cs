using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Building
{
    [Serializable]
    public class BuildingStateContainer
    {
        public BuildingState State;
        public GameObject Object;
        public List<BuildingStageElement> Stages;
    }
}