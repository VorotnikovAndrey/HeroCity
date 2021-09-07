using System;
using System.Collections.Generic;
using System.Linq;
using Source;
using UnityEngine;

namespace Utils.Pathfinding
{
    public class WaypointsContainer : MonoBehaviour, ICachingObject
    {
        public List<MapWaypoint> Waypoints;

        public void CacheFromEditor()
        {
            CacheWaypoints();
        }

        [ContextMenu("Cache")]
        public void CacheWaypoints()
        {
            Waypoints.Clear();

            var array = transform.root.GetComponentsInChildren<MapWaypoint>().ToList();

            foreach (MapWaypoint element in array)
            {
                Waypoints.Add(element);

                element.Validate();

                MapWaypoint duplicate = Waypoints.FirstOrDefault(item => item != element &&
                                                                         Vector3.Distance(element.Position, item.Position) < 0.05f);
                if (duplicate)
                {
                    Debug.LogError($"Waypoint duplicate: ind {Waypoints.IndexOf(element)}, ind {Waypoints.IndexOf(duplicate)}");
                }
            }
        }

        public List<MapWaypoint> GetTypePositions(MapWaypointType type)
        {
            return Waypoints.Where(x => x.WaypointType == type).ToList();
        }

        public MapWaypoint FindClosestWaypoint(Vector3 target, Func<MapWaypoint, bool> check = null, MapWaypointType type = MapWaypointType.Undefined)
        {
            List<MapWaypoint> array = Waypoints;

            MapWaypoint closest = null;
            float closestDist = Mathf.Infinity;

            for (int i = 0; i < array.Count; i++)
            {
                MapWaypoint waypoint = array[i];
                if (waypoint == null)
                {
                    Debug.LogError("Null waypoint. Waypoints container recache needed.");
                    continue;
                }

                if (check != null && !check.Invoke(waypoint))
                    continue;

                float dist = (waypoint.transform.position - target).magnitude;
                if (dist < closestDist)
                {
                    closest = waypoint;
                    closestDist = dist;
                }
            }

            return closest != null ? closest : null;
        }

#if UNITY_EDITOR
        public void MakeReferences()
        {
            CacheWaypoints();
        }
#endif
    }
}
