using Utils.ObjectPool;

namespace CameraSystem
{
    public sealed class CameraManager
    {
        public IView ActiveCamera { get; private set; }

        public void SetCameraType(GameCameraType type)
        {
            ActiveCamera?.ReleaseItemView();
            ActiveCamera = ViewGenerator.GetOrCreateItemView(string.Format(GameConstants.Base.CameraFormat, type));
        }
    }
}