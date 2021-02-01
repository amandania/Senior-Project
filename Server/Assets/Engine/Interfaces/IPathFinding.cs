using UnityEngine;
using static Engine.Entity.pathfinding.PathFinding;

namespace Engine.Interfaces
{
    public interface IPathFinding
    {
        ILoadMapData Grid { get; }
        PathResult FindPath(Vector3 startPos, Vector3 targetPos);
    }
}