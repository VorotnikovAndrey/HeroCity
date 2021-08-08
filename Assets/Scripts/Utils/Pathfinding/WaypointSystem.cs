using System.Collections.Generic;
using Source;
using UnityEngine;

namespace Utils.Pathfinding
{
    public class WaypointSystem
    {
        private WaypointsContainer _waypointsContainer;
        private PathFinder _pathFinder;

        private void Init()
        {
            _waypointsContainer = new WaypointsContainer();// Должен быть на карте
            _pathFinder = new PathFinder(_waypointsContainer);
        }

        public List<IWaypoint> GetPath(Vector3 fromPos, Vector3 targetPos, MapWaypointType type = MapWaypointType.Undefined)
        {
            if (_pathFinder == null)
            {
                Debug.LogError($"No pathfinder in WaypointSystem. Unhandled GetPath call. From {fromPos} to {targetPos}");
                return new List<IWaypoint>();
            }
            return _pathFinder.GetPath(fromPos, targetPos, type);
        }
    }
}
