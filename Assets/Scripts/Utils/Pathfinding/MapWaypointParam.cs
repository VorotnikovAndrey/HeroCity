using UnityEngine;
using Utils.Extensions;

namespace Utils.Pathfinding
{
    [RequireComponent(typeof(MapWaypoint))]
    public class MapWaypointParam : MonoBehaviour
    {
        protected virtual void Reset()
        {
            MapWaypoint wp = GetComponent<MapWaypoint>();
            wp.Params.AddIfUnique(this);
        }

        protected virtual void OnDrawGizmos()
        {
        }
    }
}
