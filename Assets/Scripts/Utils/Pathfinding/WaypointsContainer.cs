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
        public List<MapWaypoint> PetsWaypoints;

        public void CacheFromEditor()
        {
            CacheWaypoints();
        }

        [ContextMenu("Cache")]
        public void CacheWaypoints()
        {
            Waypoints.Clear();
            PetsWaypoints.Clear();

            var array = transform.root.GetComponentsInChildren<MapWaypoint>().ToList();

            foreach (var element in array)
            {
                switch (element.WaypointType)
                {
                    case MapWaypointType.Pets:
                        PetsWaypoints.Add(element);
                        break;
                    default:
                        Waypoints.Add(element);
                        break;
                }

                element.Validate();

                var duplicate = Waypoints.FirstOrDefault(item => item != element &&
                                                                 Vector3.Distance(element.Position, item.Position) < 0.05f);
                if (duplicate)
                {
                    Debug.LogError($"Waypoint duplicate: ind {Waypoints.IndexOf(element)}, ind {Waypoints.IndexOf(duplicate)}");
                }
            }
        }

        public MapWaypoint FindClosestWaypoint(Vector3 target, Func<MapWaypoint, bool> check = null, MapWaypointType type = MapWaypointType.Undefined)
        {
            List<MapWaypoint> array = null;

            switch (type)
            {
                case MapWaypointType.Pets:
                    array = PetsWaypoints;
                    break;
                default:
                    array = Waypoints;
                    break;
            }

            MapWaypoint closest = null;
            float closestDist = Mathf.Infinity;

            for (int i = 0; i < array.Count; i++)
            {
                var waypoint = array[i];
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
            
            var locView = transform.GetComponentInParent<Transform>(); // LocationView
            if (locView == null) return;
            
            foreach (var waypoint in Waypoints)
            {
                var wt = waypoint.transform;
                if (waypoint.WaypointType == MapWaypointType.Queue)
                {
                }
                else if (waypoint.WaypointType == MapWaypointType.Manager)
                {
                }
                else if (waypoint.WaypointType == MapWaypointType.RichGuySpawn)
                {
                    //locView.RichGuyCharacterSpawnPoint = wt;
                }
                else if (waypoint.WaypointType == MapWaypointType.RichGuyTarget)
                {
                   // locView.RichGuyCharacterTargetPoint = wt;
                }
            }

            var managerPoints = Waypoints.Where(x => x.WaypointType == MapWaypointType.Manager).ToList();
            //locView.ManagerCharacterPoints = managerPoints.Select(x => x.transform).ToList();
        }
        
        public void SetWaypointAsEntranceTo(MapWaypoint mapWaypoint, string buildingId)
        {
            //var locView = transform.GetComponentInParent<LocationView>();
            //if (locView == null) return;
            
            //var buildingView = locView.BuildingViews.FirstOrDefault(x => x.BuildingId == buildingId);
            //if (buildingView == null) return;

            //buildingView.GuestEntrancePoint = mapWaypoint.transform;
        }

#endif

    }
}
