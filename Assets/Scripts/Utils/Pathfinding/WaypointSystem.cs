using System.Collections.Generic;
using Source;
using UnityEngine;
using Zenject;

namespace Utils.Pathfinding
{
    public class WaypointSystem
    {
        private WaypointsContainer _waypointsContainer;
        private PathFinder _pathFinder;

        public WaypointSystem()
        {
            ProjectContext.Instance.Container.Unbind<WaypointSystem>();
            ProjectContext.Instance.Container.BindInstances(this);
        }

        public void SetWaypointsContainer(WaypointsContainer waypointsContainer)
        {
            _waypointsContainer = waypointsContainer;
            _pathFinder = new PathFinder(_waypointsContainer);
        }

        public List<IWaypoint> GetPath(Vector3 fromPos, Vector3 targetPos, MapWaypointType type = MapWaypointType.Undefined)
        {
            if (_pathFinder != null)
            {
                return _pathFinder.GetPath(fromPos, targetPos, type);
            }

            Debug.LogError($"No pathfinder in WaypointSystem. Unhandled GetPath call. From {fromPos} to {targetPos}");
            return new List<IWaypoint>();
        }
    }
}
