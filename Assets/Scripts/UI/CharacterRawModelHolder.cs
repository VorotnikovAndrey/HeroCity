using UnityEngine;
using Zenject;

namespace UI
{
    public class CharacterRawModelHolder : MonoBehaviour
    {
        [SerializeField] private Transform _holder;
        [SerializeField] private GameObject _cameraParent;
        [SerializeField] private Vector3 _defaultRotate;

        private GameObject _model;

        public Transform Holder => _holder;

        public void Awake()
        {
            ProjectContext.Instance.Container.BindInstances(this);
        }

        public void OnDestroy()
        {
            ProjectContext.Instance.Container.Unbind<CharacterRawModelHolder>();
        }

        public void Show(GameObject model)
        {
            if (_model != null)
            {
                Destroy(_model);
            }

            _model = model;
            _model.transform.SetParent(_holder);
            _model.transform.localEulerAngles = Vector3.zero;
            _model.transform.localPosition = Vector3.zero;

            ResetRotation();
        }

        public void Hide()
        {
            if (_model != null)
            {
                Destroy(_model);
            }
        }

        public void SetState(bool state)
        {
            _cameraParent.SetActive(state);
        }

        public void ResetRotation()
        {
            _holder.localEulerAngles = _defaultRotate;
        }

        public void ApplyVelocity(Vector3 velocity)
        {
            _holder.Rotate(velocity);
        }
    }
}