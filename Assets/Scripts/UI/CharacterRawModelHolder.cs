using Gameplay.LightSystem;
using UnityEngine;
using Zenject;

namespace UI
{
    public class CharacterRawModelHolder : MonoBehaviour
    {
        [SerializeField] private Transform _holder;
        [SerializeField] private GameObject _cameraParent;
        [SerializeField] private Vector3 _defaultRotate;
        [SerializeField] private Material _originCharacterMaterial;

        private GameObject _model;
        private MainDirectionLight _directionLight;

        public Transform Holder => _holder;

        public void Awake()
        {
            ProjectContext.Instance.Container.BindInstances(this);
            _directionLight = ProjectContext.Instance.Container.Resolve<MainDirectionLight>();
        }

        public void OnDestroy()
        {
            ProjectContext.Instance.Container.Unbind<CharacterRawModelHolder>();
            _directionLight = null;
        }

        public void Show(GameObject model)
        {
            if (_model != null)
            {
                Destroy(_model);
            }

            _directionLight.SecondLight.gameObject.SetActive(true);

            _model = model;
            _model.transform.SetParent(_holder);
            _model.transform.localEulerAngles = Vector3.zero;
            _model.transform.localPosition = Vector3.zero;

            var skinMeshRenderer = _model.GetComponentInChildren<SkinnedMeshRenderer>();
            if (skinMeshRenderer != null)
            {
                skinMeshRenderer.material = _originCharacterMaterial;
                skinMeshRenderer.gameObject.layer = 7;
            }

            ResetRotation();
        }

        public void Hide()
        {
            if (_model != null)
            {
                Destroy(_model);
            }

            _directionLight.SecondLight.gameObject.SetActive(false);
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