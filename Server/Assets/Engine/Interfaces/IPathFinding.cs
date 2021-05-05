using UnityEngine;
using static PathFinding;
/// <summary>
/// Pathfinding interface is deprecated code.
/// We had it as interface to build it as part of our server dependencies. <see cref="NetworkManager.RegisterDependencies(Autofac.ContainerBuilder)"/>
/// </summary>
public interface IPathFinding
{
    //Dependecy for startable class
    ILoadMapData Grid { get; }

    //Function to return our path data
    PathResult FindPath(Vector3 a_startPos, Vector3 a_targetPos);
}