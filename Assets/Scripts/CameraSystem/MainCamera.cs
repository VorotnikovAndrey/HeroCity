using UnityEngine;
using Utils.ObjectPool;

namespace CameraSystem
{
    public class MainCamera : AbstractBaseView, ICamera
    {
        public CameraType CameraType => CameraType.Main;
    }
}