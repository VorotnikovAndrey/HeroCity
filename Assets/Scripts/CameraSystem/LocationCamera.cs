using System;
using DG.Tweening;
using InputSystem;
using Source;
using UnityEngine;
using Utils.ObjectPool;
using Zenject;

namespace CameraSystem
{
    [RequireComponent(typeof(Camera))]
    public class LocationCamera : AbstractBaseView, ICamera
    {
        public CameraType CameraType => CameraType.Location;

        [HideInInspector] [SerializeField] private Camera _cam;

        [SerializeField] private float _defaultDampSmoothTime = 0.2f;
        [SerializeField] private float _dampMaxSpeed = 40f;
        [SerializeField] private float _movementLerpSpeed = 3f;
        [SerializeField] private float _successiveVelocityMult = 0.2f;
        [SerializeField] private float _successiveVelocityThreshold = 1f;
        [SerializeField] private float _smoothTimeVelocityDependenceModif = 15;
        [SerializeField] private float _smoothTimeVelocityDependenceClamp = 0.5f;
        [SerializeField] private float _camDistanceFromTarget = 50;
        [SerializeField] private float _closeViewZoom = 3f;
        [SerializeField] private float _offsetYK;
        [SerializeField] private Vector3 _closeViewOffset;
        [Space]
        [SerializeField] private float _zoomSensitivity = 1f;
        [SerializeField] private float _zoomMinSize = 3f;
        [SerializeField] private float _zoomMaxSize = 5f;
        [SerializeField] private float _zoomElasticity = 0.2f;
        [SerializeField] private float _zoomElasticityLerpSpeed = 5f;
        [SerializeField] private float _zoomLerpSpeed = 2f;
        [Space]
        [SerializeField] private float _cameraHeight = 25f;
        [Space]
        [SerializeField] private float _targetMoveInDuration = 0.4f;
        [SerializeField] private AnimationCurve _targetMoveInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _targetMoveOutDuration = 0.4f;
        [SerializeField] private AnimationCurve _targetMoveOutCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _targetZoomInDuration = 0.4f;
        [SerializeField] private AnimationCurve _targetZoomInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _targetZoomOutDuration = 0.4f;
        [SerializeField] private AnimationCurve _targetZoomOutCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private const float FixedY = 5f;

        private Plane _plane;
        private Vector3 _prevCursorPos;
        private bool _prevIsFocused;
        private Bounds _bounds;
        private bool _inputBlocked;
        private CameraStates _state;
        private Vector3 _defaultStatePosition;
        private float _defaultStateOrthoSize;
        private Sequence _switchStateSeq;
        private Transform _currentViewTarget;
        private Vector3 _currentTargetPoint;
        private float _currentTargetZoom;
        private Vector3 _currentDampVelocity;
        private int _prevTouchCount;
        private float _currentDampSmoothTime;
        private Transform _followTarget;
        private IInputSystem _inputSystem;
        private bool _isFollowTargetNull;

        private Tweener _moveTweener;
        private Tweener _zoomTweener;

        public Camera Camera => _cam;
        public Vector3 CurrentCenterPlanePosition { get; private set; }

        private void Start()
        {
            _isFollowTargetNull = _followTarget == null;
        }

        private void OnValidate()
        {
            if (_cam == null)
            {
                _cam = GetComponent<Camera>();
            }
        }

        private void Awake()
        {
            ProjectContext.Instance.Container.BindInstances(this);
        }

        public void Init(IInputSystem inputSystem, CameraSettings settings)
        {
            _inputSystem = inputSystem;
            _bounds = new Bounds(Vector3.zero, Vector3.one * 50);
            _plane = new Plane(Vector3.up, Vector3.zero);
            Camera.transform.position = _currentTargetPoint = settings.Position;
            Camera.orthographicSize = _currentTargetZoom = settings.OrthographicSize;
            _currentDampSmoothTime = _defaultDampSmoothTime;
        }

        public void Update()
        {
            if (_inputSystem == null || _inputSystem.IsInputLocked || _switchStateSeq != null)
            {
                return;
            }

            CurrentCenterPlanePosition = PlanePosition(new Vector2(Screen.width / 2f, Screen.height / 2f));

            if (_state == CameraStates.Default)
            {
                if (Application.isMobilePlatform)
                {
                    UpdateMobileInput();
                }
                else
                {
                    UpdateStandaloneInput();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) && _state == CameraStates.BuildingView)
            {
                SwitchToDefaultState();
            }
        }

        private void LateUpdate()
        {
            switch (_state)
            {
                case CameraStates.Default:
                    UpdateMovement();
                    UpdateZoom();
                    break;
                case CameraStates.Following:
                    if (_isFollowTargetNull)
                    {
                        break;
                    }

                    _currentTargetPoint = _followTarget.position - Camera.transform.rotation * Vector3.forward * _camDistanceFromTarget;

                    //_currentTargetPoint.x = Mathf.Clamp(_currentTargetPoint.x, _bounds.min.x, _bounds.max.x);
                    //_currentTargetPoint.z = Mathf.Clamp(_currentTargetPoint.z, _bounds.min.z, _bounds.max.z);

                    UpdateMovement();
                    break;
                case CameraStates.BuildingView:
                    break;
                case CameraStates.Focus:
                    break;
            }

            _currentDampSmoothTime = _defaultDampSmoothTime *
                                     (1 - (_smoothTimeVelocityDependenceModif / _currentDampVelocity.magnitude).ClampTop(
                                         _smoothTimeVelocityDependenceClamp));
        }

        private void UpdateMovement()
        {
            Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, _currentTargetPoint,
                ref _currentDampVelocity, _currentDampSmoothTime,
                _dampMaxSpeed, Time.deltaTime);
        }

        private void UpdateZoom()
        {
            if (_currentTargetZoom > _zoomMaxSize)
            {
                _currentTargetZoom = Mathf.Lerp(_currentTargetZoom, _zoomMaxSize, _zoomElasticityLerpSpeed * Time.deltaTime);
            }

            if (_currentTargetZoom < _zoomMinSize)
            {
                _currentTargetZoom = Mathf.Lerp(_currentTargetZoom, _zoomMinSize, _zoomElasticityLerpSpeed * Time.deltaTime);
            }

            Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, _currentTargetZoom, Time.deltaTime * _zoomLerpSpeed);
        }

        private void UpdateMobileInput()
        {
            //Scroll
            if (Input.touchCount == 1)
            {
                Vector3 touchPos = Input.GetTouch(0).position;
                if (Input.GetTouch(0).phase == TouchPhase.Moved && _prevCursorPos != touchPos)
                {
                    Vector3 delta = PlanePositionDelta(touchPos);
                    AddDelta(delta);
                }

                _prevCursorPos = touchPos;
            }

            //Pinch
            if (Input.touchCount >= 2)
            {
                Vector3 pos1 = PlanePosition(Input.GetTouch(0).position);
                Vector3 pos2 = PlanePosition(Input.GetTouch(1).position);
                Vector3 pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
                Vector3 pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

                //calc zoom
                var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);

                var isEdgeCase = zoom == 0 || zoom > 10;

                if (!isEdgeCase)
                {
                    var orthoSize = _currentTargetZoom / zoom;
                    var elasticityMult = 1 + Mathf.Abs(_zoomElasticity);
                    _currentTargetZoom = Mathf.Clamp(orthoSize, _zoomMinSize / elasticityMult,
                        _zoomMaxSize * elasticityMult);
                }
            }

            if (Input.touchCount == 0 && _prevTouchCount > 0)
            {
                TrySuccessiveVelocity();
            }

            _prevTouchCount = Input.touchCount;
        }

        private void UpdateStandaloneInput()
        {
            var zoomAxisValue = Input.GetAxis("Mouse ScrollWheel");

            if (zoomAxisValue != 0)
            {
                _currentTargetZoom -= zoomAxisValue * _zoomSensitivity;
                var elasticityMult = 1 + Mathf.Abs(_zoomElasticity);
                _currentTargetZoom = Mathf.Clamp(_currentTargetZoom, _zoomMinSize / elasticityMult,
                    _zoomMaxSize * elasticityMult);
            }

            if (Input.GetMouseButton(0))
            {
                if (_prevIsFocused != Application.isFocused)
                {
                    _prevCursorPos = Input.mousePosition;
                }

                Vector3 delta = PlanePositionDelta(Input.mousePosition);

                if (Input.mousePosition != _prevCursorPos)
                {
                    AddDelta(delta);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                TrySuccessiveVelocity();
            }

            _prevCursorPos = Input.mousePosition;
            _prevIsFocused = Application.isFocused;
        }

        private void TrySuccessiveVelocity()
        {
            if (!(_currentDampVelocity.magnitude > _successiveVelocityThreshold))
            {
                return;
            }

            _currentTargetPoint += _currentDampVelocity * _successiveVelocityMult;
            _currentTargetPoint.x = Mathf.Clamp(_currentTargetPoint.x, _bounds.min.x, _bounds.max.x);
            _currentTargetPoint.z = Mathf.Clamp(_currentTargetPoint.z, _bounds.min.z, _bounds.max.z);
        }

        private void AddDelta(Vector3 delta)
        {
            Vector3 pos = _currentTargetPoint + delta;
            pos.x = Mathf.Clamp(pos.x, _bounds.min.x, _bounds.max.x);
            pos.z = Mathf.Clamp(pos.z, _bounds.min.z, _bounds.max.z);
            _currentTargetPoint = pos;
        }

        public void SwitchToViewTransform(Transform target, bool defaultOffset = true)
        {
            SwitchToViewTransform(target, _closeViewZoom, defaultOffset);
        }

        public void SwitchToViewTransform(Transform t, float orthoSize, bool defaultOffset)
        {
            if (_currentViewTarget == t)
            {
                return;
            }

            _currentViewTarget = t;
            _switchStateSeq?.Kill();

            if (_state == CameraStates.Default)
            {
                _currentTargetPoint = Camera.transform.position;
                _defaultStatePosition = transform.position;
                _defaultStateOrthoSize = Camera.orthographicSize;
            }

            _state = CameraStates.BuildingView;

            _switchStateSeq = DOTween.Sequence();
            Vector3 targetPos = GetTargetPosWithOffset(t.position, defaultOffset);
            _switchStateSeq.Append(Camera.transform.DOMove(targetPos, _targetMoveInDuration).SetEase(_targetMoveInCurve));
            _switchStateSeq.Join(Camera.DOOrthoSize(orthoSize, _targetZoomInDuration).SetEase(_targetZoomInCurve));
            _switchStateSeq.SetUpdate(UpdateType.Late);
            _switchStateSeq.onKill = () =>
            {
                _switchStateSeq = null;
            };
        }

        public void SwitchToDefaultState(bool returnToPrevPos = true)
        {
            //CurrentCenterPlanePosition = PlanePosition(new Vector2(Screen.width / 2f, Screen.height / 2f));

            _currentViewTarget = null;
            _switchStateSeq?.Kill();

            _switchStateSeq = DOTween.Sequence();
            if (!returnToPrevPos)
            {
                _defaultStatePosition = CurrentCenterPlanePosition - Camera.transform.forward * _camDistanceFromTarget;
            }
            _switchStateSeq.Join(Camera.transform.DOMove(_defaultStatePosition, _targetMoveOutDuration).SetEase(_targetMoveOutCurve));
            _switchStateSeq.Join(Camera.DOOrthoSize(_defaultStateOrthoSize, _targetZoomOutDuration).SetEase(_targetZoomOutCurve));
            _switchStateSeq.SetUpdate(UpdateType.Late);
            _switchStateSeq.onKill = () =>
            {
                _switchStateSeq = null;
                _state = CameraStates.Default;
            };
        }

        public void SwitchToFollow(Transform followTarget)
        {
            _state = CameraStates.Following;
            _followTarget = followTarget;
        }

        private Vector3 PlanePositionDelta(Vector3 mousePos)
        {
            //not moved
            if (_prevCursorPos == mousePos)
            {
                return Vector3.zero;
            }

            //delta
            Ray rayBefore = Camera.ScreenPointToRay(mousePos - (mousePos - _prevCursorPos));
            Ray rayNow = Camera.ScreenPointToRay(mousePos);

            if (_plane.Raycast(rayBefore, out var enterBefore) && _plane.Raycast(rayNow, out var enterNow))
            {
                return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);
            }

            //not on plane
            return Vector3.zero;
        }

        private Vector3 PlanePosition(Vector2 screenPos)
        {
            Ray rayNow = Camera.ScreenPointToRay(screenPos);
            return _plane.Raycast(rayNow, out var enterNow) ? rayNow.GetPoint(enterNow) : Vector3.zero;
        }

        public void MoveToPosition(Vector3 position, bool instantly = false, bool ignoreOffset = false)
        {
            Vector3 target = ignoreOffset ? position : GetTargetPosWithOffset(position, false);
            _currentTargetPoint = target;

            DOVirtual.Float(0, 1, instantly ? 0 : 1, (value) =>
            {
                Vector3 cameraPosition = Camera.transform.position;
                cameraPosition = Vector3.Lerp(cameraPosition, target, value);
                Camera.transform.position = cameraPosition;
                _currentTargetPoint = cameraPosition;
            });
        }

        public void ZoomTo(float value)
        {
            _currentTargetZoom = Mathf.Clamp(value, _zoomMinSize, _zoomMaxSize);
        }

        private Vector3 GetTargetPosWithOffset(Vector3 position, bool defaultOffset)
        {
            Vector3 offset = defaultOffset ? _closeViewOffset : Vector3.zero;
            offset += offset * (position.y * _offsetYK);
            Vector3 targetPos = position + offset - transform.rotation * Vector3.forward * _camDistanceFromTarget;
            targetPos.y = _cameraHeight;
            return targetPos;
        }
    }
}