using Autofac;
using Engine.DataLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoadMapData : IStartable, IDisposable
{
    Node NodeFromWorldPoint(Vector3 worldPosition);
    List<Node> GetNeighbours(Node node);
    int MaxSize { get; }
}
