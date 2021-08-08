using UnityEngine;
using Utils.Pathfinding;

namespace Source
{
    [RequireComponent(typeof(MapWaypoint))]
    public class MapWaypointParam : MonoBehaviour
    {
        protected virtual void Reset()
        {
            var wp = GetComponent<MapWaypoint>();
            wp.Params.AddIfUnique(this);
        }

        protected virtual void OnDrawGizmos()
        {
        }
    }
}
