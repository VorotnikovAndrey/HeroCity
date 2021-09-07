using UnityEngine;
using Utils.ObjectPool;

namespace CameraSystem
{
    public class MainCamera : AbstractBaseView, ICamera
    {
        public GameCameraType GameCameraType => GameCameraType.Lobby;
    }
}