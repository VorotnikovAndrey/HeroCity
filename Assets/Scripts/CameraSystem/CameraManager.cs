using UnityEngine;
using Utils.ObjectPool;

namespace CameraSystem
{
    public sealed class CameraManager
    {
        public IView ActiveCameraView { get; private set; }
        public Camera ActiveCameraComponent { get; private set; }

        public void SetCameraType(GameCameraType type)
        {
            ActiveCameraView?.ReleaseItemView();
            ActiveCameraView = ViewGenerator.GetOrCreateItemView(string.Format(GameConstants.Base.CameraFormat, type));
            ActiveCameraComponent = ActiveCameraView.Transform.GetComponent<Camera>();
        }
    }
}