using Utils.ObjectPool;

namespace CameraSystem
{
    public sealed class CameraManager
    {
        private IView ActiveCamera;

        public void SetCameraType(CameraType type)
        {
            ActiveCamera?.ReleaseItemView();
            ActiveCamera = ViewGenerator.GetOrCreateItemView(string.Format(GameConstants.Base.CameraFormat, type));
        }
    }
}