using System.Collections.Generic;
using System.Linq;
using Source;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Pathfinding
{
    public enum MapWaypointType
    {
        Undefined = 0,
        Enter = 1,
        Exit = 2,
    }

    [ExecuteAlways]
    public class MapWaypoint : MonoBehaviour, IWaypoint
    {
        public MapWaypointType WaypointType = MapWaypointType.Undefined;
        public List<MapWaypoint> Neighbors = new List<MapWaypoint>();
        public List<MapWaypointParam> Params = new List<MapWaypointParam>();

        [HideInInspector] public Transform Transform;

        public Vector3 Position => Transform.position;
        public bool Locked;

        [HideInInspector] public MapWaypoint previous;
        [HideInInspector] public float heuristicDist;

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
            foreach (MapWaypoint neighbor in Neighbors)
            {
                if (neighbor == null)
                {
                    continue;
                }

                Gizmos.DrawLine(
                    transform.position + Vector3.up * 0.05f, 
                    neighbor.transform.position + Vector3.up * 0.05f);
            }

            Color GetGizmosColor()
            {
                switch (WaypointType)
                {
                    case MapWaypointType.Enter: return Color.yellow;
                }
                return Color.white.SetAlpha(0.5f);
            }
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
            foreach (MapWaypoint neighbor in Neighbors)
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
            WaypointsContainer container = transform.GetComponentInParent<WaypointsContainer>();
            if (container != null)
                container.CacheWaypoints();
        }

        public void TypeChanged()
        {
            gameObject.name = $"MapWaypoint_{WaypointType}_{transform.GetSiblingIndex()}";
            
            WaypointsContainer container = transform.GetComponentInParent<WaypointsContainer>();
            if (container != null)
                container.MakeReferences();
        }
#endif
    }
}
