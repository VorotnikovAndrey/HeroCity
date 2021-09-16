using System;
using System.Collections.Generic;
using DG.Tweening;
using Source;
using UnityEngine;
using Utils.Pathfinding;
using Zenject;

namespace Gameplay.Movement
{
    public class WaypointMovement : IMovable
    {
        public EventVariable<bool> IsMoving { get; } = new EventVariable<bool>();

        private Stack<IWaypoint> _currentPath;
        private IWaypoint _currentStartWaypoint;
        private IWaypoint _currentEndWaypoint;
        protected Vector3 _currentTargetPos;
        private float _moveTimeTotal;
        private float _moveTimeCurrent;
        private Action<bool> _movementEndCallback;
        private Tweener _rotateTweener;
        private Transform _viewTransform;
        private WaypointSystem _waypointSystem;

        public float MovementSpeed { get; private set; }

        public void Initialize(Transform view, Vector3 startPosition, float speed)
        {
            _viewTransform = view;
            _currentTargetPos = startPosition;
            MovementSpeed = speed;
            _waypointSystem = ProjectContext.Instance.Container.Resolve<WaypointSystem>();
        }

        public void Update()
        {
            if (_viewTransform == null)
            {
                return;
            }

            UpdateMovement();
            UpdateCurrentPosition();

            IsMoving.Value = _currentPath?.Count > 0;
        }

        public void GoTo(Vector3 destination, Action<bool> callback = null)
        {
            _movementEndCallback?.Invoke(false);
            _movementEndCallback = callback;

            var path = GetPath(destination);
            if (path == null)
            {
                _currentPath = null;
                InvokeMovementEndCallback();
                return;
            }
            Stop(false);

            if (path.Count > 1)
            {
                if (path[path.Count - 1] == _currentStartWaypoint &&
                    path[path.Count - 2] == _currentEndWaypoint ||
                    path[path.Count - 1] == _currentEndWaypoint &&
                    path[path.Count - 2] == _currentStartWaypoint ||
                    path[path.Count - 1] == _currentEndWaypoint &&
                    _currentStartWaypoint == _currentEndWaypoint)
                {
                    path.RemoveAt(path.Count - 1);
                }
            }

            _currentPath = new Stack<IWaypoint>();

            foreach (IWaypoint waypoint in path)
            {
                _currentPath.Push(waypoint);
            }

            _currentPath.Push(new VirtualWaypoint(_currentTargetPos));
        }

        protected virtual List<IWaypoint> GetPath(Vector3 destination)
        {
            return _waypointSystem.GetPath(_currentTargetPos, destination);
        }

        public void GoToDirect(Vector3 destination, Action<bool> callback = null)
        {
            _movementEndCallback?.Invoke(false);
            _movementEndCallback = callback;

            Stop(false);

            _currentPath = new Stack<IWaypoint>();
            _currentPath.Push(new VirtualWaypoint(destination));
            _currentPath.Push(new VirtualWaypoint(_currentTargetPos));
        }

        public void Warp(Vector3 destination, bool callbackSuccess = false)
        {
            InvokeMovementEndCallback(callbackSuccess);
            Stop(false);

            _viewTransform.position = destination;
            _currentTargetPos = destination;
        }

        private void UpdateMovement()
        {
            if (_currentPath == null || _currentPath.Count == 0)
            {
                return;
            }

            if (_moveTimeCurrent < _moveTimeTotal)
            {
                _moveTimeCurrent += UnityEngine.Time.deltaTime;

                if (_moveTimeCurrent > _moveTimeTotal)
                {
                    _moveTimeCurrent = _moveTimeTotal;
                }

                _currentTargetPos = Vector3.Lerp(
                    _currentStartWaypoint.Position,
                    _currentPath.Peek().Position,
                    _moveTimeCurrent / _moveTimeTotal);
            }
            else
            {
                if (_currentPath.Count > 0)
                {
                    _currentTargetPos = _currentPath.Peek().Position;
                }

                _currentStartWaypoint = _currentPath.Pop();

                if (_currentPath.Count == 0)
                {
                    Stop(false);
                    InvokeMovementEndCallback();
                }
                else
                {
                    _currentEndWaypoint = _currentPath.Peek();
                    _rotateTweener?.Kill();
                    _rotateTweener = _viewTransform.DOLookAt(_currentEndWaypoint.Position, 0.5f, AxisConstraint.Y).OnComplete(
                        () =>
                        {
                            _rotateTweener = null;
                        });

                    _moveTimeCurrent = 0;
                    _moveTimeTotal = Vector3.Distance(_currentStartWaypoint.Position, _currentEndWaypoint.Position) / MovementSpeed;
                }
            }
        }

        private void UpdateCurrentPosition()
        {
            _viewTransform.position = Vector3.Distance(_currentTargetPos, _viewTransform.position) > 1 
                ? _currentTargetPos
                : Vector3.Lerp(_viewTransform.position, _currentTargetPos, UnityEngine.Time.deltaTime * 10);
        }

        public void Stop(bool invokeCallback = true)
        {
            _currentPath = null;
            _moveTimeTotal = 0;
            _moveTimeCurrent = 0;

            if (invokeCallback)
            {
                InvokeMovementEndCallback(false);
            }
        }

        public void SetSpeedCustom(float speed)
        {
            MovementSpeed = speed;
        }

        private void InvokeMovementEndCallback(bool success = true)
        {
            var prevCallback = _movementEndCallback;
            _movementEndCallback = null;
            prevCallback?.Invoke(success);
        }
    }
}
