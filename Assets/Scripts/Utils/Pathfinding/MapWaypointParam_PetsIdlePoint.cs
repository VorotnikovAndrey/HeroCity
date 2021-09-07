using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Utils.Pathfinding;

namespace Source
{
    public class MapWaypointParam_PetsIdlePoint : MapWaypointParam
    {
        public bool IsOccupied;

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Handles.Label(transform.position + Vector3.up * 0.3f, "Pets Idle Point");
        }
#endif
    }
}