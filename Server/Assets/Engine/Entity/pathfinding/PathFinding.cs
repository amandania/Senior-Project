using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is no longer used, it is considered Deprecaed code. We use default nav mesh agent to do any pathfinding.
/// </summary>
public class PathFinding : IPathFinding
{
    /// <summary>
    /// The container grid we load on container build
    /// </summary>
    public ILoadMapData Grid { get; }


    /// <summary>
    /// We constructed pathfinding class as a single instance. <see cref="NetworkManager.RegisterDependencies(Autofac.ContainerBuilder)"/>
    /// We have Pathfinding as a dependecy and LoadMapData. Load MapData is also a startable.
    /// </summary>
    /// <param name="globalGrid"></param>
    public PathFinding(ILoadMapData globalGrid)
    {
        Grid = globalGrid;
    }


    /// <summary>
    /// This function retrun a list of waypoints given a start position and a target position.
    /// We use a* algorithm on a heap and sort accordingly to get our path in a O(Logn) time
    /// </summary>
    /// <param name="startPos">StartVector 3 position to get a NodeFromWorldPoint to</param>
    /// <param name="targetPos">End Vector 3 position to get a NodeFromWorldPoint </param>
    /// <returns>Path result data contain waypoints</returns>
    public PathResult FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = Grid.NodeFromWorldPoint(startPos);
        Node targetNode = Grid.NodeFromWorldPoint(targetPos);

        startNode.m_parent = startNode;


        if (startNode.m_walkable && targetNode.m_walkable)
        {
            CustomHeap<Node> openSet = new CustomHeap<Node>(Grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    //print ("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in Grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.m_walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.m_gCost + GetDistance(currentNode, neighbour) + neighbour.m_movementPenalty;
                    if (newMovementCostToNeighbour < neighbour.m_gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.m_gCost = newMovementCostToNeighbour;
                        neighbour.m_hCost = GetDistance(neighbour, targetNode);
                        neighbour.m_parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        return new PathResult(waypoints, pathSuccess);
    }

    /// <summary>
    /// Class to Represent a path that is finished
    /// </summary>
    public class PathResult
    {
        public Vector3[] path;
        public bool success;

        public PathResult(Vector3[] path, bool success)
        {
            this.path = path;
            this.success = success;
        }
    }

    /// <summary>
    /// This function starts at the head of our list and traverse backwards to trace our path because we will have extra nodes sometimes when reaching the goal.
    /// So we retrace the shortest path from  here.
    /// </summary>
    /// <param name="startNode">start node to compare end has been reachd</param>
    /// <param name="endNode">The node to check the parent of each time till we meet start node.</param>
    /// <returns></returns>
    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.m_parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    /// <summary>
    /// This function siplyify our path so that we have the shortest directions to the the path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].m_gridX - path[i].m_gridX, path[i - 1].m_gridY - path[i].m_gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].m_worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }


    /// <summary>
    /// Historic functio used to set fcost values for pathfinding
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns></returns>
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Math.Abs(nodeA.m_gridX - nodeB.m_gridX);
        int dstY = Math.Abs(nodeA.m_gridY - nodeB.m_gridY);

        if (dstX > dstY)
            return 6 * dstY + 2 * (dstX - dstY);
        return 6 * dstX + 2 * (dstY - dstX);
    }

}