using Engine.Interfaces;
using UnityEngine;
using static Engine.Entity.pathfinding.PathFinding;

public interface IPathFinding
{
    ILoadMapData Grid { get; }
    PathResult FindPath(Vector3 startPos, Vector3 targetPos);
}