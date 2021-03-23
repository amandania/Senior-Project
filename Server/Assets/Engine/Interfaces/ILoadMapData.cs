using Autofac;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This was a starable interface used to build our map container which was used for pathfinding behaviors. We no longer use this  because we switch to navmesh pathfiding.
/// <see cref="MapData"/>
/// </summary>
public interface ILoadMapData : IStartable, IDisposable
{
    Node NodeFromWorldPoint(Vector3 a_worldPosition);
    List<Node> GetNeighbours(Node a_node);
    int MaxSize { get; }
}
