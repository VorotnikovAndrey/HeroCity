using CameraSystem;
using UnityEngine;
using Utils.ObjectPool;

namespace Gameplay.Locations.View
{
    public class LocationView : AbstractBaseView
    {
        [HideInInspector] public string LocationId;
        public CameraSettings CameraSettings;
    }
}