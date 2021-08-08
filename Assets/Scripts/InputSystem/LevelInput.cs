using System.Collections.Generic;
using System.Linq;
using CameraSystem;
using InputSyatem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InputSystem
{
    public class LevelInput : IInputSystem
    {
        private const float MinDistance = 15f;
        private readonly HashSet<object> _inputBlockers = new HashSet<object>();
        private Vector3 _cursorDownPosition;

        public bool IsInputLocked => _inputBlockers.Count > 0;

        private LevelCamera _gameCamera;
        private LevelCamera GameCamera
        {
            get
            {
                return _gameCamera ??= ProjectContext.Instance.Container.Resolve<LevelCamera>();
            }
        }

        public void Update(float deltaTime, float totalTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _cursorDownPosition = Input.mousePosition;
            }

            if (!Input.GetMouseButtonUp(0) || !(Vector3.Distance(_cursorDownPosition, Input.mousePosition) < MinDistance))
            {
                return;
            }

            if (!TryPointerCast(out RaycastHit hit))
            {
                return;
            }

            ProcessHit(hit);
        }

        public void LockInput(object blocker)
        {
            _inputBlockers.Add(blocker);
        }

        public void UnlockInput(object blocker)
        {
            _inputBlockers.Remove(blocker);
        }

        private void ProcessHit(RaycastHit hit)
        {
            if (TrySelectBuildingEmployeeSign(hit))
            {
                return;
            }

            if (TrySelectBuilding(hit))
            {
                return;
            }

            if (IsInputLocked)
            {
                return;
            }

            hit.transform.GetComponent<IClickableView>()?.ProcessClick();
        }

        private bool TryPointerCast(out RaycastHit hit)
        {
            hit = default;
            var results = new List<RaycastResult>();
            PointerEventData pointer = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            EventSystem.current.RaycastAll(pointer, results);
            var canProcess = results.All(result => result.gameObject.GetComponent<ICanvasRaycastFilter>() == null);

            if (!canProcess)
            {
                return false;
            }

            Ray ray = GameCamera.Camera.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(ray, out hit);
        }

        private bool TrySelectBuilding(RaycastHit hit)
        {
            //var building = hit.transform.GetComponent<BuildingViewHitbox>();

            //if (building)
            //{
            //    if (building.View == null)
            //    {
            //        Debug.LogError(
            //            $"No view for building hitbox. Parent: {building.transform.parent.name}, Pos: {building.transform.position}");
            //        return true;
            //    }

            //    PublishOnView(new EventBuildingViewSelection { View = building.View, TapPosition = hit.point });
            //    return true;
            //}

            return false;
        }

        private bool TrySelectBuildingEmployeeSign(RaycastHit hit)
        {
            //var sign = hit.transform.GetComponent<BuildingEmployeeSignHitbox>();

            //if (sign != null)
            //{
            //    if (sign.BuildingView == null)
            //    {
            //        Debug.LogError($"No view for BuildingEmployeeSignHitbox. Parent: {sign.transform.parent.name}, Pos: {sign.transform.position}");
            //        return false;
            //    }

            //    PublishOnView(new EventBuildingViewEmployeeSelection { View = sign.BuildingView });
            //    return true;
            //}

            return false;
        }
    }
}