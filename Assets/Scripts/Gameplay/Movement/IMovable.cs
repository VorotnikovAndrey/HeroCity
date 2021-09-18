using System;
using Source;
using UnityEngine;
using Utils.ObjectPool;

namespace Gameplay.Movement
{
    public interface IMovable
    {
        EventVariable<bool> IsMoving { get; }

        void Initialize(IView view, Vector3 startPosition, float speed);
        void DeInitialize();
        void GoTo(Vector3 destination, Action<bool> callback = null);
        void Warp(Vector3 destination, bool callbackSuccess = false);
        void Stop(bool invokeCallback = true);
        void Update();
    }
}