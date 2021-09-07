using UnityEngine;

namespace CameraSystem
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Camera/Camera Settings")]
    public class CameraSettings : ScriptableObject
    {
        public Vector3 Position;
        public Bounds Bounds;
        public float OrthographicSize;
    }
}