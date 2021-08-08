using UnityEditor;
using UnityEngine;

namespace Source
{
    public class MapWaypointParam_OverrideRotation : MapWaypointParam
    {
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            //Handles.Label(transform.position + Vector3.up * 0.3f, "OverrideRot");
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.3f);
            Gizmos.DrawSphere(transform.position + Vector3.up * 0.3f, 0.12f);
            Gizmos.color = Color.blue;
            var exitPosUp = transform.position + Vector3.up * 0.33f;
            var exitPosForwardUp = transform.position + Vector3.up * 0.33f + transform.forward * 0.25f;
            Gizmos.DrawLine(exitPosUp, exitPosForwardUp);
            Gizmos.DrawCube(exitPosForwardUp, Vector3.one * 0.07f);
        }
#endif
    }
}