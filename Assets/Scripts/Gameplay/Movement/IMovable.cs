using System;
using UnityEngine;

namespace Gameplay.Movement
{
    public interface IMovable
    {
        void Initialize(Transform view, Vector3 startPosition, float speed);
        void GoTo(Vector3 destination, Action<bool> callback = null);
        void Warp(Vector3 destination, bool callbackSuccess = false);
        void Stop(bool invokeCallback = true);
        void Update();
    }
}