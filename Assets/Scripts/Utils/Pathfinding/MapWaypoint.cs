using System.Collections.Generic;
using System.Linq;
using Source;
using UnityEngine;

namespace Utils.Pathfinding
{
    public enum MapWaypointType
    {
        Undefined = 0,
        GuestSpawn = 1,
        MountGuestSpawn = 2,
        TransportExit = 3,
        RichGuySpawn = 4,
        RichGuyTarget = 5,
        Manager = 6,
        RoomEntrance = 7,
        Queue = 8, // todo: add
        Pets = 9,
    }

    public enum MapWaypointInteractionType
    {
        None = 0,
        Room = 1,
        Reception = 2,
        Parking = 3,
        Restaurant = 4,
        Bar = 5,
    }

    [ExecuteAlways]
    public class MapWaypoint : MonoBehaviour, IWaypoint
    {
        public MapWaypointType WaypointType = MapWaypointType.Undefined;
        public List<MapWaypoint> Neighbors = new List<MapWaypoint>();
        public List<MapWaypointParam> Params = new List<MapWaypointParam>();

        [HideInInspector] public Transform Transform;

        public Vector3 Position => transform.position;

        [HideInInspector] public MapWaypoint previous;
        [HideInInspector] public float heuristicDist;
        [HideInInspector] public List<string> buildingIds;

        public T GetParam<T>() where T : MapWaypointParam
        {
            return (T)Params.FirstOrDefault(param => param is T);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = GetGizmosColor();
            Gizmos.DrawWireSphere(transform.position, 0.125f);
            Gizmos.DrawSphere(transform.position, 0.1f);

            Gizmos.color = Color.white.SetAlpha(0.2f);
            foreach (var neighbor in Neighbors)
            {
                if (neighbor == null)
                    continue;
                Gizmos.DrawLine(
                    transform.position + Vector3.up * 0.05f, 
                    neighbor.transform.position + Vector3.up * 0.05f);
            }
        }

        private Color GetGizmosColor()
        {
            switch (WaypointType)
            {
                case MapWaypointType.GuestSpawn: return Color.blue;
                case MapWaypointType.MountGuestSpawn: return Color.cyan;
                case MapWaypointType.TransportExit: return Color.cyan;
                case MapWaypointType.RichGuySpawn: return Color.yellow;
                case MapWaypointType.RichGuyTarget: return Color.yellow;
                case MapWaypointType.Manager: return Color.red;
                case MapWaypointType.RoomEntrance: return Color.green;
                case MapWaypointType.Queue: return Color.magenta; 
                case MapWaypointType.Pets: return Color.black;
            }
            return Color.white.SetAlpha(0.5f);
        }

        private void OnValidate()
        {
            Validate();

            Transform = GetComponent<Transform>();
        }

        public void Validate()
        {
            Neighbors = Neighbors.Distinct().ToList();
            Neighbors.RemoveAll(waypoint => waypoint == null);
            foreach (var neighbor in Neighbors)
            {
                if (!neighbor.Neighbors.Contains(this))
                {
                    neighbor.Neighbors.Add(this);
                }
            }

            for (int i = 0; i < Neighbors.Count; i++)
            {
                if (Neighbors[i] == null)
                {
                    Neighbors.RemoveAt(i);
                }
            }
        }
        
#if UNITY_EDITOR
        private void OnDestroy()
        {
            var container = transform.GetComponentInParent<WaypointsContainer>();
            if (container != null)
                container.CacheWaypoints();
        }

        public void TypeChanged()
        {
            gameObject.name = $"Waypoint_{WaypointType}";
            
            var container = transform.GetComponentInParent<WaypointsContainer>();
            if (container != null)
                container.MakeReferences();
        }
        
        public void SetAsEntranceTo(string buildingId)
        {
            WaypointType = MapWaypointType.RoomEntrance;
            TypeChanged();
            
            var container = transform.GetComponentInParent<WaypointsContainer>();
            if (container != null)
                container.SetWaypointAsEntranceTo(this, buildingId);
        }
#endif

    }
}
