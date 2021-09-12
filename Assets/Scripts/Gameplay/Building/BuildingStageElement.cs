using CameraSystem;
using UnityEngine;

namespace Gameplay.Building
{
    [RequireComponent(typeof(CameraOffsetParams))]
    public class BuildingStageElement : MonoBehaviour
    {
        public GameObject Object;
        public int Stage;
        public CameraOffsetParams CameraOffset;

        private void OnValidate()
        {
            CameraOffset = GetComponent<CameraOffsetParams>();
        }
    }
}