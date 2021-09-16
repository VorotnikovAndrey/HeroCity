using System;
using Source;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Movement
{
    public class NavMeshMovement : IMovable
    {
        private Transform _viewTransform;
        private NavMeshAgent _agent;
        private EventVariable<bool> Destinated;

        public void Initialize(Transform view, Vector3 startPosition, float speed)
        {
            _viewTransform = view;
            _agent = _viewTransform.transform.GetComponent<NavMeshAgent>();
            _agent.speed = speed;
            _agent.Warp(startPosition);
        }

        public void GoTo(Vector3 destination, Action<bool> callback = null)
        {
            _agent.SetDestination(destination);

            Destinated = new EventVariable<bool>();
            Destinated.AddListener(callback);
        }

        public void Warp(Vector3 destination, bool callbackSuccess = false)
        {
            _agent.Warp(destination);
        }

        public void Stop(bool invokeCallback = true)
        {
            _agent.isStopped = true;
        }

        public void Update()
        {
            if (_viewTransform == null)
            {
                return;
            }

            CheckDestination();
        }

        private void CheckDestination()
        {
            if (Destinated == null)
            {
                return;
            }

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

            Destinated.Value = true;
            Destinated = null;
        }
    }
}