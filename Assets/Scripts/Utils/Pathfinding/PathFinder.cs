using System.Collections.Generic;
using Source;
using UnityEngine;

namespace Utils.Pathfinding
{
    public class PathFinder
    {
        private readonly WaypointsContainer _waypointsContainer;

        public PathFinder(WaypointsContainer waypointsContainer)
        {
            _waypointsContainer = waypointsContainer;
        }

        public List<IWaypoint> GetPath(Vector3 curPos, Vector3 targetPos, MapWaypointType type = MapWaypointType.Undefined)
        {
            var dirtyNodes = new HashSet<MapWaypoint>();
            var currentNode = _waypointsContainer.FindClosestWaypoint(curPos, null, type);
            var endNode = _waypointsContainer.FindClosestWaypoint(targetPos, null, type);
            var path = GetPathInternal(currentNode, endNode);
            var counter = 0;
            const int maxIterations = 100;
            while (path.Count == 0 && counter < maxIterations)
            {
                dirtyNodes.Add(endNode);
                endNode = _waypointsContainer.FindClosestWaypoint(targetPos, wp => !dirtyNodes.Contains(wp), type);
                path = GetPathInternal(currentNode, endNode);
                counter++;
            }
            return path;
        }

        private List<IWaypoint> GetPathInternal(MapWaypoint currentNode, MapWaypoint endNode)
        {
            var path = new List<IWaypoint>();

            if (currentNode == null || endNode == null)
                return null;
            if (currentNode == endNode)
                return new List<IWaypoint> { endNode };

            var openList = new SortedList<float, MapWaypoint>();
            var distHash = new HashSet<float>();
            var nodeHash = new HashSet<MapWaypoint>();
            var closedList = new HashSet<MapWaypoint>();
            openList.Add(0, currentNode);
            distHash.Add(0);
            nodeHash.Add(currentNode);
            currentNode.previous = null;
            currentNode.heuristicDist = 0f;
            while (openList.Count > 0)
            {
                currentNode = openList.Values[0];
                distHash.Remove(openList.Keys[0]);
                nodeHash.Remove(currentNode);
                openList.RemoveAt(0);
                var dist = currentNode.heuristicDist;
                closedList.Add(currentNode);
                if (currentNode == endNode)
                {
                    break;
                }
                foreach (var neighbor in currentNode.Neighbors)
                {
                    if (closedList.Contains(neighbor) || nodeHash.Contains(neighbor))
                        continue;
                    neighbor.previous = currentNode;
                    neighbor.heuristicDist = dist + (neighbor.transform.position - currentNode.transform.position).magnitude;
                    var distanceToTarget = (neighbor.transform.position - endNode.transform.position).magnitude;
                    var totalHeuristicDist = neighbor.heuristicDist + distanceToTarget;
                    while (distHash.Contains(totalHeuristicDist))
                        totalHeuristicDist += 0.01f;
                    openList.Add(totalHeuristicDist, neighbor);
                    nodeHash.Add(neighbor);
                    distHash.Add(totalHeuristicDist);
                }
            }
            if (currentNode == endNode)
            {
                while (currentNode.previous != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.previous;
                }
                path.Add(currentNode);
            }
            return path;
        }
    }
}
