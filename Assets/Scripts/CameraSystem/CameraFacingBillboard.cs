using UnityEngine;
using Zenject;

namespace CameraSystem
{
    public class CameraFacingBillboard : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = ProjectContext.Instance.Container.Resolve<CameraManager>().ActiveCameraComponent;
        }

        private void Update()
        {
            if (_camera == null)
            {
                return;
            }

            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
                _camera.transform.rotation * Vector3.up);
        }
    }
}
