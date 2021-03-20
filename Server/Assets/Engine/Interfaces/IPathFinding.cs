using UnityEngine;
using static PathFinding;

public interface IPathFinding
{
    ILoadMapData Grid { get; }
    PathResult FindPath(Vector3 startPos, Vector3 targetPos);
}