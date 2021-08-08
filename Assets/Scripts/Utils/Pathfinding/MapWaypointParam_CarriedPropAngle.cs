using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Utils.Pathfinding;

namespace Source
{
    public class MapWaypointParam_CarriedPropAngle : MapWaypointParam
    {
        public MapWaypointVector3Dictionary Angles;

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Handles.Label(transform.position + Vector3.up * 0.3f, "CarriedProp");
        }
#endif
    }

    [Serializable]
    public class MapWaypointVector3Dictionary : SerializableDictionary<MapWaypoint, Vector3> { }
}
