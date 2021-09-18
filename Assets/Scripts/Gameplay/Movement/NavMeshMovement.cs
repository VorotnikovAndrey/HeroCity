using System;
using Gameplay.Characters;
using Source;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Utils.ObjectPool;

namespace Gameplay.Movement
{
    public class NavMeshMovement : IMovable
    {
        private BaseCharacterView _view;
        private NavMeshAgent _agent;
        private Action<bool> _callback;

        public EventVariable<bool> IsMoving { get; } = new EventVariable<bool>();

        public void Initialize(IView view, Vector3 startPosition, float speed)
        {
            _view = view as BaseCharacterView;

            if (_view == null)
            {
                Debug.LogError("View is null".AddColorTag(Color.red));
                return;
            }

            _view.AnimatorController.ConnectToMovement(this);

            _agent = _view.Transform.GetComponent<NavMeshAgent>();
            _agent.enabled = true;
            _agent.speed = speed;
        }

        public void DeInitialize()
        {
            _view.AnimatorController.DisconnectFromMovement();

            _view = null;
            _agent.isStopped = true;
            _agent.enabled = false;
            _agent = null;
        }

        public void GoTo(Vector3 destination, Action<bool> callback = null)
        {
            _agent.isStopped = false;
            _agent.SetDestination(destination);

            _callback = callback;
            IsMoving.Value = true;
        }

        public void Warp(Vector3 destination, bool callbackSuccess = false)
        {
            _agent.Warp(destination);
        }

        public void Stop(bool invokeCallback = true)
        {
            _agent.isStopped = true;
            IsMoving.Value = false;
        }

        public void Update()
        {
            if (_view == null)
            {
                return;
            }

            CheckDestination();
        }

        private void CheckDestination()
        {
            if (_agent.pathPending)
            {
                return;
            }

            if (!(_agent.remainingDistance <= _agent.stoppingDistance))
            {
                return;
            }

            if (_agent.hasPath && _agent.velocity.sqrMagnitude != 0f)
            {
                return;
            }

            IsMoving.Value = false;
            _callback?.Invoke(true);
        }
    }
}