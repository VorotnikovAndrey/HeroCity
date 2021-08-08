using UnityEngine;

namespace Source
{
    public class VirtualWaypoint : IWaypoint
    {
        public Vector3 Position { get; }

        public VirtualWaypoint(Vector3 position)
        {
            Position = position;
        }
    }

}
